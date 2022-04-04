using CAD.Domain.Models.Response;
using CAD.Domain.Models.Seguranca;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients.Seguranca
{
    public interface ISegurancaClient
    {
        Task<SegurancaModel> ObterPerfilAsync(string modulo, RegistroModel registro);
    }
}
