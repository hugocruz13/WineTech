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

        public AdegaBLL(IAdegaDAL adegaDAL)
        {
            _adegaDAL = adegaDAL;
        }

        public async Task<int> InserirAdega(string localizacao)
        {
            if (string.IsNullOrWhiteSpace(localizacao))
                throw new ArgumentException("A localização é obrigatória.");

            return await _adegaDAL.InserirAdega(localizacao);
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

        public async Task<bool> ModificarAdega(Adega adega)
        {
            if (adega == null || adega.Id <= 0)
                throw new ArgumentException("Dados inválidos.");
            if (string.IsNullOrWhiteSpace(adega.Localizacao))
                throw new ArgumentException("Localização vazia.");

            return await _adegaDAL.ModificarAdega(adega);
        }

        public async Task<bool> ApagarAdega(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            return await _adegaDAL.ApagarAdega(id);
        }
    }
}
