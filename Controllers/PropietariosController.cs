using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class PropietariosController : Controller
{
  private readonly IPropietarioService _propietarioService;

  public PropietariosController(IPropietarioService propietarioService)
  {
    _propietarioService = propietarioService;
  }
  //GET: Propietario/Index
  public IActionResult Index()
  {
    return View();
  }
//GET: Propietario/Create
  public IActionResult Create()
  {
    return View();
  }

  //POST: Propietarios/create
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(PropietarioModel propietario)
  {
    if (!ModelState.IsValid)
    {
      return View(propietario);
    }
    //GUARDAR EL PROPIETARIO
    await _propietarioService.Crear(propietario);

    return RedirectToAction(nameof(Index));
  }

  [ValidateAntiForgeryToken]
  public IActionResult Edit(PropietarioModel propietario)
  {
    return RedirectToAction(nameof(Index));
  }
}