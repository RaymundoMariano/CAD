using CAD.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients.Aplicacao
{
    public interface IEmpresaClient : IClient<EmpresaModel>
    {
        Task ManterEnderecoAsync(int empresaId, EnderecoModel enderecoModel, string token);
        Task<List<FilialModel>> ObterFiliaisAsync(int empresaId, string token);
        Task ManterFiliaisAsync(int empresaId, List<FilialModel> empresasModel, string token);
        Task<List<PessoaModel>> ObterSociosAsync(int empresaId, string token);
        Task ManterSociosAsync(int empresaId, List<PessoaModel> pessoasModel, string token);
    }
}
