using MEC.Models;

namespace MEC.DTOs.ItemProjeto
{
    public class ItemProjetoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public string ProdutoCodigo { get; set; }
        public TipoProduto ProdutoTipo { get; set; }
        public int Quantidade { get; set; }
        public decimal? MetrosLineares { get; set; }
        public StatusItemProjeto Status { get; set; }
        public string? Observacao { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
