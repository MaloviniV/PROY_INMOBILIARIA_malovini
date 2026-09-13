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
    const string sql = @"INSERT INTO propietarios (id_persona, cbu, cuit)
                VALUES (@id_persona, @cbu, @cuit);
                          SELECT LAST_INSERT_ID();";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", propietario.IdPersona);
    command.Parameters.AddWithValue("@cbu", propietario.Cbu);
    command.Parameters.AddWithValue("@cuit", propietario.Cuit);

    var result = await command.ExecuteScalarAsync();
    return Convert.ToInt32(result);
  }

  public async Task<bool> Modificar(PropietarioModel p)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"UPDATE propietarios
                SET cbu=@Cbu, cuit=@Cuit
                WHERE id_persona = @IdPersona";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@IdPersona", p.IdPersona);
    command.Parameters.AddWithValue("@Cbu", p.Cbu);
    command.Parameters.AddWithValue("@Cuit", p.Cuit);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<bool> Eliminar(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"DELETE FROM propietarios 
                        WHERE id_persona = @id_persona;";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", id);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<PropietarioModel?> ObtenerPorId(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT id_persona, cbu, cuit
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
      };
    }
    return null;
  }

  public async Task<IList<PropietarioModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT p.id_persona, p.cbu, p.cuit,
                              pers.id, pers.nombre, pers.apellido, pers.dni, pers.mail, pers.telefono, pers.direccion
                        FROM propietarios p
                        INNER JOIN personas pers ON p.id_persona = pers.id
                        ORDER BY p.id_persona
                        LIMIT @limit OFFSET @offset";

    int offset = (paginaNro - 1) * tamPagina;

    await using var command = new MySqlCommand(sql, conexion, transaccion);
    command.Parameters.AddWithValue("@limit", tamPagina);
    command.Parameters.AddWithValue("@offset", offset);

    var lista = new List<PropietarioModel>();

    await using var reader = await command.ExecuteReaderAsync();

    while (reader.Read())
    {
      lista.Add(new PropietarioModel
      {
        IdPersona = reader.GetInt32("id_persona"),
        Cbu = reader.GetString("cbu"),
        Cuit = reader.GetString("cuit"),
        Persona = new PersonaModel
        {
          Id = reader.GetInt32("id"),
          Nombre = reader.GetString("nombre"),
          Apellido = reader.GetString("apellido"),
          Dni = reader.GetString("dni"),
          Mail = reader.GetString("mail"),
          Telefono = reader.GetString("telefono"),
          Direccion = reader.GetString("direccion"),
        }
      });
    }

    return lista;
  }

  public async Task<int> ObtenerCantidad()
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT COUNT(*) as cantidad FROM propietarios";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    await using var reader = await command.ExecuteReaderAsync();

    if (reader.Read())
    {
      return reader.GetInt32("cantidad");
    }

    return 0;
  }
}