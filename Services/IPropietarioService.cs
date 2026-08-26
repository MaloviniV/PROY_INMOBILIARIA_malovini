using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Services;
public interface IPropietarioService : IServiceBase<PropietarioModel>
{
	Task<(PersonaModel? persona, PropietarioModel? propietario)> BuscarPorDni(string dni);
}