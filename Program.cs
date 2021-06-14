using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Utilizar somente para testes, nunca em produção
            /*using var db = new Data.ApplicationContext();
            db.Database.Migrate();

            var existe = db.Database.GetPendingMigrations().Any();

            if (existe)
            {
                //Validar se existem migrações pendentes a serem feitas no banco de dados.
            }*/

            //Console.WriteLine("Inserindo Produto!\n");

            //InserirDados(); 
            //InserirDadosEmMassa();
            //ConsultarDados();
            //InserirPedidos();
            //ConsultarPedidosCarregamentoAdiantado();
            //AtualizarDados();
            RemoverRegistro();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            var clientes = db.Clientes.Where(c => c.Id > 0).ToList();

            foreach(var cliente in clientes)
            {
                //Vai primeiramente em memoria, se não encontrar vai na base de dados
                Console.WriteLine("Consultando Cliente: " + cliente.Nome);
                db.Clientes.Find(cliente.Id);
                //db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void ConsultarPedidosCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db
                .Pedidos
                .Include(i => i.Itens)
                .ThenInclude(t => t.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void InserirPedidos()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Primeiro Pedido",
                StatusPedido = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 20
                    }
                }    
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();

        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Almofada Preta",
                CodigoBarras = "123452456",
                Valor = 15m,
                TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Natália Rodrigues",
                CEP = "35681469",
                Cidade = "Itaúna",
                Estado = "MG",
                Telefone = "37991262371"
            };

            var cliente1 = new Cliente
            {
                Nome = "Pretinha",
                CEP = "35681469",
                Cidade = "Itaúna",
                Estado = "MG",
                Telefone = "37991262371"
            };

            using var db = new Data.ApplicationContext();

            db.AddRange(cliente, cliente1);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registros: { registros }");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Almofada Amarela",
                CodigoBarras = "123456789",
                Valor = 10m,
                TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);

            var registros = db.SaveChanges();
            
            Console.WriteLine("Total de produtos inseridos: " + registros);
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Isaac";

            /*var clienteDesconectado = new 
            {
                Nome = "Cliente Desconectado",
                Telefone = "1235455"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);*/

            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(2);
            var cliente = new Cliente { Id = 3 };
            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }
    }
}
