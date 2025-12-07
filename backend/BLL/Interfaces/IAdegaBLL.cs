using Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAdegaBLL
    {
        Task<int> InserirAdega(string localizacao);
        Task<List<Adega>> TodasAdegas();
        Task<Adega> AdegaById(int id);
        Task<bool> ModificarAdega(Adega adega);
        Task<bool> ApagarAdega(int id);

    }
}
