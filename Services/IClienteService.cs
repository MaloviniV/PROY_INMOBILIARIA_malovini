using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Services;

public interface IClienteService
{
  Task<IList<ClienteViewModel>> ObtenerClientes(int paginaNro = 1, int tamPagina = 10);
  Task<PersonaModel?> BuscarPorDni(string dni);
  Task<bool> Modificar(PersonaModel persona);
}