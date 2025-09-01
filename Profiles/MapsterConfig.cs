using Mapster;
using MEC.DTOs.Fornecedor;
using MEC.DTOs.Produto;
using MEC.DTOs.Usuario;
using MEC.Models;

namespace MEC.Profiles
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // Configurar mapeamentos personalizados se necessário
            TypeAdapterConfig<Usuario, UsuarioReadDTO>
                .NewConfig();
            // .Map(dest => dest.PropriedadeCustom, src => src.OutraPropriedade);

            TypeAdapterConfig<UsuarioCreateDTO, Usuario>
                .NewConfig();

            TypeAdapterConfig<UsuarioUpdateDTO, Usuario>
                .NewConfig();

            // Configuração para CorteMaterial -> CorteMaterialDTO
            TypeAdapterConfig<CorteMaterial, CorteMaterialDTO>
                .NewConfig()
                .Map(dest => dest.MaterialNome, src => src.MaterialLinear != null ? src.MaterialLinear.Nome : "")
                .Map(dest => dest.MaterialCodigo, src => src.MaterialLinear != null ? src.MaterialLinear.Codigo : "");

            TypeAdapterConfig<Fornecedor, FornecedorDTO>.NewConfig();
            TypeAdapterConfig<FornecedorCreateDTO, Fornecedor>.NewConfig();
            TypeAdapterConfig<FornecedorUpdateDTO, Fornecedor>.NewConfig();

            TypeAdapterConfig<ChapaMDFCreateDTO, ChapaMDF>.NewConfig();
            TypeAdapterConfig<ChapaMDFUpdateDTO, ChapaMDF>.NewConfig();

            TypeAdapterConfig<MaterialLinearCreateDTO, MaterialLinear>.NewConfig();
            TypeAdapterConfig<MaterialLinearUpdateDTO, MaterialLinear>.NewConfig();

            TypeAdapterConfig<MaterialUnidadeCreateDTO, MaterialUnidade>.NewConfig();
            TypeAdapterConfig<MaterialUnidadeUpdateDTO, MaterialUnidade>.NewConfig();

            TypeAdapterConfig<MaterialUnidade, MaterialUnidadeDTO>.NewConfig();
        }
    }
}
