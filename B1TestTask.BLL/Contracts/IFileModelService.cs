using B1TestTask.BLLTask2.Dtos;

namespace B1TestTask.BLLTask2.Contracts
{
    public interface IFileModelService
    {
        Task AddFile(FileModelDto fileContentDto);
    }
}
