using MEC.Data;
using MEC.Models;
using MEC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEC.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly AppDbContext _context;
        private readonly IEstoqueService _estoqueService;        
        public ProjetoService(AppDbContext context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        public async Task<Projeto> CriarProjeto(Projeto projeto, List<ItemProjeto> itens)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Calcular valor dos materiais
                projeto.ValorMateriais = await CalcularValorMateriais(itens);

                // Adicionar projeto
                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();

                // Adicionar itens
                foreach (var item in itens)
                {
                    item.ProjetoId = projeto.Id;
                    item.Status = await VerificarDisponibilidadeItem(item);
                    _context.ItensProjeto.Add(item);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return projeto;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ReservarMateriais(int projetoId)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in projeto.Itens.Where(i => i.Status == StatusItemProjeto.EmEstoque))
                {
                    // Reservar estoque - PASSANDO A TRANSAÇÃO
                    var sucesso = await _estoqueService.SaidaEstoque(
                        item.ProdutoId,
                        item.Quantidade,
                        $"Reserva para projeto {projeto.Codigo}",
                        transaction); // ← Passar a transação

                    if (sucesso)
                    {
                        item.Status = StatusItemProjeto.Reservado;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<StatusItemProjeto> VerificarDisponibilidadeItem(ItemProjeto item)
        {
            var produto = await _context.Produtos.FindAsync(item.ProdutoId);
            if (produto == null) return StatusItemProjeto.NecessitaCompra;

            if (produto is MaterialLinear materialLinear && item.MetrosLineares.HasValue)
            {
                return materialLinear.ComprimentoDisponivel >= item.MetrosLineares.Value
                    ? StatusItemProjeto.EmEstoque
                    : StatusItemProjeto.NecessitaCompra;
            }
            else
            {
                return produto.QuantidadeAtual >= item.Quantidade
                    ? StatusItemProjeto.EmEstoque
                    : StatusItemProjeto.NecessitaCompra;
            }
        }

        public async Task<decimal> CalcularValorMateriais(List<ItemProjeto> itens)
        {
            decimal total = 0;

            foreach (var item in itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto != null)
                {
                    if (produto is MaterialLinear && item.MetrosLineares.HasValue)
                    {
                        total += produto.PrecoVenda * (decimal)item.MetrosLineares.Value;
                    }
                    else
                    {
                        total += produto.PrecoVenda * item.Quantidade;
                    }
                }
            }

            return total;
        }

        public async Task<bool> AtualizarStatusProjeto(int projetoId, StatusProjeto novoStatus)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto == null) return false;

            projeto.Status = novoStatus;

            // Se projeto foi pago, calcular data de entrega (30 dias)
            if (novoStatus == StatusProjeto.Pago && projeto.DataPagamento.HasValue)
            {
                projeto.DataEntrega = projeto.DataPagamento.Value.AddDays(30);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LiberarMateriais(int projetoId)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in projeto.Itens.Where(i => i.Status == StatusItemProjeto.Reservado))
                {
                    // Liberar estoque - PASSANDO A TRANSAÇÃO
                    var movimento = new MovimentoEstoque
                    {
                        ProdutoId = item.ProdutoId,
                        Tipo = TipoMovimento.Entrada,
                        Quantidade = item.Quantidade,
                        Observacao = $"Liberação de reserva do projeto {projeto.Codigo}",
                        DataMovimento = DateTime.UtcNow
                    };

                    var sucesso = await _estoqueService.RegistrarMovimento(movimento, transaction);

                    if (sucesso)
                    {
                        item.Status = StatusItemProjeto.EmEstoque;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> AtualizarStatusItensProjeto(int projetoId)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null) return false;

            foreach (var item in projeto.Itens)
            {
                item.Status = await VerificarDisponibilidadeItem(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
