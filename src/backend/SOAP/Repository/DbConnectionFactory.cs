using System.Configuration;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class DbConnectionFactory
    {
        private readonly string _connStr;

        public DbConnectionFactory()
        {
            _connStr = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connStr);
        }
    }
}