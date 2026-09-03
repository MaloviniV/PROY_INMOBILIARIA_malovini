using PROY_INMOBILIARIA_malovini.Data;
using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;

public class InquilinoService : IInquilinoService
{
  private readonly IPersonaRepository _personaRepositorio;
  private readonly IInquilinoRepository _inquilinoRepositorio;
  private readonly IUnitOfWork _uow;

  public InquilinoService(IPersonaRepository persRep, IInquilinoRepository propRep, IUnitOfWork uow)
  {
    _personaRepositorio = persRep;
    _inquilinoRepositorio = propRep;
    _uow = uow;
  }

  public async Task<bool> Crear(InquilinoModel i)
  {
    try
    {
      await _uow.TransactionAsync();

      var personaExistente = await _personaRepositorio.ObtenerPorDni(i.Persona.Dni);

      if (personaExistente is null)
      {
        int idPersona = await _personaRepositorio.Crear(i.Persona);
        if (idPersona == 0)
        {
          await _uow.RollbackAsync();
          return false;
        }

        i.Persona.Id = idPersona;
        i.IdPersona = idPersona;
      }
      else
      {
        i.Persona.Id = personaExistente.Id;
        i.IdPersona = personaExistente.Id;

        bool personaModificada = await _personaRepositorio.Modificar(i.Persona);
        if (!personaModificada)
        {
          await _uow.RollbackAsync();
          return false;
        }
      }

      var inquilinoExistente = await _inquilinoRepositorio.ObtenerPorId(i.IdPersona);

      if (inquilinoExistente is null)
      {
        int idInquilino = await _inquilinoRepositorio.Crear(i);
        if (idInquilino == 0)
        {
          await _uow.RollbackAsync();
          return false;
        }
      }
      else
      {
        i.IdPersona = inquilinoExistente.IdPersona;

        bool inquilinoModificado = await _inquilinoRepositorio.Modificar(i);
        if (!inquilinoModificado)
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

  public async Task<bool> Modificar(InquilinoModel i)
  {
    try
    {
      await _uow.TransactionAsync();

      bool persModif = await _personaRepositorio.Modificar(i.Persona);
      if (!persModif)
      {
        await _uow.RollbackAsync();
        return persModif;
      }

      bool propModif = await _inquilinoRepositorio.Modificar(i);
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
    return await _inquilinoRepositorio.Eliminar(id);
  }

  public async Task<InquilinoModel?> BuscarPorId(int id)
  {
    var inquilino = await _inquilinoRepositorio.ObtenerPorId(id);
    if (inquilino is null) return null;

    var persona = await _personaRepositorio.ObtenerPorId(id);
    if (persona is null) return null;
    inquilino.Persona = persona;

    return inquilino;
  }

  public async Task<(PersonaModel? persona, InquilinoModel? inquilino)> BuscarPorDni(string dni)
  {
    var persona = await _personaRepositorio.ObtenerPorDni(dni);

    if (persona is null) return (null, null);

    var inquilino = await _inquilinoRepositorio.ObtenerPorId(persona.Id);

    return (persona, inquilino);
  }

  public Task<IList<InquilinoModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    return _inquilinoRepositorio.ObtenerLista(paginaNro, tamPagina);
  }

  public Task<int> ObtenerCantidad()
  {
    return _inquilinoRepositorio.ObtenerCantidad();
  }
}