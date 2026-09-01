using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PersonaRepository : IPersonaRepository
{
  private readonly IUnitOfWork _uow;

  public PersonaRepository(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public async Task<int> Crear(PersonaModel p)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"INSERT INTO personas (nombre, apellido, dni, telefono, mail, direccion)
                          VALUES (@nombre, @apellido, @dni, @telefono, @mail, @direccion);
                          SELECT LAST_INSERT_ID();";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@nombre", p.Nombre);
    command.Parameters.AddWithValue("@apellido", p.Apellido);
    command.Parameters.AddWithValue("@dni", p.Dni);
    command.Parameters.AddWithValue("@telefono", p.Telefono);
    command.Parameters.AddWithValue("@mail", p.Mail);
    command.Parameters.AddWithValue("@direccion", p.Direccion);

    var result = await command.ExecuteScalarAsync();
    return Convert.ToInt32(result);
  }

  public async Task<bool> Modificar(PersonaModel p)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    var sql = @"UPDATE personas
                SET nombre=@Nombre, apellido=@Apellido, dni=@Dni, telefono=@Telefono, mail=@Mail, direccion=@Direccion
                WHERE id = @Id";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@Id", p.Id);
    command.Parameters.AddWithValue("@Nombre", p.Nombre);
    command.Parameters.AddWithValue("@Apellido", p.Apellido);
    command.Parameters.AddWithValue("@Dni", p.Dni);
    command.Parameters.AddWithValue("@Telefono", p.Telefono);
    command.Parameters.AddWithValue("@Mail", p.Mail);
    command.Parameters.AddWithValue("@Direccion", p.Direccion);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<bool> Eliminar(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"DELETE FROM personas 
                        WHERE id = @id;";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    command.Parameters.AddWithValue("@id", id);

    return await command.ExecuteNonQueryAsync() >= 1;
  }

  public async Task<PersonaModel?> ObtenerPorId(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT *
                        FROM personas
                        WHERE id=@id";

    await using var comandoPersona = new MySqlCommand(sql, conexion, transaccion);
    comandoPersona.Parameters.AddWithValue("@id", id);

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
    ;
    return null;
  }

  public async Task<PersonaModel?> ObtenerPorDni(string dni)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT id, apellido, nombre, dni, mail, telefono, direccion 
                        FROM personas
                        WHERE dni=@dni";

    await using var comandoPersona = new MySqlCommand(sql, conexion, transaccion);

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

  public async Task<PersonaModel?> ObtenerPorMail(string mail)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT id, apellido, nombre, dni, mail, telefono, direccion 
                        FROM personas
                        WHERE mail=@mail";

    await using var comandoPersona = new MySqlCommand(sql, conexion, transaccion);

    comandoPersona.Parameters.AddWithValue("@mail", mail);

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

  public async Task<IList<PersonaModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT id, nombre, apellido, dni, mail, telefono, direccion
                        FROM personas
                        ORDER BY id
                        LIMIT @limit OFFSET @offset";

    int offset = (paginaNro - 1) * tamPagina;

    await using var command = new MySqlCommand(sql, conexion, transaccion);
    command.Parameters.AddWithValue("@limit", tamPagina);
    command.Parameters.AddWithValue("@offset", offset);

    var lista = new List<PersonaModel>();

    await using var reader = await command.ExecuteReaderAsync();

    while (reader.Read())
    {
      lista.Add(new PersonaModel
      {
        Id = reader.GetInt32("id"),
        Nombre = reader.GetString("nombre"),
        Apellido = reader.GetString("apellido"),
        Dni = reader.GetString("dni"),
        Mail = reader.GetString("mail"),
        Telefono = reader.GetString("telefono"),
        Direccion = reader.GetString("direccion"),
      });
    }

    return lista;
  }

  public async Task<int> ObtenerCantidad()
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;
    const string sql = @"SELECT COUNT(*) as cantidad FROM personas";

    await using var command = new MySqlCommand(sql, conexion, transaccion);

    await using var reader = await command.ExecuteReaderAsync();

    if (reader.Read())
    {
      return reader.GetInt32("cantidad");
    }

    return 0;
  }
}