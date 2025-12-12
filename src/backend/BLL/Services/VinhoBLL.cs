using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class VinhoBLL : IVinhoBLL
    {
        private readonly IVinhoDAL _vinhoDAL;

        public VinhoBLL(IVinhoDAL vinhoDAL)
        {
            _vinhoDAL = vinhoDAL;
        }
        public async Task<Vinho> InserirVinho(Vinho vinho)
        {
            if (vinho == null)
                throw new ArgumentException("Vinho não pode ser nulo");

            if (vinho.Id != 0)
                throw new ArgumentException("O ID deve ser zero para um novo vinho.");

            if (string.IsNullOrWhiteSpace(vinho.Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(vinho.Tipo))
                throw new ArgumentException("O Tipo é obrigatório.");

            if (vinho.Preco <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            if (vinho.Ano <= 0)
                throw new ArgumentException("O ano não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(vinho.Descricao))
                throw new ArgumentException("A descrição é obrigatória.");

            return await _vinhoDAL.InserirVinho(vinho);
        }
        public async Task<Vinho> ModificarVinho(Vinho vinho)
        {
            if (vinho == null || vinho.Id <= 0)
                throw new ArgumentException("Dados inválidos.");

            return await _vinhoDAL.ModificarVinho(vinho);
        }
        public async Task<Vinho> VinhoById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var vinho = await _vinhoDAL.VinhoById(id);
            if (vinho == null)
                throw new KeyNotFoundException($"Vinho {id} não encontrado.");

            return vinho;
        }
        public async Task<List<Vinho>> TodosVinhos()
        {
            return await _vinhoDAL.TodosVinhos();
        }
        public async Task<bool> ApagarVinho(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            return await _vinhoDAL.ApagarVinho(id);
        }
    }
}
