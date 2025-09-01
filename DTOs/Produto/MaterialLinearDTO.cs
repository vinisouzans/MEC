namespace MEC.DTOs.Produto
{
    public class MaterialLinearDTO : ProdutoDTO
    {
        public decimal ComprimentoTotal { get; set; }
        public decimal ComprimentoDisponivel { get; set; }
        public List<CorteMaterialDTO> Cortes { get; set; } = new List<CorteMaterialDTO>();
    }    
}
