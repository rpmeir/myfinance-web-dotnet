namespace MyFinanceWeb.Tests.Unit;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Repositories;
using MyFinanceWeb.Tests.Database;

[TestClass]
public sealed class PlanoContaRepositoryTests
{
    [TestMethod]
    public void Cadastrar_DevePersistirERetornarPlanoConta()
    {
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var planoConta = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };

        repository.Cadastrar(planoConta);

        var planoContaPersistido = repository.RetornarRegistro(planoConta.Id);

        Assert.IsNotNull(planoContaPersistido);
        Assert.AreEqual("Salario", planoContaPersistido.Descricao);
        Assert.AreEqual('R', planoContaPersistido.Tipo);
    }
}
