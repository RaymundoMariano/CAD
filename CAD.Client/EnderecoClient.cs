using CAD.Client;
using CAD.Domain.Contracts.Clients;
using Cadastro.Domain.Models;

namespace CAD.Client
{
    public class EnderecoClient : Client<EnderecoModel>, IEnderecoClient
    {
        public EnderecoClient() : base("https://localhost:44383/api/enderecos") { }
    }
}
