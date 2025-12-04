using DAL;
using System.Threading.Tasks;

namespace BLL
{
    public class UtilizadorBLL
    {
        public async Task InserteUserAsync(string auth0UserId)
        {
            var dal = new UtilizadorDAL();
            await dal.InsertUserAsync(auth0UserId);
        }
    }
}