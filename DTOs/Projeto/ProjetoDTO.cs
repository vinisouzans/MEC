using MEC.DTOs.ItemProjeto;
using MEC.Models;

namespace MEC.DTOs.Projeto
{
    public class ProjetoDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NomeCliente { get; set; }
        public string? TelefoneCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? EnderecoEntrega { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataEntrega { get; set; }
        public decimal ValorMateriais { get; set; }
        public decimal ValorMaoObra { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusProjeto Status { get; set; }
        public List<ItemProjetoDTO> Itens { get; set; } = new List<ItemProjetoDTO>();
    }
}
