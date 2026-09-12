using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;

namespace MyFinanceWeb.Infra.Repositories
{
    public class PlanoContaRepository(MyFinanceDbContext dbContext) : Repository<PlanoConta>(dbContext), IPlanoContaRepository
    {
    }
}