using Microsoft.AspNetCore.Mvc;
using PROY_INMOBILIARIA_malovini.Data;

namespace PROY_INMOBILIARIA_malovini.Controllers;

public class DbTestController : Controller
{
  private readonly DbConnection _dbConnection;

  public DbTestController(DbConnection dbConnection)
  {
    _dbConnection = dbConnection;
  }

  public async Task<IActionResult> Index()
  {
    try
    {
      await using var conexion = _dbConnection.CreateConnection();
      await conexion.OpenAsync();

      return Content("Conexion exitosa");
    }
    catch (Exception e)
    {
      return Content($"Error en la conexion: {e.Message}");
    }
  }
}