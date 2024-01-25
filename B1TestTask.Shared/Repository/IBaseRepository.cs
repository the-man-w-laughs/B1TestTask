using System.Linq.Expressions;

namespace B1TestTask.Shared.Repository
{
    // Интерфейс базового репозитория для работы с сущностями TEntity
    public interface IBaseRepository<TEntity>
        where TEntity : class, new()
    {
        // Получение всех сущностей асинхронно
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        // Получение всех сущностей с учетом пагинации асинхронно
        Task<List<TEntity>> GetAllAsync(
            int offset,
            int limit,
            CancellationToken cancellationToken = default
        );

        // Получение всех сущностей, удовлетворяющих условию, с учетом пагинации асинхронно
        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> where,
            int offset,
            int limit,
            CancellationToken cancellationToken
        );

        // Получение сущности по идентификатору асинхронно
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);

        // Получение сущности, удовлетворяющей условию асинхронно
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default
        );

        // Добавление сущности асинхронно
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        // Обновление сущности
        void Update(TEntity entity);

        // Удаление сущности
        void Delete(TEntity entity);

        // Удаление сущности по идентификатору асинхронно
        Task<TEntity?> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

        // Сохранение изменений в базе данных асинхронно
        Task SaveAsync(CancellationToken cancellationToken = default);
    }

}
