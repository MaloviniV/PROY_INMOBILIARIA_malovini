using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;
public class PropietarioService : IPropietarioService
{
  private readonly IPersonaRepository _personaRepositorio;
  private readonly IPropietarioRepository _propietarioRepositorio;
  private readonly IUnitOfWork _uow;

  public PropietarioService(IPersonaRepository persRep, IPropietarioRepository propRep, IUnitOfWork uow)
  {
    _personaRepositorio = persRep;
    _propietarioRepositorio = propRep;
    _uow = uow;
  }

  public async Task Crear(PropietarioModel p)
  {
    try
    {
      await _uow.TransactionAsync();

      bool persCreada = await _personaRepositorio.Crear(p.Persona);
    }
    catch (System.Exception)
    {
      await _uow.RollbackAsync();
      throw;
    }
  }

  public Task Eliminar(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> Modificar(PropietarioModel p)
  {
    try
    {
    await _uow.TransactionAsync();

    bool persModif = await _personaRepositorio.Modificar(p.Persona);
    if(!persModif)
    {
      await _uow.RollbackAsync();
      return persModif;
    }

    bool propModif = await _propietarioRepositorio.Modificar(p);
    if(!propModif)
    {
      await _uow.RollbackAsync();
      return persModif;
    }
    
    await _uow.CommitAsync();
    return true;
    }
    catch (System.Exception)
    {
      await _uow.RollbackAsync();
      throw;
    }
  }

  public Task<int> ObtenerCantidad()
  {
    throw new NotImplementedException();
  }

  public Task<IList<PropietarioModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    throw new NotImplementedException();
  }

  public async Task<PropietarioModel?> BuscarPorId(int id)
  {
    var propietario = await _propietarioRepositorio.ObtenerPorId(id);
    if(propietario is null) return null;

    var persona = await _personaRepositorio.ObtenerPorId(id);
    if(persona is null) return null;
    propietario.Persona = persona;

    return propietario;
  }

  public async Task<(PersonaModel? persona, PropietarioModel? propietario)> BuscarPorDni(string dni)
  {
    var persona = await _personaRepositorio.ObtenerPorDni(dni);

    if(persona is null) return (null,null);

    var propietario = await _propietarioRepositorio.ObtenerPorId(persona.Id);

    return (persona, propietario);
  }
}