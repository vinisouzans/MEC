using MEC.Models;

namespace MEC.Services.Interfaces
{
    public interface IProjetoService
    {
        Task<Projeto> CriarProjeto(Projeto projeto, List<ItemProjeto> itens);
        Task<bool> ReservarMateriais(int projetoId);
        Task<bool> LiberarMateriais(int projetoId);
        Task<bool> AtualizarStatusProjeto(int projetoId, StatusProjeto novoStatus);
        Task<bool> AtualizarStatusItensProjeto(int projetoId);
        Task<decimal> CalcularValorMateriais(List<ItemProjeto> itens);
    }
}
