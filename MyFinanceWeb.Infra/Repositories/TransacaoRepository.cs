using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;

namespace MyFinanceWeb.Infra.Repositories
{
    public class TransacaoRepository(MyFinanceDbContext dbContext) : Repository<Transacao>(dbContext), ITransacaoRepository
    {
        public override int Cadastrar(Transacao entity)
        {
            if (entity.PlanoContaId == 0)
            {
                return 0;
            }

            return base.Cadastrar(entity);
        }

        public override List<Transacao> ListarRegistros()
        {
            return _dbSet.Include(transacao => transacao.PlanoConta)
                .ToList();
        }

        public override Transacao? RetornarRegistro(int id)
        {
            return _dbSet.Include(transacao => transacao.PlanoConta)
                .FirstOrDefault(transacao => transacao.Id == id);
        }
    }
}