using CAD.Domain.Contracts.Autenticacao;
using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Autenticacao
{
    public class LoginAuthentication : ClientBase, ILoginAuthentication
    {
        public LoginAuthentication() : base("https://localhost:44305/api/usuarios/login") { }

        public async Task<ResultModel> LoginAsync(LoginModel login)
        {
            base.NovaRota("", null);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", login));
        }
    }
} 
