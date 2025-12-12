using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CompraBLL : ICompraBLL
    {
        private readonly ICompraDAL _compraDAL;
        private readonly IVinhoDAL _vinhoDAL;

        public CompraBLL(ICompraDAL compraDAL, IVinhoDAL vinhoDAL)
        {
            _compraDAL = compraDAL;
            _vinhoDAL = vinhoDAL;
        }

        public async Task<bool> ProcessarCarrinho(string utilizadorId)
        {
            //Buscar carrinho pessoal do utilizador
            //carrinhos = await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadorId);

            //if (carrinhos == null || !carrinhos.Any())
            //    return false;

            //var compra = await _compraDAL.CriarCompra(new Compra { UtilizadorId = utilizadorId });
            //if (compra == null)
            //    return false;

            //double total = 0;

            //foreach (var item in carrinhos)
            //{
            //    List<int> stockIds = await _compraDAL.ObterStockIds(new StockInput { VinhoId = item.VinhosId, Quantidade = item.Quantidade });

            //    Vinho vinho = await _vinhoDAL.VinhoById(item.VinhosId);

            //    if (vinho == null || stockIds == null || stockIds.Count == 0)
            //        return false;

            //    if (stockIds.Count < item.Quantidade)
            //        return false;

            //    total += vinho.Preco * item.Quantidade;

            //    LinhaCompra linha = new LinhaCompra { CompraId = compra.Id, stockIds = stockIds };

            //    bool criada = await _compraDAL.CriarLinha(linha);
            //    if (!criada)
            //        return false;

            //    bool finalizada = await _compraDAL.FinalizarCompra(linha);
            //    if (!finalizada)
            //        return false;
            //}

            //compra.ValorTotal = total;
            //bool atualizou = await _compraDAL.AtualizarValorTotal(compra);
            //if (!atualizou)
            //    return false;

            return true;
        }
    }
}
