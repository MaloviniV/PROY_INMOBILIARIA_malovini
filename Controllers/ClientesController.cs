using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class ClientesController : Controller
{
  //private readonly IClienteService _clienteService;

  public ClientesController()//pasar por parametro IClienteService clienteService
  {
    //_clienteService = clienteService;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult verificarDni(string dni)
  {
    //BUSCAR LA PERSONA EN LA BD
    var persona = new { Dni = "11111111",
                        Nombre = "Juan",
                        Apellido = "Pérez"
                      };

    if (persona.Dni == dni)
    {
      return Json(new 
      { existe = true,
        nombreCompleto = $"{persona.Apellido} {persona.Nombre}"
      });
    }

    return Json(new
    {
        existe = false,
        nombreCompleto = ""
    });
  }
}