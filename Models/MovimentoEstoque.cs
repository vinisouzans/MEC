namespace MEC.Models
{
    public class MovimentoEstoque
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public TipoMovimento Tipo { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
        public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
        public string? UsuarioResponsavel { get; set; }

        // Para materiais lineares (cortes)
        public decimal? MetrosLineares { get; set; }

        // Relacionamentos
        public virtual Produto Produto { get; set; }
    }

    public enum TipoMovimento
    {
        Entrada,    // Compra, doação, etc.
        Saida,      // Venda, uso em produção, etc.
        Ajuste,     // Correção de inventário
        Corte       // Corte de material linear
    }
}
