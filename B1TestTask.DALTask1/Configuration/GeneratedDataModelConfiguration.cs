using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using B1TestTask.DALTask1.Models;

namespace B1TestTask.DALTask2.Configuration
{
    public class GeneratedDataModelConfiguration : IEntityTypeConfiguration<GeneratedDataModel>
    {
        public void Configure(EntityTypeBuilder<GeneratedDataModel> builder)
        {
            builder.HasKey(ag => ag.Id);           
        }
    }
}
