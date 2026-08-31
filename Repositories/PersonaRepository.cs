using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class PersonaRepository : IPersonaRepository
{
  private readonly IUnitOfWork _uow;

  public PersonaRepository(IUnitOfWork uow)
  {
    _uow=uow;
  }

  public Task<bool> Crear(PersonaModel p)
  {
    throw new NotImplementedException();
  }

  public Task<int> Eliminar(int id)
  {
    throw new NotImplementedException();
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

  public async Task<PersonaModel?> ObtenerPorId(int id)
  {
    var conexion = await _uow.Connection();
    var transaccion = _uow.Transaction;

    const string sql = @"SELECT *
                        FROM personas
                        WHERE id=@id";

    await using var comandoPersona = new MySqlCommand(sql,conexion, transaccion);
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
    };
    return null;
  }

  public Task<PersonaModel?> ObtenerPorMail(string mail)
  {
    throw new NotImplementedException();
  }
}