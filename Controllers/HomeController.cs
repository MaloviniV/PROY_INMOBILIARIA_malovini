using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
