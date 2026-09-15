using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinanceWeb.Mvc.Models;

namespace MyFinanceWeb.Mvc.Controllers;

[Route("")]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("Index")]
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred.");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
