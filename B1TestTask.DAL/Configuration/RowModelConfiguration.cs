using B1TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B1TestTask.DAL.Configuration
{
    public class RowModelConfiguration : IEntityTypeConfiguration<RowModel>
    {
        public void Configure(EntityTypeBuilder<RowModel> builder)
        {
            builder.HasKey(r => r.RowId);
            builder.Property(r => r.Content).IsRequired();

            builder
                .HasOne(r => r.Account)
                .WithMany(a => a.Rows)
                .HasForeignKey(r => r.AccountId)
                .IsRequired(); 
        }
    }
}
