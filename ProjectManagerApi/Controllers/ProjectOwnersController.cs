using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ProjectManagerApi.Data;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class ProjectOwnersController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectOwner>>> GetAll()
        => Ok(await db.ProjectOwners.OrderBy(x => x.Name).ToListAsync()); 

    public record ProjectOwnerNameDto([Required, MaxLength(200)] string Name);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectOwnerNameDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (await db.ProjectOwners.AnyAsync(x => x.Name == dto.Name))
            return Conflict("Project Owner already exists.");

        var o = new ProjectOwner { Name = dto.Name };
        db.ProjectOwners.Add(o);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = o.Id }, o);
    }
}
