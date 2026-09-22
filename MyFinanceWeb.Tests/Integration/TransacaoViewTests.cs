namespace MyFinanceWeb.Tests.Integration;

using System.Net;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra;
using MyFinanceWeb.Infra.Repositories;

[TestClass]
public sealed class TransacaoViewTests
{
    [TestMethod]
    public async Task Index_DeveRenderizarTransacaoPersistida()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        SeedTransacao(factory);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/Transacao");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "<h1>Transa&#xE7;&#xF5;es</h1>");
        StringAssert.Contains(html, "Recebimento");
        StringAssert.Contains(html, "10/09/2026");
        StringAssert.Contains(html, "Criar item de Transação");
        StringAssert.Contains(html, "Transacao/Index.js");
    }

    [TestMethod]
    public async Task Cadastrar_DeveRenderizarCamposDoFormulario()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        SeedPlanoConta(factory, "Salario", 'R');
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/Transacao/Cadastrar");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "<h1>Cadastrar Transa&#xE7;&#xE3;o</h1>");
        StringAssert.Contains(html, "name=\"Data\"");
        StringAssert.Contains(html, "name=\"ValorDisplay\"");
        StringAssert.Contains(html, "name=\"PlanoContaId\"");
        StringAssert.Contains(html, "Salario");
        StringAssert.Contains(html, "name=\"Historico\"");
        StringAssert.Contains(html, "Transacao/Cadastrar.js");
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

    private static void SeedTransacao(MyFinanceWebApplicationFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
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
            Historico = "Recebimento",
            Data = new DateTime(2026, 9, 10, 0, 0, 0, DateTimeKind.Unspecified),
            Valor = 5000m,
            PlanoContaId = planoConta.Id
        });
    }
}