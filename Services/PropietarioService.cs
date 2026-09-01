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

  public async Task<bool> Crear(PropietarioModel p)
  {
    try
    {
      await _uow.TransactionAsync();

      var personaExistente = await _personaRepositorio.ObtenerPorDni(p.Persona.Dni);

      if (personaExistente is null)
      {
        int idPersona = await _personaRepositorio.Crear(p.Persona);
        if (idPersona == 0)
        {
          await _uow.RollbackAsync();
          return false;
        }

        p.Persona.Id = idPersona;
        p.IdPersona = idPersona;
      }
      else
      {
        p.Persona.Id = personaExistente.Id;
        p.IdPersona = personaExistente.Id;

        bool personaModificada = await _personaRepositorio.Modificar(p.Persona);
        if (!personaModificada)
        {
          await _uow.RollbackAsync();
          return false;
        }
      }

      var propietarioExistente = await _propietarioRepositorio.ObtenerPorId(p.IdPersona);

      if (propietarioExistente is null)
      {
        int idPropietario = await _propietarioRepositorio.Crear(p);
        if (idPropietario == 0)
        {
          await _uow.RollbackAsync();
          return false;
        }
      }
      else
      {
        p.IdPersona = propietarioExistente.IdPersona;

        bool propietarioModificado = await _propietarioRepositorio.Modificar(p);
        if (!propietarioModificado)
        {
          await _uow.RollbackAsync();
          return false;
        }
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

  public async Task<bool> Modificar(PropietarioModel p)
  {
    try
    {
      await _uow.TransactionAsync();

      bool persModif = await _personaRepositorio.Modificar(p.Persona);
      if (!persModif)
      {
        await _uow.RollbackAsync();
        return persModif;
      }

      bool propModif = await _propietarioRepositorio.Modificar(p);
      if (!propModif)
      {
        await _uow.RollbackAsync();
        return propModif;
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

  public async Task<bool> Eliminar(int id)
  {
    return await _propietarioRepositorio.Eliminar(id);
  }

  public async Task<PropietarioModel?> BuscarPorId(int id)
  {
    var propietario = await _propietarioRepositorio.ObtenerPorId(id);
    if (propietario is null) return null;

    var persona = await _personaRepositorio.ObtenerPorId(id);
    if (persona is null) return null;
    propietario.Persona = persona;

    return propietario;
  }

  public async Task<(PersonaModel? persona, PropietarioModel? propietario)> BuscarPorDni(string dni)
  {
    var persona = await _personaRepositorio.ObtenerPorDni(dni);

    if (persona is null) return (null, null);

    var propietario = await _propietarioRepositorio.ObtenerPorId(persona.Id);

    return (persona, propietario);
  }

  public Task<IList<PropietarioModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    return _propietarioRepositorio.ObtenerLista(paginaNro, tamPagina);
  }

  public Task<int> ObtenerCantidad()
  {
    return _propietarioRepositorio.ObtenerCantidad();
  }
}