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

  // GET: Propietarios/Gestionar
  public async Task<IActionResult> Gestionar(BusquedaDniViewModel? busqueda)
  {
    //Si el input DNI tiene espacios vacios devuelvo los campos vacios
    if (busqueda is null || string.IsNullOrWhiteSpace(busqueda.Dni))
    {
      return View(new BusquedaDniViewModel());
    }

    //Si el input tiene un formato valido ingreso los datos necesarios para la vista
    if (ModelState.IsValid)
    {
      var (persona, propietario) = await _propietarioService.BuscarPorDni(busqueda.Dni);

      ViewBag.DniBuscado = busqueda.Dni;
      ViewBag.Persona = persona;
      ViewBag.Propietario = propietario;
    }

    return View(busqueda);
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