using MEC.Models;

namespace MEC.DTOs.MovimentoEstoque
{
    public class MovimentoEstoqueDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public TipoMovimento Tipo { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
        public DateTime DataMovimento { get; set; }
        public string? UsuarioResponsavel { get; set; }
        public decimal? MetrosLineares { get; set; }

        // Informações do produto
        public string? ProdutoNome { get; set; }
        public string? ProdutoCodigo { get; set; }
    }
}
