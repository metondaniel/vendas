using Microsoft.AspNetCore.Mvc;
using Vendas.Domain.Entities;
using Vendas.Domain.Interfaces.Service;

namespace Vendas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        // GET: api/vendas
        [HttpGet]
        public async Task<IActionResult> GetVendas()
        {
            var vendas = await _vendaService.ObterTodasVendasAsync();
            return Ok(vendas);
        }

        // GET: api/vendas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenda(Guid id)
        {
            var venda = await _vendaService.ObterVendaPorIdAsync(id);
            if (venda == null)
                return NotFound();
            return Ok(venda);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenda([FromBody] Venda venda)
        {
            var novaVenda = await _vendaService.CriarVendaAsync(venda);
            return CreatedAtAction(nameof(GetVenda), new { id = novaVenda.Id }, novaVenda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenda(Guid id, [FromBody] Venda venda)
        {
            if (id != venda.Id)
                return BadRequest();

            await _vendaService.AtualizarVendaAsync(venda);
            return NoContent();
        }

        // DELETE: api/vendas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenda(Guid id)
        {
            await _vendaService.CancelarVendaAsync(id);
            return NoContent();
        }
    }
}
