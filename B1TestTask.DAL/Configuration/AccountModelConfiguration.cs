using B1TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DAL.Configuration
{
    public class AccountModelConfiguration : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.HasKey(a => a.AccountId);
            builder.Property(a => a.AccountNumber).IsRequired();
            builder.Property(a => a.AccountName).IsRequired();

            builder
                .HasOne(a => a.AccountGroup)
                .WithMany(ag => ag.Accounts)
                .HasForeignKey(a => a.AccountGroupId)
                .IsRequired();
        }
    }
}
