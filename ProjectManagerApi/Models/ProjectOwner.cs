using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectManagerApi.Models
{
    public class ProjectOwner
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}

