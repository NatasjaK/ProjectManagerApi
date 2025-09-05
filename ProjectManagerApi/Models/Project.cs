// AI-genererad kod av ChatGPT
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public DateTime? StartDateUtc { get; set; }
        public DateTime? DueDateUtc { get; set; }

        [MaxLength(200)]
        public string? ClientName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        public int? ClientId { get; set; }

        [JsonIgnore]
        public Client? Client { get; set; }

        public int? ProjectOwnerId { get; set; }        
        [Column("OwnerName")]
        public string? ProjectOwnerName { get; set; }     

        [JsonIgnore]
        [ForeignKey(nameof(ProjectOwnerId))]
        public ProjectOwner? Owner { get; set; }
    }
}
