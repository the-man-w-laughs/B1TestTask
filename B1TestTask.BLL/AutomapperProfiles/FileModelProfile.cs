using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Models;

namespace B1TestTask.BLLTask2.AutomapperProfiles
{
    public class FileModelProfile : BaseProfile
    {
        public FileModelProfile()
        {
            CreateMap<FileModelDto, FileModel>();
            CreateMap<FileContentDto, FileContent>();

            CreateMap<FileModel, FileModelDto>();
            CreateMap<FileContent, FileContentDto>();
        }
    }
}
