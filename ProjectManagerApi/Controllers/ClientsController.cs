using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ProjectManagerApi.Data;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        => Ok(await db.Clients.OrderBy(x => x.Name).ToListAsync());

    public record ClientNameDto([Required, MaxLength(200)] string Name);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientNameDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var exists = await db.Clients.AnyAsync(x => x.Name == dto.Name);
        if (exists) return Conflict("Client already exists.");

        var c = new Client { Name = dto.Name };
        db.Clients.Add(c);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = c.Id }, c);
    }
}
