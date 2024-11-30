using CAD.Domain.Contracts.Clients.Seguranca;
using CAD.Domain.Models.Response;
using CAD.Domain.Models.Seguranca;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Seguranca
{
    public class SegurancaClient : BaseClient, ISegurancaClient
    {
        public SegurancaClient() : base("https://segurancaapi.azurewebsites.net/api/usuarios") { }

        public async Task<SegurModel> ObterPerfilAsync(string modulo, RegistroModel registro)
        {
            var usuario = new UsuarioModel()
            {
                Email = registro.Email,
                Nome = registro.Nome,
            };

            base.NovaRota("/PostUsuario?modulo=" + modulo, registro.Token);
            var response = Response(await base.Client.PostAsJsonAsync("", usuario));

            return JsonConvert.DeserializeObject<SegurModel>(response.ObjectRetorno.ToString());
        }
    }
} 
