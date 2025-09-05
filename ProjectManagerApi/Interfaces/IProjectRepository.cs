using ProjectManagerApi.Models;

namespace ProjectManagerApi.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(bool completedOnly, bool sortDesc);
        Task<Project?> GetByIdAsync(int id);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<int> SaveChangesAsync();
    }
}
