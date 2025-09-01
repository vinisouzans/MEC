namespace MEC.Models
{
    public class MaterialUnidade : Produto
    {
        // Propriedades específicas para unidades
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string UnidadeMedida { get; set; } = "un";

        // Para colas pode ter volume
        public decimal? VolumeUnitario { get; set; } // em litros
        public string? TipoEmbalagem { get; set; }
    }
}
