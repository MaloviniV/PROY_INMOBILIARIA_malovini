using MySqlConnector;

namespace PROY_INMOBILIARIA_malovini.Data;

public interface IUnitOfWork : IAsyncDisposable
{
  Task<MySqlConnection> Connection();
  MySqlTransaction? Transaction {get;}
  Task TransactionAsync();
  Task CommitAsync();
  Task RollbackAsync();
}