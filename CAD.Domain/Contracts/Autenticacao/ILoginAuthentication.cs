using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Autenticacao
{
    public interface ILoginAuthentication
    {
        Task<ResultModel> LoginAsync(LoginModel login);
    }
}
