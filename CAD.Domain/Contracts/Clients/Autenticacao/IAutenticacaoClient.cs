using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients.Autenticacao
{
    public interface IAutenticacaoClient
    {
        Task<RegistroModel> LoginAsync(LoginModel login);
        Task<RegistroModel> RegisterAsync(RegisterModel register);
        Task<List<string>> TrocaSenhaAsync(TrocaSenhaModel trocaSenha);
    }
}
