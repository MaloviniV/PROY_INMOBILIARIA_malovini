namespace PROY_INMOBILIARIA_malovini.Services;

public interface IServiceBase<T>
{
  Task Crear(T p);
  Task Eliminar(int id);
  Task Modificar(T p);
  Task<T?> ObtenerPorId(int id);
  Task<IList<T>> ObtenerLista(int paginaNro = 1, int tamPagina = 10);
  Task<int> ObtenerCantidad();
}