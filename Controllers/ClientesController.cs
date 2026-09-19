using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Services;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class ClientesController : Controller
{
  private readonly IClienteService _clienteService;
  private readonly IPropietarioService _propietarioService;
  private readonly IInquilinoService _inquilinoService;
  private readonly IInmuebleService _inmuebleService;
  private readonly IReservaService _reservaService;

  public ClientesController(
    IClienteService clienteService,
    IPropietarioService propService,
    IInquilinoService inqService,
    IInmuebleService inmuebleService,
    IReservaService reservaService)
  {
    _clienteService = clienteService;
    _propietarioService = propService;
    _inquilinoService = inqService;
    _inmuebleService = inmuebleService;
    _reservaService = reservaService;
  }

  public async Task<IActionResult> Index()
  {
    return View();
  }

  public async Task<IActionResult> Manager([FromQuery] string? dni, [FromQuery] string modo = "crear")
  {
    var modelo = new ClienteManagerViewModel { Modo = modo };

    //MODO CREAR CON O SIN DNI
    if (modo == "crear")
    {
      modelo.Persona.Dni = dni?.Trim() ?? "";
      modelo.Propietario = null;
      modelo.Inquilino = null;
      return View(modelo);
    }

    //MODO LECTURA SIN DNI (ERROR) ENVIA A LISTADO DE CLIENTES
    if (string.IsNullOrWhiteSpace(dni))
      return RedirectToAction(nameof(Index));

    var persona = await _clienteService.BuscarPorDni(dni.Trim());

    //SI LA BUSQUEDA DA NULL REDIRIGE CON EL MODO CREAR
    if (persona is null)
      return RedirectToAction(nameof(Manager), new { modo = "crear", dni });

    modelo.Persona = persona;
    modelo.Propietario = await _propietarioService.BuscarPorId(persona.Id);
    modelo.Inquilino = await _inquilinoService.BuscarPorId(persona.Id);

    if (modelo.Propietario is not null)
    {
      modelo.InmueblesPropietario = (await _inmuebleService.ObtenerPorPropietario(persona.Id))
                                    ?? new List<InmuebleModel>();
    }

    if (modelo.Inquilino is not null)
    {
      modelo.AlquileresInquilino = (await _reservaService.ObtenerPorInquilino(persona.Id))
                                    ?? new List<ReservaModel>();      
    }

    return View(modelo);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> GuardarPersona(ClienteManagerViewModel modelo)
  {
    if (!ModelState.IsValid)
    {
      return View("Manager", modelo);
    }

    if (modelo.Persona is null)
    {
      return ErrorDeGuardado(modelo, "No se recibieron los datos del cliente.");
    }

    bool modificado = await _clienteService.Modificar(modelo.Persona);

    if (!modificado)
    {
      return ErrorDeGuardado(modelo, "No se pudieron guardar los cambios del cliente.");
    }

    return RedirectAfterSave(modelo.Persona.Dni, "Datos del cliente guardados correctamente.");
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> GuardarPropietario(ClienteManagerViewModel modelo)
  {
    if (!ModelState.IsValid)
    {
      return View("Manager", modelo);
    }

    if (modelo.Propietario is null)
    {
      return ErrorDeGuardado(modelo, "No se recibieron los datos del propietario.");
    }

    var persona = ObtenerPersonaRelacionada(modelo, modelo.Propietario.IdPersona);
    modelo.Propietario.Persona = persona;
    modelo.Propietario.IdPersona = persona.Id;

    bool modificado = await _propietarioService.Modificar(modelo.Propietario);

    if (!modificado)
    {
      return ErrorDeGuardado(modelo, "No se pudieron guardar los cambios del propietario.");
    }

    return RedirectAfterSave(persona.Dni, "Datos del propietario guardados correctamente.");
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> GuardarInquilino(ClienteManagerViewModel modelo)
  {
    if (!ModelState.IsValid)
    {
      return View("Manager", modelo);
    }

    if (modelo.Inquilino is null)
    {
      return ErrorDeGuardado(modelo, "No se recibieron los datos del inquilino.");
    }

    var persona = ObtenerPersonaRelacionada(modelo, modelo.Inquilino.IdPersona);
    modelo.Inquilino.Persona = persona;
    modelo.Inquilino.IdPersona = persona.Id;

    bool modificado = await _inquilinoService.Modificar(modelo.Inquilino);

    if (!modificado)
    {
      return ErrorDeGuardado(modelo, "No se pudieron guardar los cambios del inquilino.");
    }

    return RedirectAfterSave(persona.Dni, "Datos del inquilino guardados correctamente.");
  }

  private IActionResult ErrorDeGuardado(ClienteManagerViewModel modelo, string mensaje)
  {
    ModelState.AddModelError(string.Empty, mensaje);
    return View("Manager", modelo);
  }

  private IActionResult RedirectAfterSave(string dni, string mensaje)
  {
    TempData["SuccessMessage"] = mensaje;
    return RedirectToAction(nameof(Manager), new { modo = "edicion", dni });
  }

  private static PersonaModel ObtenerPersonaRelacionada(ClienteManagerViewModel modelo, int fallbackId)
  {
    var persona = modelo.Persona ?? new PersonaModel();
    var personaId = modelo.Persona?.Id > 0 ? modelo.Persona.Id : fallbackId;

    persona.Id = personaId;
    persona.Dni = modelo.Persona?.Dni ?? string.Empty;

    return persona;
  }

  public IActionResult Create([FromQuery] DniValidatorModel query)
  {
    if (!ModelState.IsValid)
    {
      Console.WriteLine("ERROR AL RECIBIR EL DNI DESDE EL CLIENTE");
      return RedirectToAction(nameof(Index));
    }

    var nuevoCliente = new PersonaModel
    {
      Dni = query.Dni.Trim()
    };

    return View(nuevoCliente);
  }
  public async Task<IActionResult> Details([FromQuery] DniValidatorModel query)
  {
    if (!ModelState.IsValid)
    {
      Console.WriteLine("ERROR AL RECIBIR EL DNI DESDE EL CLIENTE");
      return RedirectToAction(nameof(Index));
    }

    string dniPersona = query.Dni.Trim();

    var cliente = await _clienteService.BuscarPorDni(dniPersona);

    if (cliente == null)
    {
      return RedirectToAction(nameof(Create), new { dni = dniPersona });
    }

    return View(cliente);
  }
  //-------------------------------------------------------------------------------------

  //***** BUSQUEDA DE CLIENTE (PERSONA) POR DNI ******
  public async Task<IActionResult> BuscarPorDni([FromQuery] DniValidatorModel query)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(new { message = "El DNI ingresado no es válido." });
    }

    try
    {
      string dniPersona = query.Dni;

      var persona = await _clienteService.BuscarPorDni(dniPersona);

      return Json(new
      {
        success = persona is not null,
        data = persona is null ? null : new
        {
          dni = persona.Dni,
          nombre = persona.Nombre,
          apellido = persona.Apellido
        }
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        success = false,
        message = "Error al buscar el cliente por DNI.",
        error = ex.Message
      });
    }
  }

  //***** LISTA DE CLIENTES (PERSONAS) ******
  public async Task<IActionResult> ListarClientes()
  {
    try
    {
      var clientes = await _clienteService.ObtenerClientes(1, 1000);

      return Json(new
      {
        success = true,
        data = clientes
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        success = false,
        message = "Error al obtener el listado de clientes.",
        error = ex.Message
      });
    }
  }

}