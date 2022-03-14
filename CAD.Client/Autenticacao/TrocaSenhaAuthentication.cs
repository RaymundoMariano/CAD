using CAD.Domain.Contracts.Autenticacao;
using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Autenticacao
{
    public class TrocaSenhaAuthentication : ClientBase, ITrocaSenhaAuthentication
    {
        public TrocaSenhaAuthentication() : base("https://localhost:44305/api/usuarios/trocasenha") { }

        public async Task<ResultModel> TrocaSenhaAsync(TrocaSenhaModel trocaSenha)
        {
            base.NovaRota("", null);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", trocaSenha));         
        }
    }
} 
