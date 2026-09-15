using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public class ClienteRepository : IClienteRepository
{
  private readonly IUnitOfWork _uow;

  public ClienteRepository(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public async Task<IList<ClienteViewModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    var conexion = await _uow.Connection();

    const string sql = @"SELECT p.id, p.dni, p.nombre, p.apellido,
                          EXISTS (  SELECT * FROM propietarios pr
                                    WHERE pr.id_persona = p.id) AS esPropietario,
                          EXISTS (  SELECT * FROM inquilinos i
                                    WHERE i.id_persona = p.id ) AS esInquilino
                        FROM personas p
                        ORDER BY p.id
                        LIMIT @limit OFFSET @offset
                        ";

    var offset = (paginaNro - 1) * tamPagina;
    var clientes = new List<ClienteViewModel>();

    await using var command = new MySqlCommand(sql, conexion, _uow.Transaction);
    command.Parameters.AddWithValue("@limit", tamPagina);
    command.Parameters.AddWithValue("@offset", offset);

    await using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
      clientes.Add(new ClienteViewModel
      {
        Id = reader.GetInt32("id"),
        Dni = reader.GetString("dni"),
        Nombre = reader.GetString("nombre"),
        Apellido = reader.GetString("apellido"),
        EsPropietario = reader.GetBoolean("esPropietario"),
        EsInquilino = reader.GetBoolean("esInquilino")
      });
    }

    return clientes;
  }
}