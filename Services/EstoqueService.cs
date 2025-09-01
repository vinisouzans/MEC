using MEC.Data;
using MEC.Models;
using MEC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEC.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly AppDbContext _context;

        public EstoqueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarMovimento(MovimentoEstoque movimento)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var produto = await _context.Produtos.FindAsync(movimento.ProdutoId);
                if (produto == null) return false;

                // Atualiza saldo do produto
                if (movimento.Tipo == TipoMovimento.Entrada || movimento.Tipo == TipoMovimento.Ajuste)
                {
                    produto.QuantidadeAtual += movimento.Quantidade;
                }
                else if (movimento.Tipo == TipoMovimento.Saida || movimento.Tipo == TipoMovimento.Corte)
                {
                    if (produto.QuantidadeAtual < movimento.Quantidade)
                        return false;

                    produto.QuantidadeAtual -= movimento.Quantidade;
                }

                // Atualiza datas
                if (movimento.Tipo == TipoMovimento.Entrada)
                    produto.DataUltimaEntrada = DateTime.UtcNow;
                else if (movimento.Tipo == TipoMovimento.Saida || movimento.Tipo == TipoMovimento.Corte)
                    produto.DataUltimaSaida = DateTime.UtcNow;

                _context.MovimentosEstoque.Add(movimento);
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

        public async Task<bool> EntradaEstoque(int produtoId, int quantidade, string? observacao = null)
        {
            var movimento = new MovimentoEstoque
            {
                ProdutoId = produtoId,
                Tipo = TipoMovimento.Entrada,
                Quantidade = quantidade,
                Observacao = observacao,
                DataMovimento = DateTime.UtcNow
            };

            return await RegistrarMovimento(movimento);
        }

        public async Task<bool> SaidaEstoque(int produtoId, int quantidade, string? observacao = null)
        {
            var movimento = new MovimentoEstoque
            {
                ProdutoId = produtoId,
                Tipo = TipoMovimento.Saida,
                Quantidade = quantidade,
                Observacao = observacao,
                DataMovimento = DateTime.UtcNow
            };

            return await RegistrarMovimento(movimento);
        }

        public async Task<bool> CortarMaterialLinear(int materialId, decimal metrosCortados, string responsavel, string? descricao = null)
        {
            var material = await _context.MateriaisLineares.FindAsync(materialId);
            if (material == null || material.ComprimentoDisponivel < metrosCortados)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                material.ComprimentoDisponivel -= metrosCortados;
                material.ComprimentoCortado += metrosCortados;

                var corte = new CorteMaterial
                {
                    MaterialLinearId = materialId,
                    ComprimentoCortado = metrosCortados,
                    DataCorte = DateTime.UtcNow,
                    Responsavel = responsavel,
                    Descricao = descricao
                };

                _context.CortesMateriais.Add(corte);
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

        public async Task<int> ConsultarSaldo(int produtoId)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);
            return produto?.QuantidadeAtual ?? 0;
        }
    }
}