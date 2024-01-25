using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Repositories
{
    // Репозиторий для работы с сущностью ClassModel
    public class ClassModelRepository : BaseRepository<B1TestTask2DBContext, ClassModel>, IClassModelRepository
    {
        // Конструктор с передачей контекста базы данных
        public ClassModelRepository(B1TestTask2DBContext context)
            : base(context) { }
    }

}
