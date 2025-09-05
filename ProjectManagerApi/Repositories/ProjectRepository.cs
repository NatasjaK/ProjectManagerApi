using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data;
using ProjectManagerApi.Interfaces;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _db;
        
        public ProjectRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Project>> GetAllAsync(bool completedOnly, bool sortDesc)
        {
            var q = _db.Projects.AsQueryable();
            if (completedOnly) q = q.Where(p => p.IsCompleted);
            q = sortDesc ? q.OrderByDescending(p => p.CreatedAtUtc)
                         : q.OrderBy(p => p.CreatedAtUtc);
            return await q.ToListAsync();
        }

        public Task<Project?> GetByIdAsync(int id) =>
            _db.Projects.FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Project project) => await _db.Projects.AddAsync(project);

        public Task UpdateAsync(Project project)
        {
            _db.Projects.Update(project);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Project project)
        {
            _db.Projects.Remove(project);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
