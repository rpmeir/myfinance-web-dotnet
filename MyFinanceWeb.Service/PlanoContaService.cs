namespace MyFinanceWeb.Service;

using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra;
using MyFinanceWeb.Service.Interfaces;

public class PlanoContaService : IPlanoContaService
{
    private readonly MyFinanceDbContext _dbContext;

    public PlanoContaService(MyFinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Cadastrar(PlanoConta planoConta)
    {
        if (planoConta.Id == 0)
        {
            _dbContext.PlanoContas.Add(planoConta);
        }
        else
        {
            _dbContext.PlanoContas.Update(planoConta);
        }
        _dbContext.SaveChanges();
    }

    public bool Excluir(int id)
    {
        var planoConta = _dbContext.PlanoContas.Find(id);
        if (planoConta == null)
        {
            return false;
        }

        if (_dbContext.Transacoes.Any(item => item.PlanoContaId == id))
        {
            return false;
        }

        _dbContext.PlanoContas.Remove(planoConta);
        _dbContext.SaveChanges();
        return true;
    }

    public List<PlanoConta> ListarRegistros()
    {
        return _dbContext.PlanoContas.ToList();
    }

    public PlanoConta? RetornarRegistro(int id)
    {
        return _dbContext.PlanoContas.Find(id) ?? null;
    }
}