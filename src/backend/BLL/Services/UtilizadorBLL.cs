using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UtilizadorBLL : IUtilizadorBLL
    {
        private readonly IUtilizadorDAL _dal;

        public UtilizadorBLL(IUtilizadorDAL dal)
        {
            _dal = dal;
        }

        public async Task<Utilizador> RegisterUserAsync(Utilizador user)
        {
            return await _dal.AddUserAsync(user);
        }

        public async Task<Utilizador> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(id));
            }
            return await _dal.GetUserByIdAsync(id);
        }
    }
}
