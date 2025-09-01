namespace MEC.DTOs.Fornecedor
{
    public class FornecedorCreateDTO
    {
        public string Nome { get; set; }
        public string? NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? CEP { get; set; }
        public string? Endereco { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Observacoes { get; set; }
    }
}
