namespace MyFinanceWeb.Tests.Integration;

[TestClass]
public sealed class HomeControllerIntegrationTests
{
    [TestMethod]
    public async Task Index_DeveResponderComSucessoSemCredenciaisDoPostgreSql()
    {
        using var factory = new MyFinanceWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        Assert.IsTrue(response.IsSuccessStatusCode);
    }
}