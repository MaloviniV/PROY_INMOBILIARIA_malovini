using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PropietarioRepository : IPropietarioRepository
{
  private readonly IUnitOfWork _uow;

  public PropietarioRepository(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public async Task<int> Crear(PropietarioModel propietario)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;

    const string sql = @"INSERT INTO propietarios (id_persona, cbu, cuit, estado)
                          VALUES (@id_persona, @cbu, @cuit, @estado);";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", propietario.IdPersona);
    command.Parameters.AddWithValue("@cbu", propietario.Cbu);
    command.Parameters.AddWithValue("@cuit", propietario.Cuit);
    command.Parameters.AddWithValue("@estado", true);

    return await command.ExecuteNonQueryAsync();
  }

  public async Task<int> Eliminar(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;

    const string sql = @"DELETE FROM propietarios 
                        WHERE id_persona = @id_persona;";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", id);

    return await command.ExecuteNonQueryAsync();
  }

  public async Task<bool> Modificar(PropietarioModel p)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    var sql = @"UPDATE propietarios
                SET cbu=@Cbu, cuit=@Cuit
                WHERE id_persona = @IdPersona";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@IdPersona", p.IdPersona);
    command.Parameters.AddWithValue("@Cbu", p.Cbu);
    command.Parameters.AddWithValue("@Cuit", p.Cuit);

    return await command.ExecuteNonQueryAsync() >= 1;
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
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;

    const string sql = @"SELECT id_persona, cbu, cuit, estado
                        FROM propietarios
                        WHERE id_persona=@id_persona";

    await using var comandoPersona = new MySqlCommand(sql, conexion, transaccion);
    comandoPersona.Parameters.AddWithValue("@id_persona", id);

    await using var reader = await comandoPersona.ExecuteReaderAsync();

    if (reader.Read())
    {
      return new PropietarioModel
      {
        IdPersona = reader.GetInt32("id_persona"),
        Cbu = reader.GetString("cbu"),
        Cuit = reader.GetString("cuit"),
        Estado = reader.GetBoolean("estado"),
      };
    }
    return null;
  }
}