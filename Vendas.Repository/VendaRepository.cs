using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Vendas.Domain.Entities;
using Vendas.Domain.Interfaces.Repository;

namespace Vendas.Repository
{
    public class VendaRepository : IVendaRepository
    {
        private readonly VendaContext _context;

        public VendaRepository(VendaContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Venda venda)
        {
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Venda>> GetAllAsync()
        {
            return await _context.Vendas.Include(v => v.Itens).ToListAsync();
        }

        public async Task<Venda> GetByIdAsync(Guid id)
        {
            return await _context.Vendas.Include(v => v.Itens).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task UpdateAsync(Venda venda)
        {
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();
        }
    }
}
