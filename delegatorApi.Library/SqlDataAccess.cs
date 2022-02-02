using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace delegatorApi.Library
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public List<T> LoadData<T, U>(string storedProcedure, U parametrs, string connectionStringName)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                List<T> rows = cnn.Query<T>(storedProcedure, parametrs, 
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parametrs, string connectionStringName)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                cnn.Execute(storedProcedure, parametrs,
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}
