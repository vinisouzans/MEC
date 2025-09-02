namespace MEC.Models
{
    public class ItemProjeto
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        // Para materiais lineares
        public decimal? MetrosLineares { get; set; }

        // Status do item
        public StatusItemProjeto Status { get; set; }
        public string? Observacao { get; set; }

        // Relacionamentos
        public virtual Projeto Projeto { get; set; }
        public virtual Produto Produto { get; set; }
    }

    public enum StatusItemProjeto
    {
        EmEstoque,
        NecessitaCompra,
        Reservado,
        Utilizado
    }
}