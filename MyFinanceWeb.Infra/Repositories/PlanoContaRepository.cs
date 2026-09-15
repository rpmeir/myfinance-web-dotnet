using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;

namespace MyFinanceWeb.Infra.Repositories
{
    public class PlanoContaRepository(MyFinanceDbContext dbContext) : Repository<PlanoConta>(dbContext), IPlanoContaRepository
    {
        public override int Excluir(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
            {
                return 0;
            }

            if (_db.Set<Transacao>().Any(item => item.PlanoContaId == id))
            {
                return 0;
            }

            _dbSet.Remove(entity);
            return _db.SaveChanges();
        }
    }
}