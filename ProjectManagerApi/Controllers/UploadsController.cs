using Microsoft.AspNetCore.Mvc;

namespace ProjectManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadsController : ControllerBase
    {
        private static readonly HashSet<string> Allowed =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        private const long MaxBytes = 20 * 1024 * 1024; 

        private readonly IWebHostEnvironment _env;
        public UploadsController(IWebHostEnvironment env) => _env = env;

        [HttpPost("image")]
        [RequestSizeLimit(MaxBytes)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file.");

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext) || !Allowed.Contains(ext))
                return BadRequest("Unsupported file type.");

            if (file.ContentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == false)
                return BadRequest("Unsupported content type.");

            var webRoot = string.IsNullOrWhiteSpace(_env.WebRootPath)
                ? Path.Combine(_env.ContentRootPath, "wwwroot")
                : _env.WebRootPath;

            var uploadDir = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploadDir);

            var fileName = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var savePath = Path.Combine(uploadDir, fileName);

            await using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(fs);
            }

            var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(new { url });
        }
    }
}
