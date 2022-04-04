using CAD.Domain.Models.Aplicacao;
using System.Threading.Tasks;

namespace CAD.Domain.Contracts.Clients.Aplicacao
{
    public interface IPessoaClient : IClient<PessoaModel>
    {
        Task ManterEnderecoAsync(int pessoaId, EnderecoModel enderecoModel, string token);
    }
}
