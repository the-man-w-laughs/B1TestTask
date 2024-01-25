using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация сущности ClassModel
    public class ClassModelConfiguration : IEntityTypeConfiguration<ClassModel>
    {
        public void Configure(EntityTypeBuilder<ClassModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(c => c.ClassModelId);

            // Установка обязательного свойства ClassName
            builder.Property(c => c.ClassName).IsRequired();

            // Установка связи между ClassModel и AccountGroupModel
            builder
                .HasMany(c => c.AccountGroups)
                .WithOne(ag => ag.ClassModel)
                .HasForeignKey(ag => ag.ClassModelId)
                .OnDelete(DeleteBehavior.Cascade); // Указание на каскадное удаление при удалении связанного объекта
        }
    }

}
