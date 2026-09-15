using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;

public class ClienteService : IClienteService
{
  private readonly IPersonaRepository _personaRepositorio;
  private readonly IClienteRepository _clienteRepositorio;
  private readonly IUnitOfWork _uow;

  public ClienteService(IPersonaRepository persRep, IClienteRepository clienteRep, IUnitOfWork uow)
  {
    _personaRepositorio = persRep;
    _clienteRepositorio = clienteRep;
    _uow = uow;
  }

  public Task<IList<ClienteViewModel>> ObtenerClientes(
    int paginaNro = 1,
    int tamPagina = 10)
  {
    return _clienteRepositorio.ObtenerLista(paginaNro, tamPagina);
  }

  public Task<PersonaModel?> BuscarPorDni(string dni)
  {
    return _personaRepositorio.ObtenerPorDni(dni);
  }

  public async Task<bool> Modificar(PersonaModel persona)
  {
    try
    {
      await _uow.TransactionAsync();
      var modificado = await _personaRepositorio.Modificar(persona);

      if (!modificado)
      {
        await _uow.RollbackAsync();
        return false;
      }

      await _uow.CommitAsync();
      return true;
    }
    catch
    {
      await _uow.RollbackAsync();
      throw;
    }
  }
}