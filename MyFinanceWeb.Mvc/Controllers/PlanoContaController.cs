namespace MyFinanceWeb.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Mvc.Models;
using MyFinanceWeb.Service.Interfaces;

[Route("[controller]")]
public class PlanoContaController : Controller
{
    private readonly ILogger<PlanoContaController> _logger;
    private readonly IPlanoContaService _planoContaService;

    public PlanoContaController(ILogger<PlanoContaController> logger, IPlanoContaService planoContaService)
    {
        _logger = logger;
        _planoContaService = planoContaService;
    }

    [HttpGet]
    [Route("Index")]
    public IActionResult Index()
    {
        var planoContaList = _planoContaService.ListarRegistros();
        List<PlanoContaModel> planoContaModelList = new List<PlanoContaModel>();
        foreach (var planoConta in planoContaList)
        {
            planoContaModelList.Add(new PlanoContaModel
            {
                Id = planoConta.Id,
                Descricao = planoConta.Descricao,
                Tipo = planoConta.Tipo
            });
        }
        ViewBag.PlanoContaList = planoContaModelList;
        return View();
    }

    [HttpGet]
    [Route("Cadastrar/{id?}")]
    public IActionResult Cadastrar(int? id)
    {
        if (!id.HasValue)
        {
            return View();
        }
        var planoConta = _planoContaService.RetornarRegistro(id.Value);
        if (planoConta == null)
        {
            return View();
        }
        var planoContaModel = new PlanoContaModel
        {
            Id = planoConta.Id,
            Descricao = planoConta.Descricao,
            Tipo = planoConta.Tipo
        };
        return View(planoContaModel);
    }

    [HttpPost]
    [Route("Cadastrar/{id?}")]
    public IActionResult Cadastrar(PlanoContaModel planoContaModel, int? id)
    {
        if (id.HasValue && id.Value > 0 && planoContaModel.Id != id.Value)
        {
            return BadRequest();
        }

        _planoContaService.Cadastrar(new PlanoConta
        {
            Id = planoContaModel.Id,
            Descricao = planoContaModel.Descricao,
            Tipo = planoContaModel.Tipo
        });

        return RedirectToAction("Index");
    }

    [HttpDelete]
    [Route("Excluir/{id}")]
    public IActionResult Excluir(int id)
    {
        _planoContaService.Excluir(id);
        return NoContent();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred.");
        return View("Error");
    }
}