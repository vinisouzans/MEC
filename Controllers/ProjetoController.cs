using Mapster;
using MEC.Data;
using MEC.DTOs.ItemProjeto;
using MEC.DTOs.Projeto;
using MEC.Models;
using MEC.Services;
using MEC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProjetoService _projetoService;

        public ProjetoController(AppDbContext context, IProjetoService projetoService)
        {
            _context = context;
            _projetoService = projetoService;
        }

        // POST: api/projeto
        [HttpPost]
        public async Task<ActionResult<ProjetoDTO>> CriarProjeto(ProjetoCreateDTO dto)
        {
            // Verificar se código já existe
            var codigoExiste = await _context.Projetos.AnyAsync(p => p.Codigo == dto.Codigo);
            if (codigoExiste)
                return BadRequest("Código do projeto já existe.");

            var projeto = new Projeto
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                NomeCliente = dto.NomeCliente,
                TelefoneCliente = dto.TelefoneCliente,
                EmailCliente = dto.EmailCliente,
                EnderecoEntrega = dto.EnderecoEntrega,
                ValorMaoObra = dto.ValorMaoObra,
                ValorFrete = dto.ValorFrete,
                MargemLucro = dto.MargemLucro,
                DataCriacao = DateTime.UtcNow,
                Status = StatusProjeto.Rascunho
            };

            // Mapeamento MANUAL dos itens
            var itens = dto.Itens.Select(item => new ItemProjeto
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                MetrosLineares = item.MetrosLineares
            }).ToList();

            var projetoCriado = await _projetoService.CriarProjeto(projeto, itens);

            // MAPEAMENTO MANUAL para o DTO de retorno
            var projetoDto = new ProjetoDTO
            {
                Id = projetoCriado.Id,
                Codigo = projetoCriado.Codigo,
                Nome = projetoCriado.Nome,
                Descricao = projetoCriado.Descricao,
                NomeCliente = projetoCriado.NomeCliente,
                TelefoneCliente = projetoCriado.TelefoneCliente,
                EmailCliente = projetoCriado.EmailCliente,
                EnderecoEntrega = projetoCriado.EnderecoEntrega,
                DataCriacao = projetoCriado.DataCriacao,
                DataAssinatura = projetoCriado.DataAssinatura,
                DataPagamento = projetoCriado.DataPagamento,
                DataEntrega = projetoCriado.DataEntrega,
                ValorMateriais = projetoCriado.ValorMateriais,
                ValorMaoObra = projetoCriado.ValorMaoObra,
                ValorFrete = projetoCriado.ValorFrete,
                MargemLucro = projetoCriado.MargemLucro,
                ValorTotal = projetoCriado.ValorTotal,
                Status = projetoCriado.Status,
                Itens = new List<ItemProjetoDTO>() // Inicializar lista vazia
            };

            return CreatedAtAction(nameof(GetProjeto), new { id = projetoCriado.Id }, projetoDto);
        }

        // GET: api/projeto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetoDTO>> GetProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            Console.WriteLine($"Itens no banco: {projeto.Itens.Count}");
            foreach (var item in projeto.Itens)
            {
                Console.WriteLine($"Item: {item.Id}, Produto: {item.ProdutoId}, Qtd: {item.Quantidade}");
            }

            return Ok(projeto.Adapt<ProjetoDTO>());
        }

        // POST: api/projeto/1/reservar-materiais
        [HttpPost("{id}/reservar-materiais")]
        public async Task<IActionResult> ReservarMateriais(int id)
        {
            var sucesso = await _projetoService.ReservarMateriais(id);

            if (!sucesso)
                return BadRequest("Não foi possível reservar os materiais.");

            return Ok();
        }

        // PUT: api/projeto/1/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusProjeto status)
        {
            var sucesso = await _projetoService.AtualizarStatusProjeto(id, status);

            if (!sucesso)
                return BadRequest("Não foi possível atualizar o status.");

            return Ok();
        }

        // GET: api/projeto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoDTO>>> GetProjetos()
        {
            var projetos = await _context.Projetos
                .Include(p => p.Itens)
                .ToListAsync();

            return Ok(projetos.Adapt<List<ProjetoDTO>>());
        }
    }
}