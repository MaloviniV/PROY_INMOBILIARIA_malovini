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
    if (!ModelState.IsValid) return View(propietario);
    
    //GUARDAR EL PROPIETARIO
    bool modificado = await _propietarioService.Crear(propietario);

      if(!modificado)
    {
      TempData["ErrorMessage"] = "No se pudo guardar el propietario.";
      return View(propietario);
    } 

    TempData["SuccessMessage"] = "Propietario guardado correctamente.";

    return RedirectToAction(nameof(Index));
  }


  // GET: Propietarios/Edit
  public async Task<IActionResult> Edit(int id)
  {
    var propietario = await _propietarioService.BuscarPorId(id);

    if (propietario is null)
    {
      ViewBag.ErrorMessage = "El propietario no existe o fue eliminado.";
      return View(nameof(Edit));
    }

    return View(propietario);
  }

  // POST: Propietarios/Edit
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(PropietarioModel propietario)
  {
    if (!ModelState.IsValid) return View(propietario);

    bool modificado = await _propietarioService.Modificar(propietario);

    if(!modificado)
    {
      TempData["ErrorMessage"] = "No se pudo actualizar el propietario.";
      return View(propietario);
    } 

    TempData["SuccessMessage"] = "Propietario actualizado correctamente.";
    return RedirectToAction(nameof(Index));
  }






}