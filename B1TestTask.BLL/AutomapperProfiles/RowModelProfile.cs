using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Models;

namespace B1TestTask.BLLTask2.AutomapperProfiles
{
    public class RowModelProfile : BaseProfile
    {
        public RowModelProfile()
        {
            CreateMap<RowContentDto, RowContent>();
            CreateMap<RowContent, RowContentDto>();
        }
    }
}
