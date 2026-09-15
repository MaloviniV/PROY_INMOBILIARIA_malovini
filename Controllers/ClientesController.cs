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
    var modelo = new ClienteManagerViewModel
    {
      Modo = modo
    };
    //MODO CREAR CON O SIN DNI
    if (modo == "crear")
    {
      modelo.Persona.Dni = dni?.Trim() ?? "";
      modelo.Propietario = new PropietarioModel
      {
        IdPersona = modelo.Persona.Id,
        Persona = modelo.Persona,
        Cbu = string.Empty,
        Cuit = string.Empty
      };
      modelo.Inquilino = new InquilinoModel
      {
        IdPersona = modelo.Persona.Id,
        Persona = modelo.Persona,
        Profesion = string.Empty
      };
      return View(modelo);
    }

    //MODO LECTURA/EDICION SIN DNI (ERROR) ENVIA A LISTADO DE CLIENTES
    if (string.IsNullOrWhiteSpace(dni))
      return RedirectToAction(nameof(Index));

    var persona = await _clienteService.BuscarPorDni(dni.Trim());

    //SI LA BUSQUEDA DA NULL REDIRIGE CON EL MODO CREAR
    if (persona is null)
      return RedirectToAction(nameof(Manager), new { modo = "crear", dni });

    modelo.Persona = persona;
    modelo.Propietario = await _propietarioService.BuscarPorId(persona.Id);
    modelo.Inquilino = await _inquilinoService.BuscarPorId(persona.Id);

    modelo.Propietario ??= new PropietarioModel
    {
      IdPersona = persona.Id,
      Persona = persona,
      Cbu = string.Empty,
      Cuit = string.Empty
    };
    modelo.Inquilino ??= new InquilinoModel
    {
      IdPersona = persona.Id,
      Persona = persona,
      Profesion = string.Empty
    };

    var inmuebles = await _inmuebleService.ObtenerLista(1, 1000);
    modelo.InmueblesPropietario = inmuebles
      .Where(inmueble => inmueble.IdPropietario == persona.Id)
      .ToList();

    var reservas = await _reservaService.ObtenerLista(1, 1000);
    modelo.AlquileresInquilino = reservas
      .Where(reserva => reserva.IdInquilino == persona.Id)
      .ToList();

    return View(modelo);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Manager(ClienteManagerViewModel modelo)
  {
    if (!ModelState.IsValid)
    {
      return View(modelo);
    }

    bool modificado;

    switch (modelo.SeccionGuardada)
    {
      case "persona":
        modificado = await _clienteService.Modificar(modelo.Persona);
        break;
      case "propietario" when modelo.Propietario is not null:
        modelo.Propietario.Persona = modelo.Persona;
        modelo.Propietario.IdPersona = modelo.Persona.Id;
        modificado = await _propietarioService.Modificar(modelo.Propietario);
        break;
      case "inquilino" when modelo.Inquilino is not null:
        modelo.Inquilino.Persona = modelo.Persona;
        modelo.Inquilino.IdPersona = modelo.Persona.Id;
        modificado = await _inquilinoService.Modificar(modelo.Inquilino);
        break;
      default:
        ModelState.AddModelError(string.Empty, "No se indicó una sección válida para guardar.");
        return View(modelo);
    }

    if (!modificado)
    {
      ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios.");
      return View(modelo);
    }

    TempData["SuccessMessage"] = "Cambios guardados correctamente.";
    return RedirectToAction(nameof(Manager), new { modo = "edicion", dni = modelo.Persona.Dni });
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