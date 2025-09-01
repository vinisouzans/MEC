namespace MEC.Models
{
    public class MaterialLinear : Produto
    {
        public decimal ComprimentoTotal { get; set; } // em metros
        public decimal ComprimentoDisponivel { get; set; } // em metros
        public decimal ComprimentoCortado { get; set; } // em metros

        // Histórico de cortes
        public List<CorteMaterial> Cortes { get; set; } = new List<CorteMaterial>();
    }

    public class CorteMaterial
    {
        public int Id { get; set; }
        public int MaterialLinearId { get; set; }
        public decimal ComprimentoCortado { get; set; } // em metros
        public DateTime DataCorte { get; set; }
        public string Responsavel { get; set; }
        public string Descricao { get; set; }

        public MaterialLinear MaterialLinear { get; set; }
    }
}
