using CAD.Domain.Contracts.Clients;
using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client
{
    public class PessoaClient : Client<PessoaModel>, IPessoaClient
    {
        public PessoaClient() : base("https://localhost:44383/api/pessoas") { }

        public async Task<ResultResponse> ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?pessoaId=" + pessoaId, token);
            var httpResponse = await base.Client.PostAsJsonAsync("", enderecoModel);

            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }

    }
}
