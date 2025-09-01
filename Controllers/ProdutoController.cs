using Mapster;
using MEC.Data;
using MEC.DTOs.Produto;
using MEC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/produto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Fornecedor)
                .Where(p => p.Ativo)
                .ToListAsync();

            return Ok(produtos.Adapt<List<ProdutoDTO>>());
        }

        // GET: api/produto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduto(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            // Retorna DTO específico baseado no tipo
            return produto switch
            {
                ChapaMDF chapa => chapa.Adapt<ChapaMDFDTO>(),
                MaterialLinear material => material.Adapt<MaterialLinearDTO>(),
                MaterialUnidade material => material.Adapt<MaterialUnidadeDTO>(),
                _ => produto.Adapt<ProdutoDTO>()
            };
        }

        // POST: api/produto/chapa-mdf
        [HttpPost("chapa-mdf")]
        public async Task<ActionResult<ChapaMDFDTO>> CreateChapaMDF(ChapaMDFCreateDTO dto)
        {
            // Verificar se código já existe
            var codigoExiste = await _context.Produtos.AnyAsync(p => p.Codigo == dto.Codigo);
            if (codigoExiste)
                return BadRequest("Código do produto já existe.");

            var chapa = dto.Adapt<ChapaMDF>();
            chapa.DataCadastro = DateTime.UtcNow;
            chapa.Ativo = true;
            chapa.QuantidadeAtual = 0; // Inicia com 0

            _context.ChapasMDF.Add(chapa);
            await _context.SaveChangesAsync();

            var chapaDto = chapa.Adapt<ChapaMDFDTO>();
            return CreatedAtAction(nameof(GetProduto), new { id = chapa.Id }, chapaDto);
        }

        // POST: api/produto/material-linear
        [HttpPost("material-linear")]
        public async Task<ActionResult<MaterialLinearDTO>> CreateMaterialLinear(MaterialLinearCreateDTO dto)
        {
            var codigoExiste = await _context.Produtos.AnyAsync(p => p.Codigo == dto.Codigo);
            if (codigoExiste)
                return BadRequest("Código do produto já existe.");

            var material = dto.Adapt<MaterialLinear>();
            material.DataCadastro = DateTime.UtcNow;
            material.Ativo = true;
            material.QuantidadeAtual = 1; // Materiais lineares são por unidade (1 rolo/bobina)
            material.ComprimentoDisponivel = dto.ComprimentoTotal;
            material.ComprimentoCortado = 0;

            _context.MateriaisLineares.Add(material);
            await _context.SaveChangesAsync();

            var materialDto = material.Adapt<MaterialLinearDTO>();
            return CreatedAtAction(nameof(GetProduto), new { id = material.Id }, materialDto);
        }

        // POST: api/produto/material-unidade
        [HttpPost("material-unidade")]
        public async Task<ActionResult<MaterialUnidadeDTO>> CreateMaterialUnidade(MaterialUnidadeCreateDTO dto)
        {
            var codigoExiste = await _context.Produtos.AnyAsync(p => p.Codigo == dto.Codigo);
            if (codigoExiste)
                return BadRequest("Código do produto já existe.");

            var material = dto.Adapt<MaterialUnidade>();
            material.DataCadastro = DateTime.UtcNow;
            material.Ativo = true;
            material.QuantidadeAtual = 0; // Inicia com 0

            _context.MateriaisUnidade.Add(material);
            await _context.SaveChangesAsync();

            var materialDto = material.Adapt<MaterialUnidadeDTO>();
            return CreatedAtAction(nameof(GetProduto), new { id = material.Id }, materialDto);
        }

        // PUT: api/produto/chapa-mdf/5
        [HttpPut("chapa-mdf/{id}")]
        public async Task<IActionResult> UpdateChapaMDF(int id, ChapaMDFUpdateDTO dto)
        {
            var chapa = await _context.ChapasMDF.FindAsync(id);
            if (chapa == null)
                return NotFound();

            dto.Adapt(chapa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/produto/material-linear/5
        [HttpPut("material-linear/{id}")]
        public async Task<IActionResult> UpdateMaterialLinear(int id, MaterialLinearUpdateDTO dto)
        {
            var material = await _context.MateriaisLineares.FindAsync(id);
            if (material == null)
                return NotFound();

            dto.Adapt(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/produto/material-unidade/5
        [HttpPut("material-unidade/{id}")]
        public async Task<IActionResult> UpdateMaterialUnidade(int id, MaterialUnidadeUpdateDTO dto)
        {
            var material = await _context.MateriaisUnidade.FindAsync(id);
            if (material == null)
                return NotFound();

            dto.Adapt(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/produto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                return NotFound();

            // Verificar se há movimentações no estoque
            var temMovimentacoes = await _context.MovimentosEstoque.AnyAsync(m => m.ProdutoId == id);
            if (temMovimentacoes)
                return BadRequest("Não é possível excluir o produto pois existem movimentações no estoque.");

            // Soft delete (desativar)
            produto.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/produto/por-tipo?tipo=ChapaMDF
        [HttpGet("por-tipo")]
        public async Task<ActionResult<IEnumerable<object>>> GetProdutosPorTipo([FromQuery] TipoProduto tipo)
        {
            var produtos = await _context.Produtos
                .Include(p => p.Fornecedor)
                .Where(p => p.Tipo == tipo && p.Ativo)
                .ToListAsync();

            return Ok(produtos.Adapt<List<ProdutoDTO>>());
        }

        // GET: api/produto/baixo-estoque
        [HttpGet("baixo-estoque")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosBaixoEstoque()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Fornecedor)
                .Where(p => p.Ativo && p.QuantidadeAtual <= p.QuantidadeMinima)
                .ToListAsync();

            return Ok(produtos.Adapt<List<ProdutoDTO>>());
        }
    }
}