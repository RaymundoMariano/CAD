using CAD.Domain.Contracts.Clients;
using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client
{
    public class EmpresaClient : Client<EmpresaModel>, IEmpresaClient
    {
        public EmpresaClient() : base("https://localhost:44383/api/empresas") { }

        #region Endereco
        public async Task<ResultResponse> ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", enderecoModel));
        }
        #endregion

        #region Filiais
        public async Task<ResultResponse> ObterFiliaisAsync(int empresaId, string token)
        {
            base.NovaRota("/GetFiliais?empresaId=" + empresaId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }

        public async Task<ResultResponse> ManterFiliaisAsync(int empresaId, List<EmpresaModel> empresasModel, string token)
        {
            base.NovaRota("/PostFiliais?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", empresasModel));
        }
        #endregion

        #region Socios
        public async Task<ResultResponse> ObterSociosAsync(int empresaId, string token)
        {
            base.NovaRota("/GetSocios?empresaId=" + empresaId, token);
            return await base.Client.GetFromJsonAsync<ResultResponse>("");
        }

        public async Task<ResultResponse> ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token)
        {
            base.NovaRota("/PostSocios?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", pessoasModel));
        }
        #endregion

        #region Deserialize
        private ResultResponse Deserialize(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }
        #endregion
    }
}