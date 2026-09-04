using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class TipoInmuebleRepository : ITipoInmuebleRepository
{
  private readonly IUnitOfWork _uow;
  public TipoInmuebleRepository(IUnitOfWork uow) => _uow = uow;

  public async Task<int> Crear(TipoInmuebleModel model)
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("INSERT INTO tipoinmueble (nombre) VALUES (@nombre); SELECT LAST_INSERT_ID();", connection, _uow.Transaction);
    command.Parameters.AddWithValue("@nombre", model.Nombre);
    return Convert.ToInt32(await command.ExecuteScalarAsync());
  }

  public async Task<bool> Modificar(TipoInmuebleModel model)
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("UPDATE tipoinmueble SET nombre=@nombre WHERE id=@id", connection, _uow.Transaction);
    command.Parameters.AddWithValue("@id", model.Id);
    command.Parameters.AddWithValue("@nombre", model.Nombre);
    return await command.ExecuteNonQueryAsync() > 0;
  }

  public async Task<bool> Eliminar(int id)
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("DELETE FROM tipoinmueble WHERE id=@id", connection, _uow.Transaction);
    command.Parameters.AddWithValue("@id", id);
    return await command.ExecuteNonQueryAsync() > 0;
  }

  public async Task<TipoInmuebleModel?> ObtenerPorId(int id)
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("SELECT id, nombre FROM tipoinmueble WHERE id=@id", connection, _uow.Transaction);
    command.Parameters.AddWithValue("@id", id);
    await using var reader = await command.ExecuteReaderAsync();
    return await reader.ReadAsync() ? Mapear(reader) : null;
  }

  public async Task<IList<TipoInmuebleModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("SELECT id, nombre FROM tipoinmueble ORDER BY nombre LIMIT @limite OFFSET @offset", connection, _uow.Transaction);
    command.Parameters.AddWithValue("@limite", tamPagina);
    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
    await using var reader = await command.ExecuteReaderAsync();
    var lista = new List<TipoInmuebleModel>();
    while (await reader.ReadAsync()) lista.Add(Mapear(reader));
    return lista;
  }

  public async Task<int> ObtenerCantidad()
  {
    var connection = await _uow.Connection();
    await using var command = new MySqlCommand("SELECT COUNT(*) FROM tipoinmueble", connection, _uow.Transaction);
    return Convert.ToInt32(await command.ExecuteScalarAsync());
  }

  private static TipoInmuebleModel Mapear(MySqlDataReader reader) => new() { Id = reader.GetInt32("id"), Nombre = reader.GetString("nombre") };
}
