using System.Data;
using MySqlConnector;

namespace PROY_INMOBILIARIA_malovini.Data;

public class UnitOfWork : IUnitOfWork
{
  private readonly DbConnection _dbConnection;
  private MySqlConnection? _connection;
  private MySqlTransaction? _transaction;

  public UnitOfWork(DbConnection conn)
  {
    _dbConnection = conn;
  }
  public MySqlTransaction? Transaction
  {
    get
    {
      return _transaction;
    }
  }
  public async Task<MySqlConnection> Connection()
  {
    if (_connection is null)
    {
      _connection = _dbConnection.CreateConnection();
      await _connection.OpenAsync();
    }
    return _connection;
  }

  public async Task TransactionAsync()
  {
    if (_transaction is not null)
      throw new InvalidOperationException("Ya existe una transacción activa.");

    var connection = await Connection();
    _transaction = await connection.BeginTransactionAsync();
  }

  public async Task CommitAsync()
  {
    if (_transaction is null)
      return;

    await _transaction.CommitAsync();
    await _transaction.DisposeAsync();

    _transaction = null;
  }

  public async Task RollbackAsync()
  {
    if (_transaction is null)
      return;

    await _transaction.RollbackAsync();
    await _transaction.DisposeAsync();

    _transaction = null;
  }

  public async ValueTask DisposeAsync()
  {
    if (_transaction is not null)
      await _transaction.DisposeAsync();

    if (_connection is not null)
      await _connection.DisposeAsync();
  }
}