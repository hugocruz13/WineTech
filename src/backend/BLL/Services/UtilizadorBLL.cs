using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
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

        public async Task<int> RegisterUserAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Access token not found.");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var json = await client.GetStringAsync("https://dev-pph3mb8b0az7n35a.eu.auth0.com/userinfo");

                using (var doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("sub", out var subProp))
                        throw new Exception("User ID claim not found.");

                    Utilizador utilizador = new Utilizador
                    {
                        Auth0UserId = subProp.GetString(),
                        Nome = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : "",
                        Email = root.TryGetProperty("email", out var emailProp) ? emailProp.GetString() : "",
                        ImgUrl = root.TryGetProperty("picture", out var picProp) ? picProp.GetString() : ""
                    };

                    return await _dal.AddUserAsync(utilizador);
                }
            }
        }
    }
}
