using B1TestTask.DALTask1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace B1TestTask.DALTask1
{
    public class B1TestTask1DBContext : DbContext
    {
        public DbSet<GeneratedDataModel> GeneratedDataModels { get; set; }        

        private readonly IConfiguration configuration;

        public B1TestTask1DBContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionTask1");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
