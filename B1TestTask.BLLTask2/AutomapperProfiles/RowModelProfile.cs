using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Models;

namespace B1TestTask.BLLTask2.AutomapperProfiles
{
    // Класс, представляющий профиль AutoMapper для маппинга между DTO и моделями строк
    public class RowModelProfile : BaseProfile
    {
        public RowModelProfile()
        {
            // Определение маппинга для RowContentDto к RowContent и наоборот
            CreateMap<RowContentDto, RowContent>();
            CreateMap<RowContent, RowContentDto>();
        }
    }
}
