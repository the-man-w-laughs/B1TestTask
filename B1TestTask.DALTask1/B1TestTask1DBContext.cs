using B1TestTask.DALTask1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace B1TestTask.DALTask1
{
    // Контекст базы данных для работы с таблицами GeneratedDataModel и TempOutputTable
    public class B1TestTask1DBContext : DbContext
    {
        // Таблица для хранения сгенерированных данных
        public DbSet<GeneratedDataModel> GeneratedDataModels { get; set; }

        // Таблица для хранения результатов вычислений суммы и медианы
        public DbSet<CalculateSumAndMedianResult> TempOutputTable { get; set; }

        private readonly IConfiguration configuration;        
        public B1TestTask1DBContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            // Убедимся, что база данных создана
            Database.EnsureCreated();
        }

        // Метод для настройки подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Получение строки подключения из конфигурации
            string connectionString = configuration.GetConnectionString("DefaultConnectionTask1");
            
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
        }

        // Метод для настройки модели данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применение конфигураций из текущей сборки
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }

}
