using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Contracts
{
    // Интерфейс репозитория для сущности FileModel
    public interface IFileModelRepository : IBaseRepository<FileModel>
    {
        // Асинхронно получает список классов из файла по его идентификатору.
        Task<List<ClassModel>> GetAllClassesFromFile(int fileId);
    }
}
