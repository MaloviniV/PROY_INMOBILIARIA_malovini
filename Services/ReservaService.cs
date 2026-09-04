using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;
namespace PROY_INMOBILIARIA_malovini.Services;

public class ReservaService : IReservaService
{
  private readonly IReservaRepository _repository;
  public ReservaService(IReservaRepository repository) => _repository = repository;
  public Task<bool> Crear(ReservaModel model) => Guardar(model, false);
  public Task<bool> Modificar(ReservaModel model) => Guardar(model, true);
  public Task<bool> Eliminar(int id) => _repository.Eliminar(id);
  public Task<ReservaModel?> BuscarPorId(int id) => _repository.ObtenerPorId(id);
  public Task<IList<ReservaModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10) => _repository.ObtenerLista(paginaNro, tamPagina);
  public Task<int> ObtenerCantidad() => _repository.ObtenerCantidad();
  private async Task<bool> Guardar(ReservaModel model, bool modificar)
  {
    if (model.FechaHasta <= model.FechaDesde) throw new ArgumentException("La fecha hasta debe ser posterior a la fecha desde.");
    return modificar ? await _repository.Modificar(model) : await _repository.Crear(model) > 0;
  }
}
