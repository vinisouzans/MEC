using Mapster;
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
        }
    }
}
