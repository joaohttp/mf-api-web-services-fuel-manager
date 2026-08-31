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
            var model = await _context.Consumos.ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
          
            _context.veiculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumos.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null)
            {
                return NotFound();
            }
            gerar Links(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var modeloDb = await _context.Consumos

                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (modeloDb == null)
            {
                return NotFound();
            }

            _context.veiculos.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumos.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            _context.veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private void gerar links(Consumo)
        {
            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(GetById), new { id = veiculo.Id }), "self", "GET"));
            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(Update), new { id = veiculo.Id }), "update_veiculo", "PUT"));
            veiculo.Links.Add(new LinkDto(veiculo.Id, Url.Action(nameof(Delete), new { id = veiculo.Id }), "delete_veiculo", "DELETE"));
        }
    }
}
}
