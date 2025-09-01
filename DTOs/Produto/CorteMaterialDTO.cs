namespace MEC.DTOs.Produto
{
    public class CorteMaterialDTO
    {
        public int Id { get; set; }
        public int MaterialLinearId { get; set; }
        public decimal ComprimentoCortado { get; set; } // em metros
        public DateTime DataCorte { get; set; }
        public string Responsavel { get; set; }
        public string Descricao { get; set; }

        // Opcional: informações do material relacionado
        public string MaterialNome { get; set; }
        public string MaterialCodigo { get; set; }
    }
}
