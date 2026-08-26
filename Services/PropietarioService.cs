using PROY_INMOBILIARIA_malovini.Models;
using PROY_INMOBILIARIA_malovini.Repositories;

namespace PROY_INMOBILIARIA_malovini.Services;
public class PropietarioService : IPropietarioService
{
  private readonly IPersonaRepository _personaRepositorio;
  private readonly IPropietarioRepository _propietarioRepositorio;

  public PropietarioService(IPersonaRepository persRep, IPropietarioRepository propRep)
  {
    _personaRepositorio = persRep;
    _propietarioRepositorio = propRep;
  }

  public Task Crear(PropietarioModel p)
  {
    throw new NotImplementedException();
  }

  public Task Eliminar(int id)
  {
    throw new NotImplementedException();
  }

  public Task Modificar(PropietarioModel p)
  {
    throw new NotImplementedException();
  }

  public Task<int> ObtenerCantidad()
  {
    throw new NotImplementedException();
  }

  public Task<IList<PropietarioModel>> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
  {
    throw new NotImplementedException();
  }

  public Task<PropietarioModel?> ObtenerPorId(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<(PersonaModel? persona, PropietarioModel? propietario)> BuscarPorDni(string dni)
  {
    var persona = await _personaRepositorio.ObtenerPorDni(dni);

    if(persona is null) return (null,null);

    var propietario = await _propietarioRepositorio.ObtenerPorId(persona.Id);

    return (persona, propietario);
  }
}