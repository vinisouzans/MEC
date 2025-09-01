using Mapster;
using MEC.Data;
using MEC.Services.Interfaces;
using MEC.DTOs.MovimentoEstoque;
using MEC.Models;
using MEC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(AppDbContext context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        // POST: api/estoque/movimento
        [HttpPost("movimento")]
        public async Task<IActionResult> RegistrarMovimento([FromBody] MovimentoEstoqueCreateDTO dto)
        {
            var movimento = dto.Adapt<MovimentoEstoque>();
            movimento.DataMovimento = DateTime.UtcNow;
            movimento.UsuarioResponsavel = User.Identity?.Name;

            var sucesso = await _estoqueService.RegistrarMovimento(movimento);

            if (!sucesso)
                return BadRequest("Não foi possível registrar o movimento.");

            return Ok();
        }

        // POST: api/estoque/entrada
        [HttpPost("entrada")]
        public async Task<IActionResult> EntradaEstoque([FromBody] MovimentoEstoqueCreateDTO dto)
        {
            var sucesso = await _estoqueService.EntradaEstoque(dto.ProdutoId, dto.Quantidade, dto.Observacao);

            if (!sucesso)
                return BadRequest("Não foi possível registrar a entrada.");

            return Ok();
        }

        // POST: api/estoque/saida
        [HttpPost("saida")]
        public async Task<IActionResult> SaidaEstoque([FromBody] MovimentoEstoqueCreateDTO dto)
        {
            var sucesso = await _estoqueService.SaidaEstoque(dto.ProdutoId, dto.Quantidade, dto.Observacao);

            if (!sucesso)
                return BadRequest("Estoque insuficiente ou produto não encontrado.");

            return Ok();
        }

        // POST: api/estoque/cortar-material/{materialId}
        [HttpPost("cortar-material/{materialId}")]
        public async Task<IActionResult> CortarMaterial(int materialId, [FromBody] CorteRequestDTO request)
        {
            var sucesso = await _estoqueService.CortarMaterialLinear(
                materialId, request.MetrosCortados, request.Responsavel, request.Descricao);

            if (!sucesso)
                return BadRequest("Não foi possível realizar o corte.");

            return Ok();
        }

        // GET: api/estoque/saldo/{produtoId}
        [HttpGet("saldo/{produtoId}")]
        public async Task<ActionResult<int>> ConsultarSaldo(int produtoId)
        {
            var saldo = await _estoqueService.ConsultarSaldo(produtoId);
            return Ok(saldo);
        }

        // GET: api/estoque/historico/{produtoId}
        [HttpGet("historico/{produtoId}")]
        public async Task<ActionResult<IEnumerable<MovimentoEstoqueDTO>>> HistoricoMovimentos(int produtoId)
        {
            var movimentos = await _context.MovimentosEstoque
                .Where(m => m.ProdutoId == produtoId)
                .Include(m => m.Produto)
                .OrderByDescending(m => m.DataMovimento)
                .ToListAsync();

            return Ok(movimentos.Adapt<List<MovimentoEstoqueDTO>>());
        }
    }

    public class CorteRequestDTO
    {
        public decimal MetrosCortados { get; set; }
        public string Responsavel { get; set; }
        public string? Descricao { get; set; }
    }
}