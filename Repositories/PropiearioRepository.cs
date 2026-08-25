using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PropietarioRepository : IPropietarioRepository
{
  private readonly DbConnection _dbConnection;

  public PropietarioRepository(DbConnection conn)
  {
    _dbConnection = conn;
  }

  public async Task<int> Crear(PropietarioModel propietario)
  {
    await using var conexion = _dbConnection.CreateConnection();
    await conexion.OpenAsync();

    const string sql = @"INSERT INTO propietarios (id_persona, cbu, cuit, estado)
                          VALUES (@id_persona, @cbu, @cuit, @estado);";

    await using var command = new MySqlCommand(sql, conexion);

    command.Parameters.AddWithValue("@id_persona", propietario.IdPersona);
    command.Parameters.AddWithValue("@cbu", propietario.Cbu);
    command.Parameters.AddWithValue("@cuit", propietario.Cuit);
    command.Parameters.AddWithValue("@estado", true);

    return await command.ExecuteNonQueryAsync();
  }

  public async Task<int> Eliminar(int id)
  {
    await using var conexion = _dbConnection.CreateConnection();
    await conexion.OpenAsync();

    const string sql = @"DELETE FROM propietarios 
                        WHERE id_persona = @id_persona;";

    await using var command = new MySqlCommand(sql, conexion);

    command.Parameters.AddWithValue("@id_persona", id);

    return await command.ExecuteNonQueryAsync();
  }

  public async Task<int> Modificar(PropietarioModel p)
  {
    throw new NotImplementedException();
  }

  public async Task<int> ObtenerCantidad()
  {
    throw new NotImplementedException();
  }

  public async Task<IList<PropietarioModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    throw new NotImplementedException();
  }

  public async Task<PropietarioModel?> ObtenerPorId(int id)
  {
    throw new NotImplementedException();
  }

  /* public async Task Crear(PropietarioModel propietario)
  {
    await using var conexion = _dbConnection.CreateConnection();
    await conexion.OpenAsync();
    await using var transaccion = await conexion.BeginTransactionAsync();

    try
    {
      const string insertarPersona = """
        INSERT INTO personas (apellido, nombre, dni, mail, telefono, direccion)
        VALUES (@apellido, @nombre, @dni, @mail, @telefono, @direccion);
        SELECT LAST_INSERT_ID();
        """;

      await using var comandoPersona = new MySqlCommand(insertarPersona, conexion, transaccion);
      comandoPersona.Parameters.AddWithValue("@apellido", propietario.Persona.Apellido);
      comandoPersona.Parameters.AddWithValue("@nombre", propietario.Persona.Nombre);
      comandoPersona.Parameters.AddWithValue("@dni", propietario.Persona.Dni);
      comandoPersona.Parameters.AddWithValue("@telefono", propietario.Persona.Telefono);
      comandoPersona.Parameters.AddWithValue("@mail", propietario.Persona.Mail);
      comandoPersona.Parameters.AddWithValue("@direccion", propietario.Persona.Direccion);
      propietario.PersonaId = Convert.ToInt32(await comandoPersona.ExecuteScalarAsync());

      const string insertarPropietario = """
        INSERT INTO propietarios (id_persona, cbu, cuit, estado)
        VALUES (@id_persona, @cbu, @cuit, @estado);
        """;

      await using var comandoPropietario = new MySqlCommand(insertarPropietario, conexion, transaccion);
      comandoPropietario.Parameters.AddWithValue("@id_persona", propietario.PersonaId);
      comandoPropietario.Parameters.AddWithValue("@cbu", propietario.Cbu);
      comandoPropietario.Parameters.AddWithValue("@cuit", propietario.Cuit);
      comandoPropietario.Parameters.AddWithValue("@estado", true);
      await comandoPropietario.ExecuteNonQueryAsync();

      await transaccion.CommitAsync();
    }
    catch
    {
      await transaccion.RollbackAsync();
      throw;
    }
  } */
}