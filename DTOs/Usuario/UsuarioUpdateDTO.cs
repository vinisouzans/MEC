﻿using System.ComponentModel.DataAnnotations;

namespace MEC.DTOs.Usuario
{
    public class UsuarioUpdateDTO
    {
        [Required]
        public string Nome { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string PrimeiroNome { get; set; } = null!;
        [Required]
        public string Sobrenome { get; set; } = null!;
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public string CPF { get; set; } = null!;
        public string? RG { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        public string? Senha { get; set; }

        public bool Ativo { get; set; }
        public string? Sexo { get; set; }
        public string? Rua { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? CEP { get; set; }
    }
}
