﻿namespace MEC.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public string Nome { get; set; }
        public string Role { get; set; }
    }
}
