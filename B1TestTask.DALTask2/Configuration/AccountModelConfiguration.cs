using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация сущности AccountModel
    public class AccountModelConfiguration : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(a => a.AccountId);

            // Установка обязательного свойства AccountNumber
            builder.Property(a => a.AccountNumber).IsRequired();

            // Установка связи между AccountModel и AccountGroupModel
            builder
                .HasOne(a => a.AccountGroup)
                .WithMany(ag => ag.Accounts)
                .HasForeignKey(a => a.AccountGroupId)
                .OnDelete(DeleteBehavior.Cascade); // Указание на каскадное удаление при удалении связанного объекта
        }
    }

}
