using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdegaDAL
    {
        Task<int> InserirAdega(string localizacao);
        List<Adega> TodasAdegas();                
        Adega AdegaById(int id);               
        bool ModificarAdega(Adega adega);        
        bool ApagarAdega(int id);
    }
}
