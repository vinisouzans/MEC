using MEC.DTOs.Usuario;
using MEC.Models;
using AutoMapper;

namespace MEC.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Usuario -> UsuarioReadDTO
            CreateMap<Usuario, UsuarioReadDTO>();                

            // UsuarioCreateDTO -> Usuario
            CreateMap<UsuarioCreateDTO, Usuario>();

            // UsuarioUpdateDTO -> Usuario
            CreateMap<UsuarioUpdateDTO, Usuario>();
        }
    }
}
