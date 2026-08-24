using MySqlConnector;

namespace PROY_INMOBILIARIA_malovini.Data;

public class DbConnection
{
  private readonly string _connectionString;

  public DbConnection(IConfiguration configuration)
  {
    _connectionString = configuration.GetConnectionString("DefaultConnection");

    if (String.IsNullOrEmpty(_connectionString))
    {
      throw new Exception("Error en la cadena de conexión");
    }
  }

  public MySqlConnection CreateConnection()
  {
    return new MySqlConnection(_connectionString);
  }
}