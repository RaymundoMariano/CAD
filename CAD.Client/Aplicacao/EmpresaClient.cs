using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Aplicacao
{
    public class EmpresaClient : Client<EmpresaModel>, IEmpresaClient
    {
        public EmpresaClient() : base("https://localhost:44343/api/empresas") { }

        #region Endereco
        public async Task ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?empresaId=" + empresaId, token);
            base.Response(await base.Client.PostAsJsonAsync("", enderecoModel));
        }
        #endregion

        #region Filiais
        public async Task<List<FilialModel>> ObterFiliaisAsync(int empresaId, string token)
        {
            base.NovaRota("/GetFiliais?empresaId=" + empresaId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<FilialModel>>(response.ObjectRetorno.ToString());
        }

        public async Task ManterFiliaisAsync(int empresaId, List<FilialModel> filiaisModel, string token)
        {
            base.NovaRota("/PostFiliais?empresaId=" + empresaId, token);
            base.Response(await base.Client.PostAsJsonAsync("", filiaisModel));
        }
        #endregion

        #region Socios
        public async Task<List<PessoaModel>> ObterSociosAsync(int empresaId, string token)
        {
            base.NovaRota("/GetSocios?empresaId=" + empresaId, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<PessoaModel>>(response.ObjectRetorno.ToString());
        }

        public async Task ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token)
        {
            base.NovaRota("/PostSocios?empresaId=" + empresaId, token);
            base.Response(await base.Client.PostAsJsonAsync("", pessoasModel));
        }
        #endregion        
    }
}