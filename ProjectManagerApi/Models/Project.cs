using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? DueDateUtc { get; set; }
    }
}
