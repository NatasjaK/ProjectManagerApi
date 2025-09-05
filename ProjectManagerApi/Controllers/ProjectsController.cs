using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Dtos;
using ProjectManagerApi.Interfaces;

namespace ProjectManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service) => _service = service;

        // GET: /api/projects?sort=desc&completedOnly=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? sort = "desc", [FromQuery] bool completedOnly = false)
        {
            var sortDesc = !string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase);
            var items = await _service.GetAllAsync(completedOnly, sortDesc);
            return Ok(items);
        }



        // GET: /api/projects/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: /api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: /api/projects/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: /api/projects/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
