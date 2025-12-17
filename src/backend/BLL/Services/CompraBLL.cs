using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CompraBLL : ICompraBLL
    {
        private readonly ICompraDAL _compraDAL;
        private readonly IVinhoDAL _vinhoDAL;
        private readonly IUtilizadorDAL _utilizadorDAL;
        private readonly ICarrinhoDAL _carrinhoDAL;
        private readonly INotificacaoBLL _notificacaoBLL;

        public CompraBLL(ICompraDAL compraDAL, IVinhoDAL vinhoDAL, IUtilizadorDAL utilizadorDAL, ICarrinhoDAL carrinhoDAL, INotificacaoBLL notificacaoBLL)
        {
            _compraDAL = compraDAL;
            _vinhoDAL = vinhoDAL;
            _utilizadorDAL = utilizadorDAL;
            _carrinhoDAL = carrinhoDAL;
            _notificacaoBLL = notificacaoBLL;
        }

        public async Task<bool> ProcessarCarrinho(string utilizadorId)
        {
            var carrinhos = await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadorId);

            if (carrinhos == null || !carrinhos.Any())
                return false;

            if (string.IsNullOrEmpty(utilizadorId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(utilizadorId));

            Utilizador utilizador = await _utilizadorDAL.GetUserByIdAsync(utilizadorId);

            if (utilizador == null)
                return false;

            Compra compra = await _compraDAL.CriarCompra(new Compra { UtilizadorId = utilizadorId });
            if (compra == null)
                return false;

            double total = 0;

            foreach (var item in carrinhos)
            {
                List<int> stockIds = await _compraDAL.ObterStockIds(new StockInput { VinhoId = item.VinhosId, Quantidade = item.Quantidade });

                Vinho vinho = await _vinhoDAL.VinhoById(item.VinhosId);

                if (vinho == null || stockIds == null || stockIds.Count == 0)
                    return false;

                if (stockIds.Count < item.Quantidade)
                    return false;

                total += vinho.Preco * item.Quantidade;

                LinhaCompra linha = new LinhaCompra { CompraId = compra.Id, stockIds = stockIds };

                bool criada = await _compraDAL.CriarLinha(linha);
                if (!criada)
                    return false;

                bool finalizada = await _compraDAL.FinalizarCompra(linha);
                if (!finalizada)
                    return false;
            }

            compra.ValorTotal = total;
            bool atualizou = await _compraDAL.AtualizarValorTotal(compra);
            if (!atualizou)
                return false;

            await _carrinhoDAL.EliminarCarrinhoPorUtilizador(utilizadorId);

            await _notificacaoBLL.InserirNotificacao(new Notificacao
            {
                Titulo = "Compra concluída com sucesso",
                Mensagem = "A sua encomenda foi processada com sucesso. Obrigado pela sua preferência.",
                Tipo = TipoNotificacao.Sucesso,
                UtilizadorId = utilizadorId
            });

            return true;
        }

        public async Task<List<Compra>> ObterComprasPorUtilizador(string utilizadorId)
        {
            if (string.IsNullOrEmpty(utilizadorId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(utilizadorId));

            Utilizador utilizador = await _utilizadorDAL.GetUserByIdAsync(utilizadorId);

            if (utilizador == null)
                return null;

            return await _compraDAL.ObterComprasUtilizador(utilizadorId);
        }

        public async Task<List<CompraDetalhe>> ObterCompraPorId(int compraId)
        {
            if (compraId <= 0)
                throw new ArgumentException("Compra ID must be greater than zero", nameof(compraId));

            return await _compraDAL.ObterDetalhesCompra(compraId);
        }
    }
}
