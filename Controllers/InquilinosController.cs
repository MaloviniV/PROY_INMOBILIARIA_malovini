using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class InquilinosController : Controller
{
  private readonly IInquilinoService _inquilinoService;

  public InquilinosController(IInquilinoService inquilinoService)
  {
    _inquilinoService = inquilinoService;
  }

  //GET: Propietario/Index
  public async Task<IActionResult> Index()
  {
    IList<InquilinoModel> inquilinos = await _inquilinoService.ObtenerLista();
    return View(inquilinos);
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
      var (persona, inquilino) = await _inquilinoService.BuscarPorDni(busqueda.Dni);

      ViewBag.DniBuscado = busqueda.Dni;
      ViewBag.Persona = persona;
      ViewBag.Inquilino = inquilino;
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
  public async Task<IActionResult> Create(InquilinoModel inquilino)
  {
    if (!ModelState.IsValid) return View(inquilino);
    
    //GUARDAR EL PROPIETARIO
    bool creado = await _inquilinoService.Crear(inquilino);

      if(!creado)
    {
      TempData["ErrorMessage"] = "No se pudo guardar el inquilino.";
      return View(inquilino);
    } 

    TempData["SuccessMessage"] = "Inquilino guardado correctamente.";
    return RedirectToAction(nameof(Index));
  }


  // GET: Propietarios/Edit
  public async Task<IActionResult> Edit(int id)
  {
    var inquilino = await _inquilinoService.BuscarPorId(id);

    if (inquilino is null)
    {
      TempData["ErrorMessage"] = "El inquilino no existe o fue eliminado.";
      return RedirectToAction(nameof(Index));
    }

    return View(inquilino);
  }

  // POST: Inquilinos/Edit
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(InquilinoModel inquilino)
  {
    if (!ModelState.IsValid) return View(inquilino);

    bool modificado = await _inquilinoService.Modificar(inquilino);

    if(!modificado)
    {
      TempData["ErrorMessage"] = "No se pudo actualizar el inquilino.";
      return View(inquilino);
    } 

    TempData["SuccessMessage"] = "Inquilino actualizado correctamente.";
    return RedirectToAction(nameof(Index));
  }

  //GET: Inquilinos/details/?
  public async Task<IActionResult> Details(int id)
  {
    var inquilino = await _inquilinoService.BuscarPorId(id);

    if (inquilino is null)
    {
      TempData["ErrorMessage"] = "El inquilino no existe o fue eliminado.";
      return RedirectToAction(nameof(Index));
    }

    return View(inquilino);
  }

}