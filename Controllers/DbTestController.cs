using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Data;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class DbTestController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

  public DbTestController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<IActionResult> Index()
  {
    try
    {
      await _unitOfWork.Connection();

      return Content("Conexion exitosa");
    }
    catch (Exception e)
    {
      return Content($"Error en la conexion: {e.Message}");
    }
  }
}