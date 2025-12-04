using Models;
using SOAP.Repository;
using System;
using System.Data.SqlClient;
using System.Web.Services;

namespace SOAP.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class UtilizadorRepository : System.Web.Services.WebService
    {
        public UtilizadorRepository()
        {
        }

        [WebMethod]
        public void InsertUser(Utilizador user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                var connectionFactory = new DbConnectionFactory();

                string query = "Exec InserirUtilizador @Auth0UserId";

                using (SqlConnection connection = connectionFactory.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Auth0UserId", user.Auth0UserId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }
    }
}