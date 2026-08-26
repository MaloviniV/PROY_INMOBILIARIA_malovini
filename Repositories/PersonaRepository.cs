using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PersonaRepository : IPersonaRepository
{
  private readonly DbConnection _dbConnection;

  public PersonaRepository(DbConnection conn)
  {
    _dbConnection=conn;
  }

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

  public async Task<PersonaModel?> ObtenerPorDni(string dni)
  {
    await using var conexion = _dbConnection.CreateConnection();
    await conexion.OpenAsync();

    const string sql = @"SELECT id, apellido, nombre, dni, mail, telefono, direccion 
                        FROM personas
                        WHERE dni=@dni";

    await using var comandoPersona = new MySqlCommand(sql,conexion);
    comandoPersona.Parameters.AddWithValue("@dni", dni);

    await using var reader = await comandoPersona.ExecuteReaderAsync();

    if (reader.Read())
    {
      return new PersonaModel
      {
        Id = reader.GetInt32("id"),
        Apellido = reader.GetString("apellido"),
        Nombre = reader.GetString("nombre"),
        Dni = reader.GetString("dni"),
        Mail = reader.GetString("mail"),
        Telefono = reader.GetString("telefono"),
        Direccion = reader.GetString("direccion"),
      };
    }
    return null;
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