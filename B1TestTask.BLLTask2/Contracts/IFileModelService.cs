using B1TestTask.BLLTask2.Dtos;

namespace B1TestTask.BLLTask2.Contracts
{
    // Интерфейс, предоставляющий функциональность сервиса для работы с моделями файлов
    public interface IFileModelService
    {
        // Асинхронный метод для добавления файла
        Task AddFile(FileModelDto fileContentDto);

        // Асинхронный метод для получения списка всех файлов
        Task<List<FileModelDto>> GetAllFiles();
    }
}
