using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AdegaBLL : IAdegaBLL
    {
        private readonly IAdegaDAL _adegaDAL;
        private readonly IVinhoDAL _vinhoDAL;

        public AdegaBLL(IAdegaDAL adegaDAL, IVinhoDAL vinhoDAL)
        {
            _adegaDAL = adegaDAL;
            _vinhoDAL = vinhoDAL;
        }

        public async Task<Adega> InserirAdega(Adega adega)
        {
            if (adega == null)
                throw new ArgumentException("Adega não pode ser nula");

            if (adega.Id != 0)
                throw new ArgumentException("O ID deve ser zero para uma nova adega.");

            if (string.IsNullOrWhiteSpace(adega.Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(adega.Localizacao))
                throw new ArgumentException("A localização é obrigatória.");

            if (adega.Capacidade <= 0)
                throw new ArgumentException("A capacidade deve ser maior que zero.");

            return await _adegaDAL.InserirAdega(adega);
        }

        public async Task<List<Adega>> TodasAdegas()
        {
            return await _adegaDAL.TodasAdegas();
        }

        public async Task<Adega> AdegaById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var adega = await _adegaDAL.AdegaById(id);
            if (adega == null)
                throw new KeyNotFoundException($"Adega {id} não encontrada.");

            return adega;
        }

        public async Task<Adega> ModificarAdega(Adega adega)
        {
            if (adega == null || adega.Id <= 0)
                throw new ArgumentException("Dados inválidos.");

            return await _adegaDAL.ModificarAdega(adega);
        }

        public async Task<bool> ApagarAdega(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            return await _adegaDAL.ApagarAdega(id);
        }

        public async Task<List<StockResumo>> ObterResumoPorAdega(int adegaId)
        {
            if (adegaId <= 0)
                throw new ArgumentException("ID inválido.");

            if (await _adegaDAL.AdegaById(adegaId) == null)
                throw new KeyNotFoundException($"Adega {adegaId} não encontrada.");

            return await _adegaDAL.ObterResumoPorAdega(adegaId);
        }

        public async Task<bool> AdicionarStock(StockInput stock)
        {
            if (stock == null)
                throw new ArgumentException("Stock não pode ser nulo");

            if (stock.Quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            Adega adega = await _adegaDAL.AdegaById(stock.AdegaId);

            if (adega == null)
                throw new KeyNotFoundException($"Adega {stock.AdegaId} não encontrada.");

            if (await _adegaDAL.ObterCapacidadeAtual(stock.AdegaId) + stock.Quantidade > adega.Capacidade)
                throw new InvalidOperationException("Capacidade da adega excedida.");

            if (await _vinhoDAL.VinhoById(stock.VinhoId) == null)
                throw new KeyNotFoundException($"Vinho {stock.VinhoId} não encontrado.");

            return await _adegaDAL.AdicionarStock(stock);
        }

        public async Task<bool> AtualizarStock(StockInput stock)
        {
            if (stock == null)
                throw new ArgumentException("Stock não pode ser nulo");

            if (stock.Quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            Adega adega = await _adegaDAL.AdegaById(stock.AdegaId);

            if (adega == null)
                throw new KeyNotFoundException($"Adega {stock.AdegaId} não encontrada.");

            Vinho vinho = await _vinhoDAL.VinhoById(stock.VinhoId);

            if (vinho == null)
                throw new KeyNotFoundException($"Vinho {stock.VinhoId} não encontrado.");

            int ocupacaoTotal = await _adegaDAL.ObterCapacidadeAtual(stock.AdegaId);
            int quantidadeAtual = _adegaDAL.ObterResumoPorAdega(stock.AdegaId).Result
                                        .Find(s => s.VinhoId == stock.VinhoId)?.Quantidade ?? 0;
            int diferenca = stock.Quantidade - quantidadeAtual;

            if (ocupacaoTotal + diferenca > adega.Capacidade)
            {
                int espacoLivre = adega.Capacidade - ocupacaoTotal;
                throw new InvalidOperationException($"Capacidade excedida. Só podes adicionar mais {espacoLivre} garrafas.");
            }

            return await _adegaDAL.AtualizarStock(stock);
        }

        public async Task<List<StockResumo>> ObterResumoStockTotal()
        {
            return await _adegaDAL.ObterResumoStockTotal();
        }

        public async Task<bool> ApagarStock(int vinhoId)
        {
            if (vinhoId <= 0)
                throw new ArgumentException("ID inválido.");
            if (await _vinhoDAL.VinhoById(vinhoId) == null)
                throw new KeyNotFoundException($"Vinho {vinhoId} não encontrado.");

            return await _adegaDAL.ApagarStock(vinhoId);
        }
    }
}
