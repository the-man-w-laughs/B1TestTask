using B1TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DAL.Configuration
{
    public class ClassModelConfiguration : IEntityTypeConfiguration<ClassModel>
    {
        public void Configure(EntityTypeBuilder<ClassModel> builder)
        {
            builder.HasKey(c => c.ClassModelId);
            builder.Property(c => c.ClassName).IsRequired();

            builder
                .HasMany(c => c.AccountGroups)
                .WithOne(ag => ag.ClassModel)
                .HasForeignKey(ag => ag.ClassModelId)
                .IsRequired();
        }
    }
}
