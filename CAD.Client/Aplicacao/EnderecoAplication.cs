using CAD.Domain.Contracts.Aplicacao;
using CAD.Domain.Models.Aplicacao;

namespace CAD.Client.Aplicacao
{
    public class EnderecoAplication : Aplication<EnderecoModel>, IEnderecoAplication
    {
        public EnderecoAplication() : base("https://localhost:44343/api/enderecos") { }
    }
}
