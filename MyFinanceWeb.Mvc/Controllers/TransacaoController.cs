namespace MyFinanceWeb.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Mvc.Models;
using MyFinanceWeb.Service.Interfaces;

[Route("[controller]")]
public class TransacaoController : Controller
{
    private readonly ILogger<TransacaoController> _logger;
    private readonly ITransacaoService _transacaoService;
    private readonly IPlanoContaService _planoContaService;

    public TransacaoController(
        ILogger<TransacaoController> logger,
        ITransacaoService transacaoService,
        IPlanoContaService planoContaService)
    {
        _logger = logger;
        _transacaoService = transacaoService;
        _planoContaService = planoContaService;
    }

    [HttpGet]
    [Route("Index")]
    public IActionResult Index()
    {
        var transacaoList = _transacaoService.ListarRegistros();
        List<TransacaoModel> transacaoModelList = new List<TransacaoModel>();
        foreach (var transacao in transacaoList)
        {
            transacaoModelList.Add(new TransacaoModel
            {
                Id = transacao.Id,
                Historico = transacao.Historico,
                Data = transacao.Data,
                Valor = transacao.Valor * (transacao.PlanoConta.Tipo == 'D' ? -1 : 1),
                Tipo = transacao.PlanoConta.Descricao,
                PlanoContaId = transacao.PlanoContaId
            });
        }
        ViewBag.TransacaoList = transacaoModelList;
        return View();
    }

    [HttpGet]
    [Route("Cadastrar/{id?}")]
    public IActionResult Cadastrar(int? id)
    {
        ViewBag.PlanoContaList = _planoContaService.ListarRegistros();

        if (!id.HasValue)
        {
            return View(new TransacaoModel
            {
                Data = DateTime.Today
            });
        }
        var transacao = _transacaoService.RetornarRegistro(id.Value);
        if (transacao == null)
        {
            return NotFound();
        }
        var transacaoModel = new TransacaoModel
        {
            Id = transacao.Id,
            Historico = transacao.Historico,
            Data = transacao.Data,
            Valor = transacao.Valor,
            Tipo = transacao.PlanoConta.Tipo.ToString(),
            PlanoContaId = transacao.PlanoContaId
        };
        return View(transacaoModel);
    }

    [HttpPost]
    [Route("Cadastrar/{id?}")]
    public IActionResult Cadastrar(TransacaoModel transacaoModel, int? id)
    {
        if (id.HasValue && id.Value > 0 && transacaoModel.Id != id.Value)
        {
            return BadRequest();
        }

        _transacaoService.Cadastrar(new Transacao
        {
            Id = transacaoModel.Id,
            Historico = transacaoModel.Historico,
            Data = transacaoModel.Data,
            Valor = transacaoModel.Valor,
            PlanoContaId = transacaoModel.PlanoContaId
        });

        return RedirectToAction("Index");
    }

    [HttpDelete]
    [Route("Excluir/{id}")]
    public IActionResult Excluir(int id)
    {
        _transacaoService.Excluir(id);
        return NoContent();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred.");
        return View("Error");
    }
}