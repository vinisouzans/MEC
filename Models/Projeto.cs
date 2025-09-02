namespace MEC.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        // Dados do Cliente
        public string NomeCliente { get; set; }
        public string? TelefoneCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? EnderecoEntrega { get; set; }

        // Datas importantes
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAssinatura { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataEntrega { get; set; }

        // Valores financeiros
        public decimal ValorMateriais { get; set; }
        public decimal ValorMaoObra { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal MargemLucro { get; set; } // em percentual
        public decimal ValorTotal => ValorMateriais + ValorMaoObra + ValorFrete + (ValorMateriais * MargemLucro / 100);

        // Status
        public StatusProjeto Status { get; set; } = StatusProjeto.Rascunho;

        // Relacionamentos
        public virtual ICollection<ItemProjeto> Itens { get; set; } = new List<ItemProjeto>();
    }

    public enum StatusProjeto
    {
        Rascunho,
        AguardandoAprovacao,
        Aprovado,
        EmProducao,
        AguardandoPagamento,
        Pago,
        Entregue,
        Cancelado
    }
}