using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients
{
    public interface IEmpresaClient : IClient<EmpresaModel>
    {
        Task<ResultResponse> ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token);
        Task<ResultResponse> ObterFiliaisAsync(int empresaId, string token);
        Task<ResultResponse> ManterFiliaisAsync(int empresaId, List<EmpresaModel> empresasModel, string token);
        Task<ResultResponse> ObterSociosAsync(int empresaId, string token);
        Task<ResultResponse> ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token);
    }
}
