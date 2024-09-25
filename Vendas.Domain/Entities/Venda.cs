using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Entities
{
    public class Venda
    {
        public Guid Id { get; set; }
        public int NumeroVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public string Cliente { get; set; }
        public decimal ValorTotal { get; set; }
        public string Filial { get; set; }
        public List<ItemVenda> Itens { get; set; }
        public bool Cancelado { get; set; }
    }
}
