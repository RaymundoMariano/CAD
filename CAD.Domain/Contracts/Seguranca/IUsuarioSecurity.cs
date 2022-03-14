using CAD.Domain.Models.Response;
using CAD.Domain.Models.Seguranca;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Seguranca
{
    public interface IUsuarioSecurity
    {
        Task<ResultModel> ObterPerfilAsync(string modulo, UsuarioModel usuario, string token);
    }
}
