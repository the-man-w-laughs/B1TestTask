using B1TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace B1TestTask.DAL
{
    public class B1TestTaskDBContext : DbContext
    {
        public DbSet<AccountGroupModel> AccountGroups { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<RowModel> Rows { get; set; }

        private readonly IConfiguration configuration;

        public B1TestTaskDBContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());            
        }        
    }
}
