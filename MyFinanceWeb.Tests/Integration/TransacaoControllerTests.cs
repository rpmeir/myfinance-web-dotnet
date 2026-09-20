namespace MyFinanceWeb.Tests.Integration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Repositories;
using MyFinanceWeb.Mvc.Controllers;
using MyFinanceWeb.Mvc.Models;
using MyFinanceWeb.Service;
using MyFinanceWeb.Tests.Database;

[TestClass]
public class TransacaoControllerTests
{
	[TestMethod]
	public void DeveListarDuasTransacoes()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var receita = new PlanoConta
		{
			Descricao = "Salario",
			Tipo = 'R'
		};
		var despesa = new PlanoConta
		{
			Descricao = "Aluguel",
			Tipo = 'D'
		};
		planoContaRepository.Cadastrar(receita);
		planoContaRepository.Cadastrar(despesa);

		var dataReceita = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Unspecified);
		var dataDespesa = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Unspecified);
		var transacaoReceita = new Transacao
		{
			Historico = "Recebimento do salario",
			Data = dataReceita,
			Valor = 5000m,
			PlanoContaId = receita.Id
		};
		var transacaoDespesa = new Transacao
		{
			Historico = "Pagamento do aluguel",
			Data = dataDespesa,
			Valor = 1500m,
			PlanoContaId = despesa.Id
		};
		transacaoRepository.Cadastrar(transacaoReceita);
		transacaoRepository.Cadastrar(transacaoDespesa);

		var transacaoService = new TransacaoService(transacaoRepository);
		var planoContaService = new PlanoContaService(planoContaRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			transacaoService,
			planoContaService);

		// Act
		var result = controller.Index();

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
		Assert.IsNull(viewResult.ViewName);

		var model = viewResult.ViewData["TransacaoList"] as List<TransacaoModel>;
		Assert.IsNotNull(model);
		Assert.AreEqual(2, model.Count);
		Assert.AreNotEqual(0, model[0].Id);
		Assert.AreEqual("Recebimento do salario", model[0].Historico);
		Assert.AreEqual(dataReceita, model[0].Data);
		Assert.AreEqual(5000m, model[0].Valor);
		Assert.AreEqual("Salario", model[0].Tipo);
		Assert.AreEqual(receita.Id, model[0].PlanoContaId);
		Assert.AreNotEqual(0, model[1].Id);
		Assert.AreEqual("Pagamento do aluguel", model[1].Historico);
		Assert.AreEqual(dataDespesa, model[1].Data);
		Assert.AreEqual(-1500m, model[1].Valor);
		Assert.AreEqual("Aluguel", model[1].Tipo);
		Assert.AreEqual(despesa.Id, model[1].PlanoContaId);
	}
}
