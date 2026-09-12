namespace MyFinanceWeb.Service;

using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;
using MyFinanceWeb.Service.Interfaces;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;

    public TransacaoService(ITransacaoRepository transacaoRepository)
    {
        _transacaoRepository = transacaoRepository;
    }

    public void Cadastrar(Transacao entity)
    {
        _transacaoRepository.Cadastrar(entity);
    }

    public bool Excluir(int id)
    {
        return _transacaoRepository.Excluir(id);
    }

    public List<Transacao> ListarRegistros()
    {
        return _transacaoRepository.ListarRegistros();
    }

    public Transacao? RetornarRegistro(int id)
    {
        return _transacaoRepository.RetornarRegistro(id);
    }
}