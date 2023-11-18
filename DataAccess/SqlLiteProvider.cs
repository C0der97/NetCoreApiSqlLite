using Microsoft.Data.Sqlite;

namespace StudentApi.DataAccess
{
    public class SqlLiteProvider(IConfiguration config) : ISqlLiteProvider
    {

        private readonly IConfiguration _config = config;

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={_config["DatabaseName"]}.db");
        }
    }
}