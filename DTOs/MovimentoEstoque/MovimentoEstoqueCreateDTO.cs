using MEC.Models;

namespace MEC.DTOs.MovimentoEstoque
{
    public class MovimentoEstoqueCreateDTO
    {
        public int ProdutoId { get; set; }
        public TipoMovimento Tipo { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
        public decimal? MetrosLineares { get; set; }
    }
}
