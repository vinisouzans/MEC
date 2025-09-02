using MEC.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace MEC.Services.Interfaces
{
    public interface IEstoqueService
    {
        Task<bool> RegistrarMovimento(MovimentoEstoque movimento, IDbContextTransaction? externalTransaction = null);
        Task<bool> EntradaEstoque(int produtoId, int quantidade, string? observacao = null, IDbContextTransaction? externalTransaction = null);
        Task<bool> SaidaEstoque(int produtoId, int quantidade, string? observacao = null, IDbContextTransaction? externalTransaction = null);
        Task<bool> CortarMaterialLinear(int materialId, decimal metrosCortados, string responsavel, string? descricao = null);
        Task<int> ConsultarSaldo(int produtoId);
    }
}