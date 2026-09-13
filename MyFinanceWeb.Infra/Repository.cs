using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Domain.Entities.Base;
using MyFinanceWeb.Infra.Interfaces.Base;

namespace MyFinanceWeb.Infra;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase

{
    protected DbContext _db;
    protected DbSet<TEntity> _dbSet;

    protected Repository(DbContext dbContext)
    {
        _db = dbContext;
        _dbSet = _db.Set<TEntity>();
    }

    public void Cadastrar(TEntity entity)
    {
        if (entity.Id == 0)
        {
            _dbSet.Add(entity);
        }
        else
        {
            _dbSet.Update(entity);
        }
        _db.SaveChanges();
    }

    public virtual bool Excluir(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        _db.SaveChanges();
        return true;
    }

    public virtual List<TEntity> ListarRegistros()
    {
        return _dbSet.ToList();
    }

    public virtual TEntity? RetornarRegistro(int id)
    {
        return _dbSet.Find(id) ?? null;
    }
}
