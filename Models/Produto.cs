namespace MEC.Models
{
    public abstract class Produto
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
        public DateTime DataCadastro { get; set; }
        public DateTime? DataUltimaEntrada { get; set; }
        public DateTime? DataUltimaSaida { get; set; }
        public bool Ativo { get; set; }

        public TipoProduto Tipo { get; set; }

        // Relacionamento com Fornecedor
        public int? FornecedorId { get; set; }
        public virtual Fornecedor? Fornecedor { get; set; }

        // Marca/Fabricante (ex: Duratex)
        public string? Marca { get; set; }

        // Histórico de movimentações
        public virtual ICollection<MovimentoEstoque> Movimentos { get; set; } = new List<MovimentoEstoque>();
    }

    public enum TipoProduto
    {
        ChapaMDF,
        MaterialLinear,
        MaterialUnidade
    }
}