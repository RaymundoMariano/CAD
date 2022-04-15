using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Contracts.Clients.Autenticacao;
using CAD.Domain.Contracts.Clients.Seguranca;

namespace CAD.Domain.Contracts.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IEmpresaClient Empresas { get; }
        IPessoaClient Pessoas { get; }
        ISegurancaClient Seguranca { get; }
        IAutenticacaoClient Autenticacao { get; }
    }
}
