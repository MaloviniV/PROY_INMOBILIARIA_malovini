using System.Data;
using MySqlConnector;

namespace PROY_INMOBILIARIA_malovini.Data;

public class UnitOfWork : IUnitOfWork
{
  private readonly string _connectionString;
  private MySqlConnection? _connection;
  private MySqlTransaction? _transaction;

  public UnitOfWork(IConfiguration configuration)
  {
    _connectionString = configuration.GetConnectionString("DefaultConnection")?? throw new Exception("No se encontró la cadena de conexión.");
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
      _connection = new MySqlConnection(_connectionString);
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