using B1TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DAL.Configuration
{
    public class FileModelConfiguration : IEntityTypeConfiguration<FileModel>
    {
        public void Configure(EntityTypeBuilder<FileModel> builder)
        {
            builder.HasKey(f => f.FileId);
            builder.Property(f => f.FileName).IsRequired();

            builder
                .HasMany(f => f.Rows)
                .WithOne(r => r.File)
                .HasForeignKey(r => r.FileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
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
