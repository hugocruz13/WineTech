using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICompraBLL
    {
        Task<bool> ProcessarCarrinho(string utilizadorId);
    }
}
