namespace MyFinanceWeb.Tests.Integration;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Repositories;
using MyFinanceWeb.Mvc.Controllers;
using MyFinanceWeb.Mvc.Models;
using MyFinanceWeb.Service;
using MyFinanceWeb.Tests.Database;

[TestClass]
public class PlanoContaControllerTests
{
    [TestMethod]
    public void DeveListarDoisPlanosDeConta()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var salario = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        var aluguel = new PlanoConta
        {
            Descricao = "Aluguel",
            Tipo = 'D'
        };
        repository.Cadastrar(salario);
        repository.Cadastrar(aluguel);

        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsNull(viewResult.ViewName);

        var model = viewResult.ViewData["PlanoContaList"] as List<PlanoContaModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(2, model.Count);
        Assert.AreNotEqual(0, model[0].Id);
        Assert.AreEqual("Salario", model[0].Descricao);
        Assert.AreEqual('R', model[0].Tipo);
        Assert.AreNotEqual(0, model[1].Id);
        Assert.AreEqual("Aluguel", model[1].Descricao);
        Assert.AreEqual('D', model[1].Tipo);
    }
}