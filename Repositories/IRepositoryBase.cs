namespace PROY_INMOBILIARIA_malovini.Repositories;

public interface IRepositorioBase<T>
{
  Task<int> Crear(T p);
  Task<bool> Eliminar(int id);
  Task<bool> Modificar(T p);
  Task<T?> ObtenerPorId(int id);

  Task<IList<T>> ObtenerLista(int paginaNro = 1, int tamPagina = 10);
  Task<int> ObtenerCantidad();
}