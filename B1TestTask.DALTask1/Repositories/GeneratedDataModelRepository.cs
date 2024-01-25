using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Models;
using B1TestTask.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask1.Repositories
{
    // Репозиторий для работы с моделью данных GeneratedDataModel
    public class GeneratedDataModelRepository : BaseRepository<B1TestTask1DBContext, GeneratedDataModel>, IGeneratedDataModelRepository
    {
        private readonly B1TestTask1DBContext _context;

        public GeneratedDataModelRepository(B1TestTask1DBContext context)
            : base(context)
        {
            _context = context;
        }

        // Асинхронный метод для вызова хранимой процедуры CalculateSumAndMedian
        public async Task<Tuple<decimal, decimal>> CallCalculateSumAndMedianAsync()
        {            
            var commandText = "CALL CalculateSumAndMedian";
            
            await _context.Database.ExecuteSqlRawAsync(commandText);

            // Получение результатов из временной таблицы
            var result = await _context.TempOutputTable.FirstOrDefaultAsync();

            // Создание кортежа с результатами
            return Tuple.Create(result?.SumOfIntegers ?? 0, result?.MedianOfDecimals ?? 0);
        }
    }
}
