using Mapster;
using MEC.DTOs.Fornecedor;
using MEC.DTOs.ItemProjeto;
using MEC.DTOs.Produto;
using MEC.DTOs.Projeto;
using MEC.DTOs.Usuario;
using MEC.Models;

namespace MEC.Profiles
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // Configurações existentes...

            // NOVAS CONFIGURAÇÕES PARA PROJETO
            TypeAdapterConfig<Projeto, ProjetoDTO>.NewConfig();
            TypeAdapterConfig<ProjetoCreateDTO, Projeto>.NewConfig();

            // CONFIGURAÇÕES PARA ITEM PROJETO
            TypeAdapterConfig<ItemProjeto, ItemProjetoDTO>.NewConfig()
                .Map(dest => dest.ProdutoNome, src => src.Produto != null ? src.Produto.Nome : "")
                .Map(dest => dest.ProdutoCodigo, src => src.Produto != null ? src.Produto.Codigo : "")
                .Map(dest => dest.ProdutoTipo, src => src.Produto != null ? src.Produto.Tipo : 0)
                .Map(dest => dest.ValorUnitario, src => src.Produto != null ? src.Produto.PrecoVenda : 0)
                .Map(dest => dest.ValorTotal, src =>
                    src.Produto != null ?
                    (src.MetrosLineares.HasValue ?
                        src.Produto.PrecoVenda * (decimal)src.MetrosLineares.Value :
                        src.Produto.PrecoVenda * src.Quantidade)
                    : 0);

            TypeAdapterConfig<ItemProjetoCreateDTO, ItemProjeto>.NewConfig();
        }
    }
}