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

    public virtual int Cadastrar(TEntity entity)
    {
        if (entity.Id == 0)
        {
            _dbSet.Add(entity);
        }
        else
        {
            var trackedEntity = _dbSet.Find(entity.Id);
            if (trackedEntity == null)
            {
                _dbSet.Update(entity);
            }
            else
            {
                _db.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }
        }
        return _db.SaveChanges();
    }

    public virtual int Excluir(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            return 0;
        }

        _dbSet.Remove(entity);
        return _db.SaveChanges();
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
