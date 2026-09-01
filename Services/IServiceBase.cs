namespace PROY_INMOBILIARIA_malovini.Services;

public interface IServiceBase<T>
{
  Task<bool> Crear(T p);
  Task<bool> Eliminar(int id);
  Task<bool> Modificar(T p);
  Task<T?> BuscarPorId(int id);
  Task<IList<T>> ObtenerLista(int paginaNro = 1, int tamPagina = 10);
  Task<int> ObtenerCantidad();
}