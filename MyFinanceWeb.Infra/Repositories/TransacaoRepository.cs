using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;

namespace MyFinanceWeb.Infra.Repositories
{
    public class TransacaoRepository(MyFinanceDbContext dbContext) : Repository<Transacao>(dbContext), ITransacaoRepository
    {
        public override List<Transacao> ListarRegistros()
        {
            return _db.Set<Transacao>()
                .Include(transacao => transacao.PlanoConta)
                .ToList();
        }

        public override Transacao? RetornarRegistro(int id)
        {
            return _db.Set<Transacao>()
                .Include(transacao => transacao.PlanoConta)
                .FirstOrDefault(transacao => transacao.Id == id);
        }
    }
}