using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Data.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal abstract class BaseRepository<T>
        where T : BaseEntity
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public abstract EntityType EntityType { get; }

        public BaseRepository(ReadingIsGoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Create(T entity, int userId)
        {
            AddEntity(entity);
            await _dbContext.SaveChangesAsync();

            LogActivity(entity, userId);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(T entity, int userId)
        {
            LogActivity(entity, userId);
            await _dbContext.SaveChangesAsync();
        }

        public abstract void AddEntity(T entity);

        private void LogActivity(T entity, int userId)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            _dbContext.ActivityLogs.Add(new ActivityLogEntity
            {
                EntityType = EntityType,
                EntityId = entity.Id,
                UserId = userId,
                New = JsonSerializer.Serialize(entity, options)
            });
        }
    }
}
