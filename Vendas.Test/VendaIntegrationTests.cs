using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Entities;
using Vendas.Domain;
using FluentAssertions;
using Xunit;
using Vendas.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Vendas.Test
{
    public class VendaIntegrationTests
    {
        private VendaService _vendaService;
        private VendaContext _context;
        private readonly ILogger<VendaService> _logger;

        public VendaIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<VendaContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new VendaContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _logger = Substitute.For<ILogger<VendaService>>();
            var vendaRepository = new VendaRepository(_context);
            _vendaService = new VendaService(vendaRepository, _logger);
        }

        [Fact]
        public async Task CriarVenda_DevePersistirVendaNoBancoDeDados()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 4, Cliente = "Cliente Teste", Filial = "2", ValorTotal = 10, Cancelado = false };

            // Act
            var novaVenda = await _vendaService.CriarVendaAsync(venda);

            // Assert
            var vendaNoBanco = await _context.Vendas.FindAsync(novaVenda.Id);
            vendaNoBanco.Should().NotBeNull();
            vendaNoBanco.Cliente.Should().Be("Cliente Teste");
        }

        [Fact]
        public async Task ObterVendaPorId_DeveRetornarVendaDoBancoDeDados()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 4, Cliente = "Cliente Existente", Filial = "2", ValorTotal = 10, Cancelado = false };
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();

            // Act
            var vendaObtida = await _vendaService.ObterVendaPorIdAsync(venda.Id);

            // Assert
            vendaObtida.Should().NotBeNull();
            vendaObtida.Cliente.Should().Be("Cliente Existente");
        }

        [Fact]
        public async Task AtualizarVenda_DeveAtualizarVendaNoBancoDeDados()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 3, Cliente = "Cliente Para Atualizar", Filial = "2", ValorTotal = 10 };
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();

            venda.Cliente = "Cliente Atualizado";

            // Act
            await _vendaService.AtualizarVendaAsync(venda);

            // Assert
            var vendaAtualizada = await _context.Vendas.FindAsync(venda.Id);
            vendaAtualizada.Cliente.Should().Be("Cliente Atualizado");
        }

        [Fact]
        public async Task CancelarVenda_DeveCancelarVendaNoBancoDeDados()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 4, Cliente = "Cliente Para Cancelar", Filial = "2", ValorTotal = 10, Cancelado = false };
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();

            // Act
            await _vendaService.CancelarVendaAsync(venda.Id);

            // Assert
            var vendaCancelada = await _context.Vendas.FindAsync(venda.Id);
            vendaCancelada.Cancelado.Should().BeTrue();
        }
    }
}