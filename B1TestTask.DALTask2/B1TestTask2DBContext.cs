using B1TestTask.DALTask2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace B1TestTask.DALTask2
{
    public class B1TestTask2DBContext : DbContext
    {
        public DbSet<AccountGroupModel> AccountGroups { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<RowModel> Rows { get; set; }

        private readonly IConfiguration configuration;

        public B1TestTask2DBContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            // Создание базы данных, если она еще не создана
            Database.EnsureCreated();
        }

        // Переопределение метода для настройки параметров соединения с базой данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Получение строки подключения к базе данных из конфигурации
            string connectionString = configuration.GetConnectionString("DefaultConnectionTask2");
            // Использование MySQL в качестве провайдера базы данных и указание версии сервера
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
        }

        // Переопределение метода для настройки модели данных при помощи Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применение конфигураций модели данных из текущей сборки
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
