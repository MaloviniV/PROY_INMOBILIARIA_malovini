using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;

public class TipoInmuebleService : ITipoInmuebleService
{
  private readonly ITipoInmuebleRepository _repository;

  public TipoInmuebleService(ITipoInmuebleRepository repository) => _repository = repository;

  public Task<bool> Crear(TipoInmuebleModel model) => CrearInterno(model);
  public Task<bool> Modificar(TipoInmuebleModel model) => _repository.Modificar(model);
  public Task<bool> Eliminar(int id) => _repository.Eliminar(id);
  public Task<TipoInmuebleModel?> BuscarPorId(int id) => _repository.ObtenerPorId(id);
  public Task<IList<TipoInmuebleModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10) => _repository.ObtenerLista(paginaNro, tamPagina);
  public Task<int> ObtenerCantidad() => _repository.ObtenerCantidad();

  private async Task<bool> CrearInterno(TipoInmuebleModel model)
  {
    return await _repository.Crear(model) > 0;
  }
}
