using Microsoft.Data.Sqlite;

public interface ISqlLiteProvider
{
    SqliteConnection GetConnection();
}