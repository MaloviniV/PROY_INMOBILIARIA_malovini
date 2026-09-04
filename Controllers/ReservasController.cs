using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class ReservasController : Controller
{
  private readonly IReservaService _service;
  private readonly IInquilinoService _inquilinoService;
  private readonly IInmuebleService _inmuebleService;

  public ReservasController(IReservaService service, IInquilinoService inquilinoService, IInmuebleService inmuebleService)
  {
    _service = service;
    _inquilinoService = inquilinoService;
    _inmuebleService = inmuebleService;
  }

  public async Task<IActionResult> Index() => View(await _service.ObtenerLista(1, 100));

  public async Task<IActionResult> Create()
  {
    await CargarOpciones();
    return View(new ReservaModel { FechaDesde = DateTime.Today, FechaHasta = DateTime.Today.AddDays(1) });
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(ReservaModel model)
  {
    if (!ModelState.IsValid)
    {
      await CargarOpciones(model);
      return View(model);
    }
    await _service.Crear(model);
    TempData["SuccessMessage"] = "Reserva creada correctamente.";
    return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Edit(int id)
  {
    var model = await _service.BuscarPorId(id);
    if (model is null) return NotFound();
    await CargarOpciones(model);
    return View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(ReservaModel model)
  {
    if (!ModelState.IsValid)
    {
      await CargarOpciones(model);
      return View(model);
    }
    try
    {
      if (!await _service.Modificar(model)) return NotFound();
    }
    catch (ArgumentException ex)
    {
      ModelState.AddModelError(string.Empty, ex.Message);
      await CargarOpciones(model);
      return View(model);
    }
    TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
    return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Details(int id)
  {
    var model = await _service.BuscarPorId(id);
    return model is null ? NotFound() : View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(int id)
  {
    await _service.Eliminar(id);
    TempData["SuccessMessage"] = "Reserva eliminada correctamente.";
    return RedirectToAction(nameof(Index));
  }

  private async Task CargarOpciones(ReservaModel? model = null)
  {
    var inquilinos = await _inquilinoService.ObtenerLista(1, 100);
    var inmuebles = await _inmuebleService.ObtenerLista(1, 100);
    ViewBag.Inquilinos = inquilinos.Select(i => new SelectListItem
    {
      Value = i.IdPersona.ToString(),
      Text = $"{i.Persona.Nombre} {i.Persona.Apellido}",
      Selected = model?.IdInquilino == i.IdPersona
    });
    ViewBag.Inmuebles = inmuebles.Select(i => new SelectListItem
    {
      Value = i.Id.ToString(),
      Text = $"{i.Direccion} - {i.NombreTipo}",
      Selected = model?.IdInmueble == i.Id
    });
  }
}
