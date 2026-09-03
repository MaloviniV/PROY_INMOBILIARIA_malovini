using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class InquilinoRepository : IInquilinoRepository
{
  private readonly IUnitOfWork _uow;

  public InquilinoRepository(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public async Task<int> Crear(InquilinoModel inquilino)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"INSERT INTO inquilinos (id_persona, garante, profesion, estado)
                          VALUES (@id_persona, @garante, @profesion, @estado);
                          SELECT LAST_INSERT_ID();";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", inquilino.IdPersona);
    command.Parameters.AddWithValue("@garante", inquilino.Garante);
    command.Parameters.AddWithValue("@profesion", inquilino.Profesion);
    command.Parameters.AddWithValue("@estado", true);

    var result = await command.ExecuteScalarAsync();
    return Convert.ToInt32(result);
  }

  public async Task<bool> Modificar(InquilinoModel i)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"UPDATE inquilinos
                SET garante=@garante, profesion=@profesion
                WHERE id_persona = @id_persona";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", i.IdPersona);
    command.Parameters.AddWithValue("@garante", i.Garante);
    command.Parameters.AddWithValue("@profesion", i.Profesion);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<bool> Eliminar(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"DELETE FROM inquilinos 
                        WHERE id_persona = @id_persona;";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id_persona", id);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<InquilinoModel?> ObtenerPorId(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT id_persona, garante, profesion, estado
                        FROM inquilinos
                        WHERE id_persona=@id_persona";

    await using var comandoPersona = new MySqlCommand(sql, conexion, transaccion);
    comandoPersona.Parameters.AddWithValue("@id_persona", id);

    await using var reader = await comandoPersona.ExecuteReaderAsync();

    if (reader.Read())
    {
      return new InquilinoModel
      {
        IdPersona = reader.GetInt32("id_persona"),
        Garante = reader.GetString("garante"),
        Profesion = reader.GetString("profesion"),
        Estado = reader.GetBoolean("estado"),
      };
    }
    return null;
  }

  public async Task<IList<InquilinoModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT i.id_persona, i.garante, i.profesion, i.estado,
                              pers.id, pers.nombre, pers.apellido, pers.dni, pers.mail, pers.telefono, pers.direccion
                        FROM inquilinos i
                        INNER JOIN personas pers ON i.id_persona = pers.id
                        ORDER BY i.id_persona
                        LIMIT @limit OFFSET @offset";

    int offset = (paginaNro - 1) * tamPagina;

    await using var command = new MySqlCommand(sql, conexion, transaccion);
    command.Parameters.AddWithValue("@limit", tamPagina);
    command.Parameters.AddWithValue("@offset", offset);

    var lista = new List<InquilinoModel>();

    await using var reader = await command.ExecuteReaderAsync();

    while (reader.Read())
    {
      lista.Add(new InquilinoModel
      {
        IdPersona = reader.GetInt32("id_persona"),
        Garante = reader.GetString("garante"),
        Profesion = reader.GetString("profesion"),
        Estado = reader.GetBoolean("estado"),
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
    const string sql = @"SELECT COUNT(*) as cantidad FROM inquilinos";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    await using var reader = await command.ExecuteReaderAsync();

    if (reader.Read())
    {
      return reader.GetInt32("cantidad");
    }

    return 0;
  }
}