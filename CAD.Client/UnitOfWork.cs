using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Contracts.Clients.Autenticacao;
using CAD.Domain.Contracts.Clients.Seguranca;
using CAD.Domain.Contracts.UnitOfWorks;

namespace CAD.Client
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmpresaClient Empresas { get; }
        public IPessoaClient Pessoas { get; }
        public ISegurancaClient Seguranca { get; }
        public IAutenticacaoClient Autenticacao { get; }
        public UnitOfWork(IEmpresaClient empresaClient
            , IPessoaClient pessoaClient
            , ISegurancaClient segurancaClient
            , IAutenticacaoClient autenticacaoClient)
        {
            Empresas = empresaClient;
            Pessoas = pessoaClient;
            Seguranca = segurancaClient;
            Autenticacao = autenticacaoClient;
        }
    }
}
