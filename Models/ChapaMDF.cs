namespace MEC.Models
{
    public class ChapaMDF : Produto
    {
        public decimal Altura { get; set; } // em metros
        public decimal Largura { get; set; } // em metros
        public decimal Espessura { get; set; } // em milímetros

        // Tipo de acabamento (usando enum)
        public TipoAcabamento Acabamento { get; set; }

        // Propriedades calculadas
        public decimal AreaTotal => Altura * Largura;
        public decimal Volume => Altura * Largura * (Espessura / 1000);
    }

    public enum TipoAcabamento
    {
        Cru,
        LaqueadoUmLado,
        LaqueadoDoisLados
    }
}
