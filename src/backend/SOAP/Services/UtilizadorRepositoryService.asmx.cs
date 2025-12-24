using SOAP.Models;
using SOAP.Repository;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace SOAP.Services
{
    /// <summary>
    /// Summary description for UtilizadorRepositoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UtilizadorRepositoryService : System.Web.Services.WebService
    {

        private readonly UtilizadorRepository _repository;

        public UtilizadorRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new UtilizadorRepository(connectionFactory);
        }

        [WebMethod]
        public Utilizador AddUser(Utilizador user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return _repository.AddUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }

        [WebMethod]
        public Utilizador GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("O ID do utilizador não pode ser nulo ou vazio", nameof(id));
            try
            {
                return _repository.GetUserById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }

        [WebMethod]
        public List<Utilizador> GetOwners()
        {
            try
            {
                return _repository.GetOwners();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }

        // Atualiza Nome, Email e ImgUrl; campos nulos não são modificados
        [WebMethod]
        public Utilizador UpdateUser(Utilizador utilizador)
        {
            if (utilizador == null)
                throw new ArgumentNullException(nameof(utilizador));

            if (string.IsNullOrEmpty(utilizador.Id))
                throw new ArgumentException("O ID do utilizador não pode ser nulo ou vazio", nameof(utilizador.Id));

            try
            {
                return _repository.UpdateUser(utilizador);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }
    }
}
