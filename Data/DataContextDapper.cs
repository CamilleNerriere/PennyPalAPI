using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace PennyPal.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql, parameters, commandType: commandType);
        }

        public T LoadDataSingle<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql, parameters, commandType: commandType);
        }

        public bool ExecuteSql(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql, parameters, commandType: commandType) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql, parameters, commandType: commandType);
        }

    }
}