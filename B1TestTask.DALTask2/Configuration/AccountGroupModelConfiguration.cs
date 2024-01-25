using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация сущности AccountGroupModel
    public class AccountGroupModelConfiguration : IEntityTypeConfiguration<AccountGroupModel>
    {
        public void Configure(EntityTypeBuilder<AccountGroupModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(ag => ag.AccountGroupId);

            // Установка обязательного свойства GroupName
            builder.Property(ag => ag.GroupName).IsRequired();

            // Установка связи между AccountGroupModel и ClassModel
            builder
                .HasOne(ag => ag.ClassModel)
                .WithMany(c => c.AccountGroups)
                .HasForeignKey(ag => ag.ClassModelId)
                .OnDelete(DeleteBehavior.Cascade); // Указание на каскадное удаление при удалении связанного объекта
        }
    }

}
