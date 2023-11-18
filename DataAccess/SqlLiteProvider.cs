using Microsoft.Data.Sqlite;

public class SqlLiteProvider : ISqlLiteProvider
{

    private readonly IConfiguration _config;
    public SqlLiteProvider(IConfiguration config)
    {
      _config = config;
    }

    public SqliteConnection GetConnection()
    {
      return new SqliteConnection($"Data Source={_config["DatabaseName"]}.db");
    }
}