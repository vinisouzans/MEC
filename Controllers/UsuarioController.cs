﻿using AutoMapper;
using MEC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using MEC.Data;
using MEC.DTOs.Usuario;
using Microsoft.EntityFrameworkCore;

namespace MEC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioReadDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UsuarioReadDTO>>(usuarios));
        }

        // GET api/usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioReadDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.ToListAsync();

            if (usuario == null)
                return NotFound();

            return Ok(_mapper.Map<UsuarioReadDTO>(usuario));
        }

        // POST api/usuario
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<UsuarioReadDTO>> CreateUsuario(UsuarioCreateDTO dto)
        {            

            // Verifica duplicidade de UserName
            var userNameExiste = await _context.Usuarios.AnyAsync(u => u.UserName == dto.UserName);
            if (userNameExiste)
                return BadRequest($"O nome de usuário '{dto.UserName}' já está em uso.");

            // Verifica duplicidade de CPF
            var cpfExiste = await _context.Usuarios.AnyAsync(u => u.CPF == dto.CPF);
            if (cpfExiste)
                return BadRequest($"O CPF '{dto.CPF}' já está cadastrado.");

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            usuario.DataCriacao = DateTime.UtcNow;
            usuario.Ativo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioRead = _mapper.Map<UsuarioReadDTO>(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuarioRead.Id }, usuarioRead);
        }


        // PUT api/usuario/5
        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            // Atualiza campos do DTO
            _mapper.Map(dto, usuario);

            // Atualiza senha se informada
            if (!string.IsNullOrWhiteSpace(dto.Senha))
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            usuario.DataAlteracao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Dados do usuário atualizados com sucesso." });
        }

        // DELETE api/usuario/5
        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
