using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация сущности RowModel
    public class RowModelConfiguration : IEntityTypeConfiguration<RowModel>
    {
        public void Configure(EntityTypeBuilder<RowModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(r => r.RowId);

            // Определение вложенного объекта Content
            builder.OwnsOne(r => r.Content, content =>
            {
                content.Property(c => c.IncomingActive).IsRequired();
                content.Property(c => c.IncomingPassive).IsRequired();
                content.Property(c => c.TurnoverDebit).IsRequired();
                content.Property(c => c.TurnoverCredit).IsRequired();
                content.Property(c => c.OutgoingActive).IsRequired();
                content.Property(c => c.OutgoingPassive).IsRequired();
            });

            // Установка связи между RowModel и AccountModel
            builder
                .HasOne(r => r.Account)
                .WithMany(a => a.Rows)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Указание на каскадное удаление при удалении связанного объекта

        }
    }
}        
