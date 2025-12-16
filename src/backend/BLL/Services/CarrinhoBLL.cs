using DAL.Interfaces;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CarrinhoBLL : ICarrinhoBLL
    {
        private readonly ICarrinhoDAL _carrinhoDAL;

        public CarrinhoBLL(ICarrinhoDAL carrinhoDAL)
        {
            _carrinhoDAL = carrinhoDAL;
        }

        public async Task<List<Models.Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            return await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadoresId);
        }
        public async Task<List<Models.Carrinho>> InserirItem(Models.Carrinho itemCarrinho)
        {
            return await _carrinhoDAL.InserirItem(itemCarrinho);
        }
    }
}
