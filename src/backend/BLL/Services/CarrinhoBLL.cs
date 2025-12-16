using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CarrinhoBLL : ICarrinhoBLL
    {
        private readonly ICarrinhoDAL _carrinhoDAL;
        private readonly IAdegaDAL _adegaDAL;

        public CarrinhoBLL(ICarrinhoDAL carrinhoDAL, IAdegaDAL adegaDAL)
        {
            _carrinhoDAL = carrinhoDAL;
            _adegaDAL = adegaDAL;
        }

        public async Task<List<Models.Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            if (string.IsNullOrWhiteSpace(utilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(utilizadoresId));

            var itens = await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadoresId);

            return itens ?? new List<Models.Carrinho>();
        }

        public async Task<List<Models.Carrinho>> InserirItem(Models.Carrinho itemCarrinho)
        {
            if (itemCarrinho == null)
                throw new ArgumentNullException(nameof(itemCarrinho));

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(itemCarrinho.UtilizadoresId));

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.", nameof(itemCarrinho.VinhosId));

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade tem de ser maior que 0.", nameof(itemCarrinho.Quantidade));

            List<Models.Carrinho> carrinho = await _carrinhoDAL.ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId) ?? new List<Models.Carrinho>();

            var quantidadeJaNoCarrinho = carrinho.Where(x => x.VinhosId == itemCarrinho.VinhosId).Sum(x => x.Quantidade);
            var quantidadePretendida = quantidadeJaNoCarrinho + itemCarrinho.Quantidade;

            var resumo = await _adegaDAL.ObterResumoStockTotal();

            if (resumo == null)
                throw new Exception("Não foi possível obter o resumo de stock.");

            var stockVinho = resumo.FirstOrDefault(s => s.VinhoId == itemCarrinho.VinhosId);

            if (stockVinho == null)
                throw new ArgumentException("O vinho indicado não existe em stock.", nameof(itemCarrinho.VinhosId));

            if (quantidadePretendida > stockVinho.Quantidade)
                throw new ArgumentException("Stock insuficiente.");

            return await _carrinhoDAL.InserirItem(itemCarrinho);
        }
    }
}
