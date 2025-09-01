using MEC.Models;

namespace MEC.DTOs.Produto
{
    public class ChapaMDFDTO : ProdutoDTO
    {
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Espessura { get; set; }
        public TipoAcabamento Acabamento { get; set; }
        public decimal AreaTotal { get; set; }
        public decimal Volume { get; set; }
    }
}
