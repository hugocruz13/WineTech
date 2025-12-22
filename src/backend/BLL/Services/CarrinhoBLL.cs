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
        private readonly INotificacaoBLL _notificacaoBLL;

        public CarrinhoBLL(ICarrinhoDAL carrinhoDAL, IAdegaDAL adegaDAL, INotificacaoBLL notificacaoBLL)
        {
            _carrinhoDAL = carrinhoDAL;
            _adegaDAL = adegaDAL;
            _notificacaoBLL = notificacaoBLL;
        }

        public async Task<List<Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            if (string.IsNullOrWhiteSpace(utilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(utilizadoresId));

            var itens = await _carrinhoDAL.ObterCarrinhoPorUtilizador(utilizadoresId);

            return itens ?? new List<Carrinho>();
        }

        public async Task<List<Carrinho>> InserirItem(Carrinho itemCarrinho, string utilizadoresId)
        {
            if (itemCarrinho == null)
                throw new ArgumentNullException(nameof(itemCarrinho));

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.", nameof(itemCarrinho.UtilizadoresId));

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.", nameof(itemCarrinho.VinhosId));

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade tem de ser maior que 0.", nameof(itemCarrinho.Quantidade));

            var resumo = await _adegaDAL.ObterResumoStockTotal();

            if (resumo == null)
                throw new Exception("Não foi possível obter o resumo de stock para validação.");

            var stockVinho = resumo.FirstOrDefault(s => s.VinhoId == itemCarrinho.VinhosId);

            if (stockVinho == null)
            {
                await _notificacaoBLL.InserirNotificacao(new Notificacao { Titulo = "Vinho indisponível", Mensagem = "Este vinho não se encontra disponível em stock.", Tipo = TipoNotificacao.Informacao, UtilizadorId = utilizadoresId });
                throw new ArgumentException("Vinho sem stock.");
            }

            List<Carrinho> carrinhoAtual = await _carrinhoDAL.ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);

            var quantidadeJaNoCarrinho = carrinhoAtual.Where(c => c.VinhosId == itemCarrinho.VinhosId).Sum(c => c.Quantidade);

            var quantidadePretendida = quantidadeJaNoCarrinho + itemCarrinho.Quantidade;

            if (quantidadePretendida > stockVinho.Quantidade)
            {
                await _notificacaoBLL.InserirNotificacao(new Notificacao { Titulo = "Stock insuficiente", Mensagem = $"Apenas temos {stockVinho.Quantidade} garrafa(s) disponível(is) em stock para este vinho.", Tipo = TipoNotificacao.Informacao, UtilizadorId = utilizadoresId });
                throw new ArgumentException("Stock insuficiente.");
            }

            var carrinhoFinal = new Carrinho
            {
                VinhosId = itemCarrinho.VinhosId,
                UtilizadoresId = itemCarrinho.UtilizadoresId,
                Quantidade = quantidadePretendida
            };

            return await _carrinhoDAL.InserirItem(carrinhoFinal);
        }



        public async Task<List<Carrinho>> AumentarItemCarrinho(Carrinho itemCarrinho, string userid)
        {
            if (itemCarrinho == null)
                throw new ArgumentException("Dados inválidos para a atualização do carrinho.");

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.");

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.");

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade a adicionar inválida.");

            List<Carrinho> carrinhoAtual = await _carrinhoDAL.ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);

            var quantidadeJaNoCarrinho = carrinhoAtual.Where(c => c.VinhosId == itemCarrinho.VinhosId).Sum(c => c.Quantidade);

            var quantidadePretendida = quantidadeJaNoCarrinho + itemCarrinho.Quantidade;

            var resumo = await _adegaDAL.ObterResumoStockTotal();

            if (resumo == null)
                throw new Exception("Não foi possível obter o resumo de stock para validação.");

            var stockVinho = resumo.FirstOrDefault(s => s.VinhoId == itemCarrinho.VinhosId);

            if (stockVinho == null)
            {
                await _notificacaoBLL.InserirNotificacao(new Notificacao { Titulo = "Vinho indisponível", Mensagem = "Este vinho não se encontra disponível em stock.", Tipo = TipoNotificacao.Informacao, UtilizadorId = userid });
                throw new ArgumentException("Vinho sem stock.");
            }

            if (quantidadePretendida > stockVinho.Quantidade)
            {
                await _notificacaoBLL.InserirNotificacao(new Notificacao { Titulo = "Stock insuficiente", Mensagem = $"Apenas temos {stockVinho.Quantidade} garrafa(s) disponível(is) em stock para este vinho.", Tipo = TipoNotificacao.Informacao, UtilizadorId = userid });
                throw new ArgumentException("Stock insuficiente.");
            }

            var carrinhoFinal = new Carrinho
            {
                VinhosId = itemCarrinho.VinhosId,
                UtilizadoresId = itemCarrinho.UtilizadoresId,
                Quantidade = quantidadePretendida
            };
            return await _carrinhoDAL.AtualizarItem(carrinhoFinal);
        }

        public async Task<List<Carrinho>> DiminuirItemCarrinho(Carrinho itemCarrinho, string userid)
        {
            if (itemCarrinho == null)
                throw new ArgumentException("Dados inválidos para a atualização do carrinho.");

            if (string.IsNullOrWhiteSpace(itemCarrinho.UtilizadoresId))
                throw new ArgumentException("Utilizador inválido.");

            if (itemCarrinho.VinhosId <= 0)
                throw new ArgumentException("Vinho inválido.");

            if (itemCarrinho.Quantidade <= 0)
                throw new ArgumentException("Quantidade a diminuir inválida.");

            List<Carrinho> carrinhoAtual = await _carrinhoDAL.ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);

            var itemExistente = carrinhoAtual.FirstOrDefault(c => c.VinhosId == itemCarrinho.VinhosId);

            if (itemExistente == null)
                throw new ArgumentException("O vinho não existe no carrinho.");

            var quantidadeFinal = itemExistente.Quantidade - itemCarrinho.Quantidade;

            if (quantidadeFinal <= 0)
            {
                throw new ArgumentException("A quantidade não pode ser inferior a 1.");
            }


            var carrinhoFinal = new Models.Carrinho
            {
                VinhosId = itemCarrinho.VinhosId,
                UtilizadoresId = itemCarrinho.UtilizadoresId,
                Quantidade = quantidadeFinal
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
