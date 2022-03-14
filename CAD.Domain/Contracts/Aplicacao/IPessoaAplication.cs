using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Aplicacao
{
    public interface IPessoaAplication : IAplication<PessoaModel>
    {
        Task<ResultModel> ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token);
    }
}
