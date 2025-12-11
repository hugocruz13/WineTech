using Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUtilizadorBLL
    {
        Task<int> RegisterUserAsync(string accessToken);
    }
}
