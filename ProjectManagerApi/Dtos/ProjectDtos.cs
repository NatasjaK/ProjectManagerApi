using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dtos
{
    public class ProjectCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime? DueDateUtc { get; set; }

        [MaxLength(200)]
        public string? ClientName { get; set; }

        public DateTime? StartDateUtc { get; set; }

        [MaxLength(200)]
        public string? ProjectOwnerName { get; set; }

        public decimal? Budget { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        public int? ClientId { get; set; }
        public int? ProjectOwnerId { get; set; }
    }

    public class ProjectUpdateDto : ProjectCreateDto { }
}
