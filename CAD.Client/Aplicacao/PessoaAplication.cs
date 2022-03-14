using CAD.Domain.Contracts.Aplicacao;
using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Aplicacao
{
    public class PessoaAplication : Aplication<PessoaModel>, IPessoaAplication
    {
        public PessoaAplication() : base("https://localhost:44343/api/pessoas") { }

        public async Task<ResultModel> ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?pessoaId=" + pessoaId, token);
            return base.Deserialize(await base.Client.PostAsJsonAsync("", enderecoModel));
        }
    }
}
