using NSubstitute;
using Vendas.Domain;
using Vendas.Domain.Entities;
using Vendas.Domain.Interfaces.Repository;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Vendas.Test
{
    public class VendaServiceTests
    {
        private readonly VendaService _vendaService;
        private readonly IVendaRepository _vendaRepository;
        private readonly ILogger<VendaService> _logger;

        public VendaServiceTests()
        {
            _logger = Substitute.For<ILogger<VendaService>>();
            _vendaRepository = Substitute.For<IVendaRepository>();
            _vendaService = new VendaService(_vendaRepository, _logger);
        }

        [Fact]
        public async Task CriarVenda_DeveRetornarVendaCriada()
        {
            // Arrange
            var venda = new Venda { NumeroVenda = 1, Cliente = "Cliente Teste", Itens = new List<ItemVenda>() };

            // Act
            var result = await _vendaService.CriarVendaAsync(venda);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.DataVenda.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(1));
            result.Cancelado.Should().BeFalse();
            await _vendaRepository.Received(1).AddAsync(Arg.Any<Venda>());
        }

        [Fact]
        public async Task AtualizarVenda_DeveAtualizarVendaExistente()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 1, Cliente = "Cliente Teste" };

            // Act
            var result = await _vendaService.AtualizarVendaAsync(venda);

            // Assert
            await _vendaRepository.Received(1).UpdateAsync(venda);
            result.Should().BeSameAs(venda);
        }

        [Fact]
        public async Task CancelarVenda_DeveCancelarVendaExistente()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 1, Cliente = "Cliente Teste", Cancelado = false };
            _vendaRepository.GetByIdAsync(venda.Id).Returns(venda);

            // Act
            await _vendaService.CancelarVendaAsync(venda.Id);

            // Assert
            venda.Cancelado.Should().BeTrue();
            await _vendaRepository.Received(1).UpdateAsync(venda);
        }

        [Fact]
        public async Task CancelarVenda_NaoDeveExecutarSeVendaNaoExistir()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            _vendaRepository.GetByIdAsync(vendaId).Returns((Venda)null);

            // Act
            await _vendaService.CancelarVendaAsync(vendaId);

            // Assert
            await _vendaRepository.DidNotReceive().UpdateAsync(Arg.Any<Venda>());
        }

        [Fact]
        public async Task ObterTodasVendas_DeveRetornarListaDeVendas()
        {
            // Arrange
            var vendas = new List<Venda>
        {
            new Venda { Id = Guid.NewGuid(), NumeroVenda = 1, Cliente = "Cliente 1" },
            new Venda { Id = Guid.NewGuid(), NumeroVenda = 2, Cliente = "Cliente 2" }
        };
            _vendaRepository.GetAllAsync().Returns(vendas);

            // Act
            var result = await _vendaService.ObterTodasVendasAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(vendas);
        }

        [Fact]
        public async Task ObterVendaPorId_DeveRetornarVendaSeExistir()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), NumeroVenda = 1, Cliente = "Cliente Teste" };
            _vendaRepository.GetByIdAsync(venda.Id).Returns(venda);

            // Act
            var result = await _vendaService.ObterVendaPorIdAsync(venda.Id);

            // Assert
            result.Should().Be(venda);
        }

        [Fact]
        public async Task ObterVendaPorId_DeveRetornarNullSeVendaNaoExistir()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            _vendaRepository.GetByIdAsync(vendaId).Returns((Venda)null);

            // Act
            var result = await _vendaService.ObterVendaPorIdAsync(vendaId);

            // Assert
            result.Should().BeNull();
        }


    }
}
