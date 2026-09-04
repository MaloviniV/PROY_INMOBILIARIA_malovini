using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;

public class InmuebleService : IInmuebleService
{
  private readonly IInmuebleRepository _repository;
  public InmuebleService(IInmuebleRepository repository) => _repository = repository;
  public Task<bool> Crear(InmuebleModel model) => CrearInterno(model);
  public Task<bool> Modificar(InmuebleModel model) => _repository.Modificar(model);
  public Task<bool> Eliminar(int id) => _repository.Eliminar(id);
  public Task<InmuebleModel?> BuscarPorId(int id) => _repository.ObtenerPorId(id);
  public Task<IList<InmuebleModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10) => _repository.ObtenerLista(paginaNro, tamPagina);
  public Task<int> ObtenerCantidad() => _repository.ObtenerCantidad();
  private async Task<bool> CrearInterno(InmuebleModel model) => await _repository.Crear(model) > 0;
}
