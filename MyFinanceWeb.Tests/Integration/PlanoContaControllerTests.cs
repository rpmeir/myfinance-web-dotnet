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
    public void DeveListarTodosOsPlanosDeConta()
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

    [TestMethod]
    public void DeveCarregarFormularioDeCadastroVazio()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Cadastrar(null);

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public void DeveCarregarFormularioDeCadastroComIdInvalido()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var salario = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        repository.Cadastrar(salario);

        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Cadastrar(999);

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public void DeveCarregarFormularioDeCadastroComIdValido()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var salario = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        repository.Cadastrar(salario);

        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Cadastrar(salario.Id);

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsNull(viewResult.ViewName);

        var model = viewResult.Model as PlanoContaModel;
        Assert.IsNotNull(model);
        Assert.AreNotEqual(0, model.Id);
        Assert.AreEqual("Salario", model.Descricao);
        Assert.AreEqual('R', model.Tipo);
    }

    [TestMethod]
    public void DeveCadastrarNovoPlanoDeContaNaBaseDeDadosERedirecionarParaIndex()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);
        var model = new PlanoContaModel
        {
            Descricao = "Salario",
            Tipo = 'R'
        };

        // Act
        var result = controller.Cadastrar(model, null);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);

        var planoConta = repository.ListarRegistros().Single();
        Assert.AreNotEqual(0, planoConta.Id);
        Assert.AreEqual(model.Descricao, planoConta.Descricao);
        Assert.AreEqual(model.Tipo, planoConta.Tipo);
    }

    [TestMethod]
    public void DeveAtualizarPlanoDeContaNaBaseDeDadosQuandoIdsForemIguais()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var planoConta = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        repository.Cadastrar(planoConta);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);
        var model = new PlanoContaModel
        {
            Id = planoConta.Id,
            Descricao = "Salario atualizado",
            Tipo = 'R'
        };

        // Act
        var result = controller.Cadastrar(model, planoConta.Id);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);

        var planoContaAtualizado = repository.RetornarRegistro(planoConta.Id);
        Assert.IsNotNull(planoContaAtualizado);
        Assert.AreEqual(model.Descricao, planoContaAtualizado.Descricao);
        Assert.AreEqual(model.Tipo, planoContaAtualizado.Tipo);
    }

    [TestMethod]
    public void NaoDeveAlterarBaseDeDadosQuandoIdsForemDiferentes()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var planoConta = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        repository.Cadastrar(planoConta);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);
        var model = new PlanoContaModel
        {
            Id = planoConta.Id + 1,
            Descricao = "Salario alterado indevidamente",
            Tipo = 'D'
        };

        // Act
        var result = controller.Cadastrar(model, planoConta.Id);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);

        var planoContaPersistido = repository.RetornarRegistro(planoConta.Id);
        Assert.IsNotNull(planoContaPersistido);
        Assert.AreEqual("Salario", planoContaPersistido.Descricao);
        Assert.AreEqual('R', planoContaPersistido.Tipo);
        Assert.AreEqual(1, repository.ListarRegistros().Count);
    }

    [TestMethod]
    public void DeveExcluirPlanoDeContaERetornarNoContent()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var planoConta = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        repository.Cadastrar(planoConta);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Excluir(planoConta.Id);

        // Assert
        Assert.IsInstanceOfType<NoContentResult>(result);
        Assert.IsNull(repository.RetornarRegistro(planoConta.Id));
    }

    [TestMethod]
    public void DeveRetornarConflictQuandoPlanoDeContaPossuirTransacoesAssociadas()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var planoContaRepository = new PlanoContaRepository(dbContext);
        var transacaoRepository = new TransacaoRepository(dbContext);
        var planoConta = new PlanoConta
        {
            Descricao = "Salario",
            Tipo = 'R'
        };
        planoContaRepository.Cadastrar(planoConta);
        transacaoRepository.Cadastrar(new Transacao
        {
            Historico = "Recebimento do salario",
            Data = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Unspecified),
            Valor = 5000m,
            PlanoContaId = planoConta.Id
        });
        var service = new PlanoContaService(planoContaRepository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Excluir(planoConta.Id);

        // Assert
        var conflictResult = result as ConflictObjectResult;
        Assert.IsNotNull(conflictResult);
        Assert.AreEqual("Não é possível excluir este plano de contas porque ele possui transações associadas.", conflictResult.Value);
        Assert.IsNotNull(planoContaRepository.RetornarRegistro(planoConta.Id));
    }

    [TestMethod]
    public void DeveRetornarConflictQuandoPlanoDeContaNaoExistir()
    {
        // Arrange
        using var dbContext = new TestDatabaseBuilder().Build();
        var repository = new PlanoContaRepository(dbContext);
        var service = new PlanoContaService(repository);
        var controller = new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service);

        // Act
        var result = controller.Excluir(999);

        // Assert
        var conflictResult = result as ConflictObjectResult;
        Assert.IsNotNull(conflictResult);
        Assert.AreEqual("Não é possível excluir este plano de contas porque ele possui transações associadas.", conflictResult.Value);
    }
}