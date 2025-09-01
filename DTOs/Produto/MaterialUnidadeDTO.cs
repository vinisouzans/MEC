namespace MEC.DTOs.Produto
{
    public class MaterialUnidadeDTO : ProdutoDTO
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string UnidadeMedida { get; set; } = "un";
        public decimal? VolumeUnitario { get; set; }
        public string? TipoEmbalagem { get; set; }
    }
}
