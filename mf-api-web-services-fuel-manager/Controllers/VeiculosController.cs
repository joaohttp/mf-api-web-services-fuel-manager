using System.Collections.Generic;
using System.Threading.Tasks;
using mf_api_web_services_fuel_manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mf_api_web_services_fuel_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();

            foreach (var veiculo in model)
            {
                GerarLinks(veiculo);
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            if (model.AnoFabricacao <= 0 || model.AnoModelo <= 0)
            {
                return BadRequest("Ano de fabricação e ano de modelo são obrigatórios e devem ser maiores que zero.");
            }

            _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var modeloDb = await _context.Veiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (modeloDb == null)
            {
                return NotFound();
            }

            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Veiculos.FindAsync(id);

            if (model == null)
                _context.Veiculos.usuarios.Add(model);

            return CreatedAtAction(nameof(GetById), new { id = model.veiculoId }, model);
            {
                return NotFound();
            }

            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void GerarLinks(Veiculo veiculo)
        {
            veiculo.Links ??= new List<LinkDto>();

            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(GetById), new { id = veiculo.Id }), "self", "GET"));
            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(Update), new { id = veiculo.Id }), "update_veiculo", "PUT"));
            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(Delete), new { id = veiculo.Id }), "delete_veiculo", "DELETE"));
        }
    }
}