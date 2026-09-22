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
	public void DeveListarTodasAsTransacoes()
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

	[TestMethod]
	public void DeveCarregarFormularioDeCadastroComValoresPadrao()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var planoContaService = new PlanoContaService(planoContaRepository);
		var transacaoService = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			transacaoService,
			planoContaService);

		// Act
		var result = controller.Cadastrar(null);

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
		Assert.IsNull(viewResult.ViewName);
		Assert.IsNotNull(viewResult.Model);

		var model = viewResult.Model as TransacaoModel;
		Assert.IsNotNull(model);
		Assert.AreEqual(DateTime.Today, model.Data);
		Assert.IsNotNull(viewResult.ViewData["PlanoContaList"]);
	}

	[TestMethod]
	public void DeveRetornarNotFoundAoCarregarTransacaoInexistente()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var planoContaService = new PlanoContaService(planoContaRepository);
		var transacaoService = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			transacaoService,
			planoContaService);

		// Act
		var result = controller.Cadastrar(999);

		// Assert
		Assert.IsInstanceOfType<NotFoundResult>(result);
		Assert.IsNotNull(controller.ViewBag.PlanoContaList);
	}

	[TestMethod]
	public void DeveCarregarTransacaoExistenteNoFormulario()
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
		var data = new DateTime(2026, 9, 10, 0, 0, 0, DateTimeKind.Unspecified);
		var transacao = new Transacao
		{
			Historico = "Recebimento",
			Data = data,
			Valor = 5000m,
			PlanoContaId = planoConta.Id
		};
		transacaoRepository.Cadastrar(transacao);
		var planoContaService = new PlanoContaService(planoContaRepository);
		var transacaoService = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			transacaoService,
			planoContaService);

		// Act
		var result = controller.Cadastrar(transacao.Id);

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
		Assert.IsNull(viewResult.ViewName);
		var model = viewResult.Model as TransacaoModel;
		Assert.IsNotNull(model);
		Assert.AreEqual(transacao.Id, model.Id);
		Assert.AreEqual(transacao.Historico, model.Historico);
		Assert.AreEqual(data, model.Data);
		Assert.AreEqual(transacao.Valor, model.Valor);
		Assert.AreEqual(planoConta.Tipo.ToString(), model.Tipo);
		Assert.AreEqual(planoConta.Id, model.PlanoContaId);
	}

	[TestMethod]
	public void DeveCadastrarNovaTransacaoNaBaseDeDadosERedirecionarParaIndex()
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
		var service = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			service,
			new PlanoContaService(planoContaRepository));
		var data = new DateTime(2026, 9, 11, 0, 0, 0, DateTimeKind.Unspecified);
		var model = new TransacaoModel
		{
			Historico = "Recebimento",
			Data = data,
			Valor = 5000m,
			PlanoContaId = planoConta.Id
		};

		// Act
		var result = controller.Cadastrar(model, null);

		// Assert
		var redirectResult = result as RedirectToActionResult;
		Assert.IsNotNull(redirectResult);
		Assert.AreEqual("Index", redirectResult.ActionName);
		var persistido = transacaoRepository.ListarRegistros().Single();
		Assert.AreNotEqual(0, persistido.Id);
		Assert.AreEqual(model.Historico, persistido.Historico);
		Assert.AreEqual(model.Data, persistido.Data);
		Assert.AreEqual(model.Valor, persistido.Valor);
		Assert.AreEqual(model.PlanoContaId, persistido.PlanoContaId);
	}

	[TestMethod]
	public void DeveAtualizarTransacaoNaBaseDeDadosQuandoIdsForemIguais()
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
		var transacao = new Transacao
		{
			Historico = "Recebimento",
			Data = new DateTime(2026, 9, 12, 0, 0, 0, DateTimeKind.Unspecified),
			Valor = 5000m,
			PlanoContaId = planoConta.Id
		};
		transacaoRepository.Cadastrar(transacao);
		var service = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			service,
			new PlanoContaService(planoContaRepository));
		var model = new TransacaoModel
		{
			Id = transacao.Id,
			Historico = "Recebimento atualizado",
			Data = transacao.Data,
			Valor = 5500m,
			PlanoContaId = planoConta.Id
		};

		// Act
		var result = controller.Cadastrar(model, transacao.Id);

		// Assert
		var redirectResult = result as RedirectToActionResult;
		Assert.IsNotNull(redirectResult);
		Assert.AreEqual("Index", redirectResult.ActionName);
		var atualizado = transacaoRepository.RetornarRegistro(transacao.Id);
		Assert.IsNotNull(atualizado);
		Assert.AreEqual(model.Historico, atualizado.Historico);
		Assert.AreEqual(model.Valor, atualizado.Valor);
	}

	[TestMethod]
	public void DeveRetornarBadRequestQuandoIdsForemDiferentes()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var planoConta = new PlanoConta { Descricao = "Salario", Tipo = 'R' };
		planoContaRepository.Cadastrar(planoConta);
		var service = new TransacaoService(transacaoRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			service,
			new PlanoContaService(planoContaRepository));
		var model = new TransacaoModel
		{
			Id = 2,
			Historico = "Recebimento",
			Data = DateTime.Today,
			Valor = 100m,
			PlanoContaId = planoConta.Id
		};

		// Act
		var result = controller.Cadastrar(model, 1);

		// Assert
		Assert.IsInstanceOfType<BadRequestResult>(result);
		Assert.AreEqual(0, transacaoRepository.ListarRegistros().Count);
	}

	[TestMethod]
	public void DeveRetornarFormularioComErrosQuandoModelStateForInvalido()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var planoContaService = new PlanoContaService(planoContaRepository);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			new TransacaoService(transacaoRepository),
			planoContaService);
		controller.ModelState.AddModelError("PlanoContaId", "Informe o plano de conta.");
		var model = new TransacaoModel
		{
			Historico = "Recebimento",
			Data = DateTime.Today,
			Valor = 100m
		};

		// Act
		var result = controller.Cadastrar(model, null);

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
		Assert.IsNull(viewResult.ViewName);
		Assert.AreSame(model, viewResult.Model);
		Assert.IsNotNull(viewResult.ViewData["PlanoContaList"]);
		Assert.AreEqual(0, transacaoRepository.ListarRegistros().Count);
	}

	[TestMethod]
	public void DeveExcluirTransacaoERetornarNoContent()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var planoConta = new PlanoConta { Descricao = "Salario", Tipo = 'R' };
		planoContaRepository.Cadastrar(planoConta);
		var transacao = new Transacao
		{
			Historico = "Recebimento",
			Data = DateTime.Today,
			Valor = 100m,
			PlanoContaId = planoConta.Id
		};
		transacaoRepository.Cadastrar(transacao);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			new TransacaoService(transacaoRepository),
			new PlanoContaService(planoContaRepository));

		// Act
		var result = controller.Excluir(transacao.Id);

		// Assert
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.IsNull(transacaoRepository.RetornarRegistro(transacao.Id));
	}

	[TestMethod]
	public void DeveRetornarNoContentAoExcluirTransacaoInexistente()
	{
		// Arrange
		using var dbContext = new TestDatabaseBuilder().Build();
		var planoContaRepository = new PlanoContaRepository(dbContext);
		var transacaoRepository = new TransacaoRepository(dbContext);
		var controller = new TransacaoController(
			NullLogger<TransacaoController>.Instance,
			new TransacaoService(transacaoRepository),
			new PlanoContaService(planoContaRepository));

		// Act
		var result = controller.Excluir(999);

		// Assert
		Assert.IsInstanceOfType<NoContentResult>(result);
	}
}
