using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLibrary
{
    public class SqlDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString) 
        {
            using (IDbConnection connection = new SqlConnection(connectionString)) 
            {
                // Dapper (IDbConnection.Query<T>) execute sql statement with given parameters
                // maps each row to object of type T and create IEnumerable
                // at the end we convert it to List<T>

                return connection.Query<T>(sqlStatement, parameters).ToList();
            }
        }

        public void SaveData<U>(string sqlStatement, U parameters, string connectionString) 
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }
    }
}
