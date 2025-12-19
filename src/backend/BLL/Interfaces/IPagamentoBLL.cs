using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPagamentoBLL
    {
        Task<bool> ValidarCartao(string numeroCartao, int mes, int ano);
    }
}
