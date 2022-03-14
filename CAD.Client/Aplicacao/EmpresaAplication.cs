using CAD.Domain.Contracts.Clients;
using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Aplicacao
{
    public class EmpresaAplication : Aplication<EmpresaModel>, IEmpresaAplication
    {
        public EmpresaAplication() : base("https://localhost:44343/api/empresas") { }

        #region Endereco
        public async Task<ResultModel> ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token)
        {
            base.NovaRota("/PostEndereco?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", enderecoModel));
        }
        #endregion

        #region Filiais
        public async Task<ResultModel> ObterFiliaisAsync(int empresaId, string token)
        {
            base.NovaRota("/GetFiliais?empresaId=" + empresaId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }

        public async Task<ResultModel> ManterFiliaisAsync(int empresaId, List<EmpresaModel> empresasModel, string token)
        {
            base.NovaRota("/PostFiliais?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", empresasModel));
        }
        #endregion

        #region Socios
        public async Task<ResultModel> ObterSociosAsync(int empresaId, string token)
        {
            base.NovaRota("/GetSocios?empresaId=" + empresaId, token);
            return await base.Client.GetFromJsonAsync<ResultModel>("");
        }

        public async Task<ResultModel> ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token)
        {
            base.NovaRota("/PostSocios?empresaId=" + empresaId, token);
            return Deserialize(await base.Client.PostAsJsonAsync("", pessoasModel));
        }
        #endregion        
    }
}