using Microsoft.Extensions.Logging;
using Vendas.Domain.Entities;
using Vendas.Domain.Interfaces.Repository;
using Vendas.Domain.Interfaces.Service;

namespace Vendas.Domain
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly ILogger<VendaService> _logger;

        public VendaService(IVendaRepository vendaRepository, ILogger<VendaService> logger)
        {
            _vendaRepository = vendaRepository;
            _logger = logger;
        }

        public async Task<Venda> CriarVendaAsync(Venda venda)
        {
            venda.Id = Guid.NewGuid();
            venda.DataVenda = DateTime.UtcNow;
            venda.Cancelado = false;

            await _vendaRepository.AddAsync(venda);
            _logger.LogInformation("CompraCriada: {VendaId}", venda.Id);
            return venda;
        }

        public async Task<Venda> AtualizarVendaAsync(Venda venda)
        {
            await _vendaRepository.UpdateAsync(venda);
            _logger.LogInformation("CompraAlterada: {VendaId}", venda.Id);
            return venda;
        }

        public async Task CancelarVendaAsync(Guid id)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda != null)
            {
                venda.Cancelado = true;
                await _vendaRepository.UpdateAsync(venda);
                _logger.LogInformation("CompraCancelada: {VendaId}", venda.Id);
            }
        }

        public async Task<IEnumerable<Venda>> ObterTodasVendasAsync()
        {
            return await _vendaRepository.GetAllAsync();
        }

        public async Task<Venda> ObterVendaPorIdAsync(Guid id)
        {
            return await _vendaRepository.GetByIdAsync(id);
        }
    }
}
