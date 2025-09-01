using Mapster;
using MEC.Data;
using MEC.DTOs;
using MEC.DTOs.Fornecedor;
using MEC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/fornecedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDTO>>> GetFornecedores()
        {
            var fornecedores = await _context.Fornecedores.ToListAsync();
            return Ok(fornecedores.Adapt<List<FornecedorDTO>>());
        }

        // GET: api/fornecedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDTO>> GetFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
                return NotFound();

            return Ok(fornecedor.Adapt<FornecedorDTO>());
        }

        // POST: api/fornecedor
        [HttpPost]
        public async Task<ActionResult<FornecedorDTO>> CreateFornecedor(FornecedorCreateDTO dto)
        {
            // Verificar se CNPJ já existe
            var cnpjExiste = await _context.Fornecedores.AnyAsync(f => f.CNPJ == dto.CNPJ);
            if (cnpjExiste)
                return BadRequest("CNPJ já cadastrado.");

            var fornecedor = dto.Adapt<Fornecedor>();
            fornecedor.DataCadastro = DateTime.UtcNow;

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            var fornecedorDto = fornecedor.Adapt<FornecedorDTO>();
            return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.Id }, fornecedorDto);
        }

        // PUT: api/fornecedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFornecedor(int id, FornecedorUpdateDTO dto)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
                return NotFound();

            dto.Adapt(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/fornecedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
                return NotFound();

            // Verificar se há produtos associados
            var temProdutos = await _context.Produtos.AnyAsync(p => p.FornecedorId == id);
            if (temProdutos)
                return BadRequest("Não é possível excluir o fornecedor pois existem produtos associados.");

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/fornecedor/5/produtos
        [HttpGet("{id}/produtos")]
        public async Task<ActionResult<IEnumerable<object>>> GetProdutosFornecedor(int id)
        {
            var produtos = await _context.Produtos
                .Where(p => p.FornecedorId == id)
                .Select(p => new { p.Id, p.Nome, p.Codigo, p.Tipo })
                .ToListAsync();

            return Ok(produtos);
        }
    }
}