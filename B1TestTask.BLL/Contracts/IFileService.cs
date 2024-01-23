using B1TestTask.BLLTask2.Dtos;

namespace B1TestTask.BLLTask2.Contracts
{
    internal interface IFileService
    {
        void AddFile(FileContentDto fileContentDto);
    }
}
