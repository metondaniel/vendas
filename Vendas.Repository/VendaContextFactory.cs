using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vendas.Repository
{
    public class VendaContextFactory : IDesignTimeDbContextFactory<VendaContext>
    {
        public VendaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VendaContext>();

            optionsBuilder.UseSqlite("Data Source=123Vendas.db");

            return new VendaContext(optionsBuilder.Options);
        }
    }
}
