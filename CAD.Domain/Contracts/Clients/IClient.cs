using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Models;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients
{
    public interface IClient<T> where T : _Model
    {
        Task<ResultResponse> ObterAsync(string token);
        Task<ResultResponse> ObterAsync(int id, string token);
        Task<ResultResponse> InsereAsync(T model, string token);
        Task<ResultResponse> UpdateAsync(int id, T model, string token);
        Task<ResultResponse> RemoveAsync(int id, string token);
    }
}
