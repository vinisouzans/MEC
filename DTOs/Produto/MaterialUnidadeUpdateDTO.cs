namespace MEC.DTOs.Produto
{
    public class MaterialUnidadeUpdateDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantidadeMinima { get; set; }
        public string Localizacao { get; set; }
        public int? FornecedorId { get; set; }
        public string? Marca { get; set; }
        public string Modelo { get; set; }
        public string UnidadeMedida { get; set; } = "un";
        public decimal? VolumeUnitario { get; set; }
        public string? TipoEmbalagem { get; set; }
        public bool Ativo { get; set; }
    }
}
