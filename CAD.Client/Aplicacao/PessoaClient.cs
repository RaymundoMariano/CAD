using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Models.Aplicacao;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Aplicacao
{
    public class PessoaClient : Client<PessoaModel>, IPessoaClient
    {
        public PessoaClient() : base("https://localhost:44343/api/pessoas") { }

        public async Task ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?pessoaId=" + pessoaId, token);
            base.Response(await base.Client.PostAsJsonAsync("", enderecoModel));
        }
    }
}
