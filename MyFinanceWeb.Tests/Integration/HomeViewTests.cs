namespace MyFinanceWeb.Tests.Integration;

using System.Net;

[TestClass]
public sealed class HomeViewTests
{
    [TestMethod]
    public async Task Index_DeveRenderizarConteudoELayout()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "Welcome");
        StringAssert.Contains(html, "MyFinanceWeb.Mvc");
        StringAssert.Contains(html, "href=\"/PlanoConta/Index\"");
        StringAssert.Contains(html, "href=\"/Transacao/Index\"");
    }

    [TestMethod]
    public async Task Privacy_DeveRenderizarPaginaComSucesso()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/Home/Privacy");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "Privacy Policy");
    }
}