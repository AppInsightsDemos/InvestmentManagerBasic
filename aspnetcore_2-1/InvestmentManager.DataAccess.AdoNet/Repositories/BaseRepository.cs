using System;
using System.Data;
using System.Data.SqlClient;


namespace InvestmentManager.DataAccess.AdoNet.Repositories
{
    internal class BaseRepository 
    {

        public BaseRepository(String connectionString)
        {
            _connectionString = connectionString;
        }


        private String _connectionString;



        protected IDbConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            return sqlConnection;
        }

    }
}
