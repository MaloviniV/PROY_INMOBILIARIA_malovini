using MySqlConnector;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;
namespace PROY_INMOBILIARIA_malovini.Repositories;

public class ReservaRepository : IReservaRepository
{
  private readonly IUnitOfWork _uow;
  public ReservaRepository(IUnitOfWork uow) => _uow = uow;
  public async Task<int> Crear(ReservaModel m) { var c = await _uow.Connection(); const string s = "INSERT INTO reservas (id_inquilino,id_inmueble,fecha_desde,fecha_hasta,monto) VALUES (@inq,@inm,@desde,@hasta,@monto); SELECT LAST_INSERT_ID();"; await using var x = new MySqlCommand(s, c, _uow.Transaction); Param(x, m); return Convert.ToInt32(await x.ExecuteScalarAsync()); }
  public async Task<bool> Modificar(ReservaModel m) { var c = await _uow.Connection(); const string s = "UPDATE reservas SET id_inquilino=@inq,id_inmueble=@inm,fecha_desde=@desde,fecha_hasta=@hasta,monto=@monto WHERE id=@id"; await using var x = new MySqlCommand(s, c, _uow.Transaction); x.Parameters.AddWithValue("@id", m.Id); Param(x, m); return await x.ExecuteNonQueryAsync() > 0; }
  public async Task<bool> Eliminar(int id) { var c = await _uow.Connection(); await using var x = new MySqlCommand("DELETE FROM reservas WHERE id=@id", c, _uow.Transaction); x.Parameters.AddWithValue("@id", id); return await x.ExecuteNonQueryAsync() > 0; }
  public async Task<ReservaModel?> ObtenerPorId(int id) { var c = await _uow.Connection(); const string s = "SELECT r.*,CONCAT(p.nombre,' ',p.apellido) inquilino,i.direccion direccion_inmueble FROM reservas r INNER JOIN inquilinos iq ON iq.id_persona=r.id_inquilino INNER JOIN personas p ON p.id=iq.id_persona INNER JOIN inmuebles i ON i.Id=r.id_inmueble WHERE r.id=@id"; await using var x = new MySqlCommand(s, c, _uow.Transaction); x.Parameters.AddWithValue("@id", id); await using var r = await x.ExecuteReaderAsync(); return await r.ReadAsync() ? Map(r) : null; }
  public async Task<IList<ReservaModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10) { var c = await _uow.Connection(); const string s = "SELECT r.*,CONCAT(p.nombre,' ',p.apellido) inquilino,i.direccion direccion_inmueble FROM reservas r INNER JOIN inquilinos iq ON iq.id_persona=r.id_inquilino INNER JOIN personas p ON p.id=iq.id_persona INNER JOIN inmuebles i ON i.Id=r.id_inmueble ORDER BY r.fecha_desde LIMIT @lim OFFSET @off"; await using var x = new MySqlCommand(s, c, _uow.Transaction); x.Parameters.AddWithValue("@lim", tamPagina); x.Parameters.AddWithValue("@off", (paginaNro - 1) * tamPagina); await using var r = await x.ExecuteReaderAsync(); var l = new List<ReservaModel>(); while (await r.ReadAsync()) l.Add(Map(r)); return l; }
  public async Task<int> ObtenerCantidad() { var c = await _uow.Connection(); await using var x = new MySqlCommand("SELECT COUNT(*) FROM reservas", c, _uow.Transaction); return Convert.ToInt32(await x.ExecuteScalarAsync()); }
  private static void Param(MySqlCommand x, ReservaModel m) { x.Parameters.AddWithValue("@inq", m.IdInquilino); x.Parameters.AddWithValue("@inm", m.IdInmueble); x.Parameters.AddWithValue("@desde", m.FechaDesde); x.Parameters.AddWithValue("@hasta", m.FechaHasta); x.Parameters.AddWithValue("@monto", m.Monto); }
  private static ReservaModel Map(MySqlDataReader r) => new() { Id = r.GetInt32("id"), IdInquilino = r.GetInt32("id_inquilino"), IdInmueble = r.GetInt32("id_inmueble"), FechaDesde = r.GetDateTime("fecha_desde"), FechaHasta = r.GetDateTime("fecha_hasta"), Monto = r.GetDecimal("monto"), Inquilino = r.GetString("inquilino"), DireccionInmueble = r.GetString("direccion_inmueble") };
}
