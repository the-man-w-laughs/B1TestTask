using B1TestTask.DALTask1.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask1.Contracts
{
    // Интерфейс репозитория для работы с моделью данных GeneratedDataModel
    public interface IGeneratedDataModelRepository : IBaseRepository<GeneratedDataModel>
    {
        // Асинхронный метод для вызова расчета суммы и медианы
        Task<Tuple<decimal, decimal>> CallCalculateSumAndMedianAsync();
    }

}
