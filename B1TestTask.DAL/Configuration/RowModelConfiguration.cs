using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B1TestTask.DALTask2.Configuration
{
    public class RowModelConfiguration : IEntityTypeConfiguration<RowModel>
    {
        public void Configure(EntityTypeBuilder<RowModel> builder)
        {
            builder.HasKey(r => r.RowId);

            builder.OwnsOne(r => r.Content, content =>
            {
                content.Property(c => c.IncomingActive).IsRequired();
                content.Property(c => c.IncomingPassive).IsRequired();
                content.Property(c => c.TurnoverDebit).IsRequired();
                content.Property(c => c.TurnoverCredit).IsRequired();
                content.Property(c => c.OutgoingActive).IsRequired();
                content.Property(c => c.OutgoingPassive).IsRequired();
            });

            builder
                .HasOne(r => r.Account)
                .WithMany(a => a.Rows)
                .HasForeignKey(r => r.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
