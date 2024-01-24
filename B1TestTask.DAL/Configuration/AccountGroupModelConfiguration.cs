using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Configuration
{
    public class AccountGroupModelConfiguration : IEntityTypeConfiguration<AccountGroupModel>
    {
        public void Configure(EntityTypeBuilder<AccountGroupModel> builder)
        {
            builder.HasKey(ag => ag.AccountGroupId);
            builder.Property(ag => ag.GroupName).IsRequired();

            builder
                .HasOne(ag => ag.ClassModel)
                .WithMany(c => c.AccountGroups)
                .HasForeignKey(ag => ag.ClassModelId)                
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
