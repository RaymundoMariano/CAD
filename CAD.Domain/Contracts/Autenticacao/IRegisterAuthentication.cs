using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Autenticacao
{
    public interface IRegisterAuthentication
    {
        Task<ResultModel> RegisterAsync(Models.Autenticacao.RegisterModel register);
    }
}
