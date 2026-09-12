using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Service.Interfaces;

public interface ITransacaoService
{
    void Cadastrar(Transacao transacao);
    void Excluir(int id);
    List<Transacao> ListarRegistros();
    Transacao? RetornarRegistro(int id);
}