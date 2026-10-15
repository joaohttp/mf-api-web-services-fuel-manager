using Microsoft.EntityFrameworkCore;
using mf_api_web_services_fuel_manager.Models; // ajuste o namespace se necessário

public class AppDbContext : DbContext
{
    // outras propriedades existentes...
    public DbSet<Usuario> Usuarios { get; set; }

    // construtor / OnModelCreating / etc...
}

using mf_api_web_services_fuel_manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SuaApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Set<Usuario>().ToListAsync();

            // Gerando os links HATEOAS para cada usuário da lista
            foreach (var usuario in model)
            {
                GerarLinks(usuario);
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Usuario model)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hash da senha antes de salvar
            // Validações básicas para a entidade Usuario (ex: campos obrigatórios)
            if (string.IsNullOrWhiteSpace(model.Nome) || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Nome e E-mail são campos obrigatórios.");
            }

            _context.Set<Usuario>().Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Usuario model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var modeloDb = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (modeloDb == null)
            {
                return NotFound();
            }

            _context.Usuarios.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Usuarios.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void GerarLinks(Usuario usuario)
        {
            // Garante que a lista de links não seja nula antes de adicionar
            usuario.Links ??= new List<LinkDto>();

            usuario.Links.Add(new LinkDto(usuario.Id, Url.Action(nameof(GetById), new { id = usuario.Id }), "self", "GET"));
            usuario.Links.Add(new LinkDto(usuario.Id, Url.Action(nameof(Update), new { id = usuario.Id }), "update_usuario", "PUT"));
            usuario.Links.Add(new LinkDto(usuario.Id, Url.Action(nameof(Delete), new { id = usuario.Id }), "delete_usuario", "DELETE"));
        }
    }
}