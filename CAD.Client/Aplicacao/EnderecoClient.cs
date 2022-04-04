using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Models.Aplicacao;

namespace CAD.Client.Aplicacao
{
    public class EnderecoClient : Client<EnderecoModel>, IEnderecoClient
    {
        public EnderecoClient() : base("https://localhost:44343/api/enderecos") { }
    }
}
