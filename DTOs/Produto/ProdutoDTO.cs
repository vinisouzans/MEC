using MEC.Models;

namespace MEC.DTOs.Produto
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantidadeMinima { get; set; }
        public int QuantidadeAtual { get; set; }
        public string Localizacao { get; set; }
        public TipoProduto Tipo { get; set; }
    }
}
