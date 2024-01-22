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
                .IsRequired();
        }
    }
}
