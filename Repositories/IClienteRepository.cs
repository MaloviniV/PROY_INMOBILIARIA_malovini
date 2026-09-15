using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Repositories;

public interface IClienteRepository
{
  Task<IList<ClienteViewModel>> ObtenerLista(
    int paginaNro = 1,
    int tamPagina = 10);
}