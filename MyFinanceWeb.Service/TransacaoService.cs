namespace MyFinanceWeb.Service;

using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra;
using MyFinanceWeb.Service.Interfaces;

public class TransacaoService : ITransacaoService
{
    private readonly MyFinanceDbContext _dbContext;

    public TransacaoService(MyFinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Cadastrar(Transacao transacao)
    {
        if (transacao.Id == 0)
        {
            _dbContext.Transacoes.Add(transacao);
        }
        else
        {
            _dbContext.Transacoes.Update(transacao);
        }
        _dbContext.SaveChanges();
    }

    public void Excluir(int id)
    {
        var transacao = _dbContext.Transacoes.Find(id);
        if (transacao != null)
        {
            _dbContext.Transacoes.Remove(transacao);
            _dbContext.SaveChanges();
        }
    }

    public List<Transacao> ListarRegistros()
    {
        return _dbContext.Transacoes.Include(t => t.PlanoConta).ToList();
    }

    public Transacao? RetornarRegistro(int id)
    {
        return _dbContext.Transacoes.Include(t => t.PlanoConta).FirstOrDefault(t => t.Id == id);
    }
}