using MEC.DTOs.ItemProjeto;
namespace MEC.DTOs.Projeto
{
    public class ProjetoCreateDTO
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NomeCliente { get; set; }
        public string? TelefoneCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? EnderecoEntrega { get; set; }
        public decimal ValorMaoObra { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal MargemLucro { get; set; }
        public List<ItemProjetoCreateDTO> Itens { get; set; } = new List<ItemProjetoCreateDTO>();
    }
}
