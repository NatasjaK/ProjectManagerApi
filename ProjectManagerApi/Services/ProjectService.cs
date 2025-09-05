using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data;
using ProjectManagerApi.Dtos;
using ProjectManagerApi.Interfaces;
using ProjectManagerApi.Models;

namespace ProjectManagerApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;
        private readonly AppDbContext _db;

        public ProjectService(IProjectRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public Task<IEnumerable<Project>> GetAllAsync(bool completedOnly, bool sortDesc)
            => _repo.GetAllAsync(completedOnly, sortDesc);

        public Task<Project?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        private static string Normalize(string s) => s.Trim();

        private async Task<Client> GetOrCreateClientAsync(string name)
        {
            var norm = Normalize(name);
            var existing = await _db.Clients
                .FirstOrDefaultAsync(c => c.Name.ToLower() == norm.ToLower());
            if (existing != null) return existing;

            var c = new Client { Name = norm };
            _db.Clients.Add(c);
            try
            {
                await _db.SaveChangesAsync();
                return c;
            }
            catch (DbUpdateException)
            {
                var again = await _db.Clients.FirstAsync(x => x.Name.ToLower() == norm.ToLower());
                return again;
            }
        }

        private async Task<ProjectOwner> GetOrCreateProjectOwnerAsync(string name)
        {
            var norm = Normalize(name);
            var existing = await _db.ProjectOwners
                .FirstOrDefaultAsync(o => o.Name.ToLower() == norm.ToLower());
            if (existing != null) return existing;

            var o = new ProjectOwner { Name = norm };
            _db.ProjectOwners.Add(o);
            try
            {
                await _db.SaveChangesAsync();
                return o;
            }
            catch (DbUpdateException)
            {
                var again = await _db.ProjectOwners.FirstAsync(x => x.Name.ToLower() == norm.ToLower());
                return again;
            }
        }

        public async Task<Project> CreateAsync(ProjectCreateDto dto)
        {
            var entity = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                IsCompleted = dto.IsCompleted,
                CreatedAtUtc = DateTime.UtcNow,
                StartDateUtc = dto.StartDateUtc,
                DueDateUtc = dto.DueDateUtc,
                Budget = dto.Budget,
                Currency = dto.Currency
            };

            if (dto.ClientId.HasValue)
            {
                var c = await _db.Clients.FindAsync(dto.ClientId.Value);
                if (c != null) { entity.ClientId = c.Id; entity.ClientName = c.Name; }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ClientName))
            {
                var c = await GetOrCreateClientAsync(dto.ClientName);
                entity.ClientId = c.Id;
                entity.ClientName = c.Name;
            }

            if (dto.ProjectOwnerId.HasValue)
            {
                var o = await _db.ProjectOwners.FindAsync(dto.ProjectOwnerId.Value);
                if (o != null) { entity.ProjectOwnerId = o.Id; entity.ProjectOwnerName = o.Name; }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ProjectOwnerName))
            {
                var o = await GetOrCreateProjectOwnerAsync(dto.ProjectOwnerName);
                entity.ProjectOwnerId = o.Id;
                entity.ProjectOwnerName = o.Name;
            }

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public async Task<Project?> UpdateAsync(int id, ProjectUpdateDto dto)
       {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.ImageUrl = dto.ImageUrl;
            entity.IsCompleted = dto.IsCompleted;
            entity.StartDateUtc = dto.StartDateUtc;
            entity.DueDateUtc = dto.DueDateUtc;
            entity.Budget = dto.Budget;
            entity.Currency = dto.Currency;

            if (dto.ClientId.HasValue)
            {
                var c = await _db.Clients.FindAsync(dto.ClientId.Value);
                if (c != null) { entity.ClientId = c.Id; entity.ClientName = c.Name; }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ClientName))
            {
                var c = await GetOrCreateClientAsync(dto.ClientName);
                entity.ClientId = c.Id; entity.ClientName = c.Name;
            }
            else
            {
                entity.ClientId = null; entity.ClientName = null;
            }

            if (dto.ProjectOwnerId.HasValue)
            {
                var o = await _db.ProjectOwners.FindAsync(dto.ProjectOwnerId.Value);
                if (o != null) { entity.ProjectOwnerId = o.Id; entity.ProjectOwnerName = o.Name; }
            }
            else if (!string.IsNullOrWhiteSpace(dto.ProjectOwnerName))
            {
                var o = await GetOrCreateProjectOwnerAsync(dto.ProjectOwnerName);
                entity.ProjectOwnerId = o.Id; entity.ProjectOwnerName = o.Name;
            }
            else
            {
                entity.ProjectOwnerId = null; entity.ProjectOwnerName = null;
            }

            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}

