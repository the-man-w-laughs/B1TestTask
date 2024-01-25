using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1TestTask.Shared.Repository
{
    // Абстрактный базовый репозиторий для работы с сущностями TEntity в контексте базы данных типа TDbContext
    public abstract class BaseRepository<TDbContext, TEntity> : IBaseRepository<TEntity>
        where TEntity : class, new()
        where TDbContext : DbContext
    {
        // Экземпляр контекста базы данных
        protected readonly TDbContext DbContext;
        // Набор данных для работы с сущностями TEntity
        private readonly DbSet<TEntity> _table;

        // Конструктор, инициализирующий контекст базы данных и набор данных
        protected BaseRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            _table = dbContext.Set<TEntity>();
        }

        // Получение всех сущностей асинхронно
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        // Получение всех сущностей с учетом пагинации асинхронно
        public virtual async Task<List<TEntity>> GetAllAsync(
            int offset,
            int limit,
            CancellationToken cancellationToken = default
        )
        {
            return await DbContext
                .Set<TEntity>()
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        // Получение всех сущностей, удовлетворяющих условию, с учетом пагинации асинхронно
        public virtual async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> where,
            int offset,
            int limit,
            CancellationToken cancellationToken = default
        )
        {
            return await DbContext
                .Set<TEntity>()
                .Where(where)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        // Получение сущности по идентификатору асинхронно
        public virtual async Task<TEntity?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            return await _table.FindAsync(id, cancellationToken);
        }

        // Получение сущности, удовлетворяющей условию асинхронно
        public virtual async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default
        )
        {
            return await _table.FirstOrDefaultAsync(where, cancellationToken);
        }

        // Добавление сущности асинхронно
        public async Task<TEntity> AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            return (await DbContext.AddAsync(entity, cancellationToken)).Entity;
        }

        // Обновление сущности
        public virtual void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        // Удаление сущности
        public virtual void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        // Удаление сущности по идентификатору асинхронно
        public virtual async Task<TEntity?> DeleteByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await GetByIdAsync(id, cancellationToken);

            if (entity != null)
            {
                Delete(entity);
            }

            return entity;
        }

        // Сохранение изменений в базе данных асинхронно
        public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

}
