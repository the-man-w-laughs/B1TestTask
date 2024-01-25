using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using B1TestTask.DALTask1.Models;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация модели данных GeneratedDataModel для работы с Entity Framework Core
    public class GeneratedDataModelConfiguration : IEntityTypeConfiguration<GeneratedDataModel>
    {
        public void Configure(EntityTypeBuilder<GeneratedDataModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(ag => ag.Id);
        }
    }

}
