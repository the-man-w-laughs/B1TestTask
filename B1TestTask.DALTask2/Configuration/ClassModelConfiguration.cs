using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
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
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
