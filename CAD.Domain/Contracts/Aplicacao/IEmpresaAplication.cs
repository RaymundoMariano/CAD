using CAD.Domain.Contracts.Aplicacao;
using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients
{
    public interface IEmpresaAplication : IAplication<EmpresaModel>
    {
        Task<ResultModel> ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token);
        Task<ResultModel> ObterFiliaisAsync(int empresaId, string token);
        Task<ResultModel> ManterFiliaisAsync(int empresaId, List<EmpresaModel> empresasModel, string token);
        Task<ResultModel> ObterSociosAsync(int empresaId, string token);
        Task<ResultModel> ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token);
    }
}
