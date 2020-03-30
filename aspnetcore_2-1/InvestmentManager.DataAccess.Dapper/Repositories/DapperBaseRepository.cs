using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InvestmentManager.DataAccess.Dapper.Repositories
{
    internal class DapperBaseRepository
    {

        public DapperBaseRepository(String connectionString)
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
