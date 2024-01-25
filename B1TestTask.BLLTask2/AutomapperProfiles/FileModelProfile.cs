using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Models;

namespace B1TestTask.BLLTask2.AutomapperProfiles
{
    // Класс, представляющий профиль AutoMapper для маппинга между DTO и моделями файлов
    public class FileModelProfile : BaseProfile
    {
        public FileModelProfile()
        {
            // Определение маппинга для FileModelDto к FileModel и наоборот
            CreateMap<FileModelDto, FileModel>();
            CreateMap<FileContentDto, FileContent>();

            // Определение маппинга для FileModel к FileModelDto и наоборот
            CreateMap<FileModel, FileModelDto>();
            CreateMap<FileContent, FileContentDto>();
        }
    }
}
