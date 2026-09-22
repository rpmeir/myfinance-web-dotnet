namespace MyFinanceWeb.Tests.Unit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Mvc.Controllers;
using MyFinanceWeb.Mvc.Models;
using MyFinanceWeb.Service.Interfaces;

[TestClass]
public class PlanoContaControllerTests
{
    [TestMethod]
    public void DeveCadastrarNovoPlanoDeContaERedirecionarParaIndex()
    {
        // Arrange
        var service = new Mock<IPlanoContaService>();
        var controller = CriarController(service);
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
        service.Verify(item => item.Cadastrar(It.Is<PlanoConta>(planoConta =>
            planoConta.Id == model.Id &&
            planoConta.Descricao == model.Descricao &&
            planoConta.Tipo == model.Tipo)), Times.Once);
    }

    [TestMethod]
    public void DeveEditarPlanoDeContaQuandoIdsForemIguais()
    {
        // Arrange
        var service = new Mock<IPlanoContaService>();
        var controller = CriarController(service);
        var model = new PlanoContaModel
        {
            Id = 7,
            Descricao = "Salario atualizado",
            Tipo = 'R'
        };

        // Act
        var result = controller.Cadastrar(model, model.Id);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
        service.Verify(item => item.Cadastrar(It.Is<PlanoConta>(planoConta =>
            planoConta.Id == model.Id &&
            planoConta.Descricao == model.Descricao &&
            planoConta.Tipo == model.Tipo)), Times.Once);
    }

    [TestMethod]
    public void DeveRetornarBadRequestQuandoIdsForemDiferentes()
    {
        // Arrange
        var service = new Mock<IPlanoContaService>();
        var controller = CriarController(service);
        var model = new PlanoContaModel
        {
            Id = 8,
            Descricao = "Salario",
            Tipo = 'R'
        };

        // Act
        var result = controller.Cadastrar(model, 7);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
        service.Verify(item => item.Cadastrar(It.IsAny<PlanoConta>()), Times.Never);
    }

    private static PlanoContaController CriarController(Mock<IPlanoContaService> service)
    {
        return new PlanoContaController(
            NullLogger<PlanoContaController>.Instance,
            service.Object);
    }
}