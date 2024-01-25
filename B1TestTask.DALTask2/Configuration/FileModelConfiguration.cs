using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
{
    // Конфигурация сущности FileModel
    public class FileModelConfiguration : IEntityTypeConfiguration<FileModel>
    {
        public void Configure(EntityTypeBuilder<FileModel> builder)
        {
            // Установка первичного ключа
            builder.HasKey(f => f.FileId);

            // Установка обязательного свойства FileName
            builder.Property(f => f.FileName).IsRequired();

            // Установка связи между FileModel и RowModel
            builder
                .HasMany(f => f.Rows)
                .WithOne(r => r.File)
                .HasForeignKey(r => r.FileId)
                .OnDelete(DeleteBehavior.Cascade); // Указание на каскадное удаление при удалении связанного объекта

            // Определение вложенного объекта FileContent
            builder.OwnsOne(f => f.FileContent, fc =>
            {                
                fc.Property(fc => fc.BankName).IsRequired();
                fc.Property(fc => fc.FileTitle).IsRequired();
                fc.Property(fc => fc.Period).IsRequired();
                fc.Property(fc => fc.AdditionalInfo).IsRequired();
                fc.Property(fc => fc.GenerationDate).IsRequired();
                fc.Property(fc => fc.Currency).IsRequired();
            });
        }
    }

}
