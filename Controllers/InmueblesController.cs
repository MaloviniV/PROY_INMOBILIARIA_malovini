using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class InmueblesController : Controller
{
  private readonly IInmuebleService _service;
  private readonly IPropietarioService _propietarioService;
  private readonly ITipoInmuebleService _tipoService;

  public InmueblesController(IInmuebleService service, IPropietarioService propietarioService, ITipoInmuebleService tipoService)
  {
    _service = service;
    _propietarioService = propietarioService;
    _tipoService = tipoService;
  }

  public async Task<IActionResult> Index() => View(await _service.ObtenerLista(1, 100));

  public async Task<IActionResult> Create()
  {
    await CargarOpciones();
    return View(new InmuebleModel());
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(InmuebleModel model)
  {
    if (!ModelState.IsValid)
    {
      await CargarOpciones(model);
      return View(model);
    }
    await _service.Crear(model);
    TempData["SuccessMessage"] = "Inmueble creado correctamente.";
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
  public async Task<IActionResult> Edit(InmuebleModel model)
  {
    if (!ModelState.IsValid)
    {
      await CargarOpciones(model);
      return View(model);
    }
    if (!await _service.Modificar(model)) return NotFound();
    TempData["SuccessMessage"] = "Inmueble actualizado correctamente.";
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
    try
    {
      await _service.Eliminar(id);
      TempData["SuccessMessage"] = "Inmueble eliminado correctamente.";
    }
    catch
    {
      TempData["ErrorMessage"] = "No se puede eliminar el inmueble porque tiene reservas asociadas.";
    }
    return RedirectToAction(nameof(Index));
  }

  private async Task CargarOpciones(InmuebleModel? model = null)
  {
    var propietarios = await _propietarioService.ObtenerLista(1, 100);
    var tipos = await _tipoService.ObtenerLista(1, 100);
    ViewBag.Propietarios = propietarios.Select(p => new SelectListItem
    {
      Value = p.IdPersona.ToString(),
      Text = $"{p.Persona.Nombre} {p.Persona.Apellido}",
      Selected = model?.IdPropietario == p.IdPersona
    });
    ViewBag.Tipos = tipos.Select(t => new SelectListItem
    {
      Value = t.Id.ToString(),
      Text = t.Nombre,
      Selected = model?.IdTipo == t.Id
    });
  }
}
