using ProjectManagerApi.Dtos;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync(bool completedOnly, bool sortDesc);
        Task<Project?> GetByIdAsync(int id);
        Task<Project> CreateAsync(ProjectCreateDto dto);
        Task<Project?> UpdateAsync(int id, ProjectUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
