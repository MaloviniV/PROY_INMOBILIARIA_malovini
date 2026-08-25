using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PersonaRepository : IPersonaRepository
{
  public Task<int> Crear(PersonaModel p)
  {
    throw new NotImplementedException();
  }

  public Task<int> Eliminar(int id)
  {
    throw new NotImplementedException();
  }

  public Task<int> Modificar(PersonaModel p)
  {
    throw new NotImplementedException();
  }

  public Task<int> ObtenerCantidad()
  {
    throw new NotImplementedException();
  }

  public Task<IList<PersonaModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    throw new NotImplementedException();
  }

  public Task<PersonaModel?> ObtenerPorDni(string dni)
  {
    throw new NotImplementedException();
  }

  public Task<PersonaModel?> ObtenerPorId(int id)
  {
    throw new NotImplementedException();
  }

  public Task<PersonaModel?> ObtenerPorMail(string mail)
  {
    throw new NotImplementedException();
  }
}