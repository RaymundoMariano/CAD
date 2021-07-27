using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Models;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients
{
    public interface IPessoaClient : IClient<PessoaModel>
    {
        Task<ResultResponse> ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token);
    }
}
