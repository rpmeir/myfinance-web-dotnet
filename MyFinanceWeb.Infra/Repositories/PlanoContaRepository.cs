using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;

namespace MyFinanceWeb.Infra.Repositories
{
    public class PlanoContaRepository(MyFinanceDbContext dbContext) : Repository<PlanoConta>(dbContext), IPlanoContaRepository
    {
        public override bool Excluir(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
            {
                return false;
            }

            if (_db.Set<Transacao>().Any(item => item.PlanoContaId == id))
            {
                return false;
            }

            _dbSet.Remove(entity);
            _db.SaveChanges();
            return true;
        }
    }
}