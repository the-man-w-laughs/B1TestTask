using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Models;
using B1TestTask.Shared.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

namespace B1TestTask.DALTask1.Repositories
{
    public class GeneratedDataModelRepository : BaseRepository<B1TestTask1DBContext, GeneratedDataModel>, IGeneratedDataModelRepository
    {
        private readonly B1TestTask1DBContext _context;

        public GeneratedDataModelRepository(B1TestTask1DBContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Tuple<decimal, decimal>> CallCalculateSumAndMedianAsync()
        {
            var commandText = "CALL CalculateSumAndMedian";
            await _context.Database.ExecuteSqlRawAsync(commandText);

            var result = await _context.TempOutputTable.FirstOrDefaultAsync();

            return Tuple.Create(result?.SumOfIntegers ?? 0, result?.MedianOfDecimals ?? 0);
        }

    }
}
