using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Entities;

namespace Vendas.Domain.Interfaces.Service
{
    public interface IVendaService
    {
        Task CancelarVendaAsync(Guid id);
        Task<Venda> AtualizarVendaAsync(Venda venda);
        Task<Venda> CriarVendaAsync(Venda venda);
        Task<IEnumerable<Venda>> ObterTodasVendasAsync();
        Task<Venda> ObterVendaPorIdAsync(Guid id);
    }
}
