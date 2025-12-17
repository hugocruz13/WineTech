using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            if (string.IsNullOrWhiteSpace(utilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(utilizadoresId));

            var itens = await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadoresId);

            return itens ?? new List<Carrinho>();
        }

        public async Task<List<Carrinho>> InserirItem(Models.Carrinho itemCarrinho)
        {
            if (itemCarrinho == null)
                throw new ArgumentNullException(nameof(itemCarrinho));

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(itemCarrinho.UtilizadoresId));

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.", nameof(itemCarrinho.VinhosId));

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade tem de ser maior que 0.", nameof(itemCarrinho.Quantidade));

            return await AtualizarItem(itemCarrinho);
        }
        public async Task<List<Carrinho>> AtualizarItem(Models.Carrinho itemCarrinho)
        {
            if (itemCarrinho == null)
                throw new ArgumentException("Dados inválidos para a atualização do carrinho.");

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.");

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.");

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade a adicionar inválida.");

            List<Carrinho> carrinhoAtual = await _carrinhoDAL.ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId) ?? new List<Carrinho>();

            var quantidadeJaNoCarrinho = carrinhoAtual.Where(c => c.VinhosId == itemCarrinho.VinhosId).Sum(c => c.Quantidade);

            var quantidadePretendida = quantidadeJaNoCarrinho + itemCarrinho.Quantidade;

            var resumo = await _adegaDAL.ObterResumoStockTotal();

            if (resumo == null)
                throw new Exception("Não foi possível obter o resumo de stock para validação.");

            var stockVinho = resumo.FirstOrDefault(s => s.VinhoId == itemCarrinho.VinhosId);
            if (quantidadePretendida > stockVinho.Quantidade)
                throw new ArgumentException("Stock insuficiente.");

            var carrinhoFinal = new Models.Carrinho
            {
                VinhosId = itemCarrinho.VinhosId,
                UtilizadoresId = itemCarrinho.UtilizadoresId,
                Quantidade = quantidadePretendida
            };
            return await _carrinhoDAL.AtualizarItem(carrinhoFinal);
        }
        public async Task<bool> EliminarItem(int vinhoId, string utilizadoresId)
        {
            if (vinhoId <= 0)
                throw new ArgumentException("ID inválido.");
            return await _carrinhoDAL.EliminarItem(vinhoId, utilizadoresId);
        }

        public async Task<List<CarrinhoDetalhe>> ObterDetalhesCarrinho(string utilizadoresId)
        {
            if (string.IsNullOrWhiteSpace(utilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(utilizadoresId));
            return await _carrinhoDAL.ObterDetalhesCarrinho(utilizadoresId);
        }
    }
}
