namespace MyFinanceWeb.Tests.Integration;

using System.Net;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra;
using MyFinanceWeb.Infra.Repositories;

[TestClass]
public sealed class PlanoContaViewTests
{
    [TestMethod]
    public async Task Index_DeveRenderizarPlanoPersistido()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        SeedPlanoConta(factory, "Salario", 'R');
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/PlanoConta");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "<h1>Plano de Contas</h1>");
        StringAssert.Contains(html, "Salario");
        StringAssert.Contains(html, ">R<");
        StringAssert.Contains(html, "Criar item de Plano Conta");
        StringAssert.Contains(html, "PlanoConta/Index.js");
    }

    [TestMethod]
    public async Task Cadastrar_DeveRenderizarFormulario()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/PlanoConta/Cadastrar");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "<h1>Cadastrar Plano de Conta</h1>");
        StringAssert.Contains(html, "name=\"Descricao\"");
        StringAssert.Contains(html, "id=\"Tipo_R\"");
        StringAssert.Contains(html, "id=\"Tipo_D\"");
        StringAssert.Contains(html, "Salvar");
        StringAssert.Contains(html, "PlanoConta/Cadastrar");
    }

    private static void SeedPlanoConta(
        MyFinanceWebApplicationFactory factory,
        string descricao,
        char tipo)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
        new PlanoContaRepository(dbContext).Cadastrar(new PlanoConta
        {
            Descricao = descricao,
            Tipo = tipo
        });
    }
}