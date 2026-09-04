using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class TiposInmuebleController : Controller
{
  private readonly ITipoInmuebleService _service;

  public TiposInmuebleController(ITipoInmuebleService service) => _service = service;

  public async Task<IActionResult> Index() => View(await _service.ObtenerLista(1, 100));

  public IActionResult Create() => View(new TipoInmuebleModel());

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(TipoInmuebleModel model)
  {
    if (!ModelState.IsValid) return View(model);
    await _service.Crear(model);
    TempData["SuccessMessage"] = "Tipo de inmueble creado correctamente.";
    return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Edit(int id)
  {
    var model = await _service.BuscarPorId(id);
    return model is null ? NotFound() : View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(TipoInmuebleModel model)
  {
    if (!ModelState.IsValid) return View(model);
    if (!await _service.Modificar(model)) return NotFound();
    TempData["SuccessMessage"] = "Tipo de inmueble actualizado correctamente.";
    return RedirectToAction(nameof(Index));
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      await _service.Eliminar(id);
      TempData["SuccessMessage"] = "Tipo de inmueble eliminado correctamente.";
    }
    catch
    {
      TempData["ErrorMessage"] = "No se puede eliminar un tipo asociado a inmuebles.";
    }
    return RedirectToAction(nameof(Index));
  }
}
