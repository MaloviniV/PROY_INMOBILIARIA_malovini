using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class InquilinosController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}