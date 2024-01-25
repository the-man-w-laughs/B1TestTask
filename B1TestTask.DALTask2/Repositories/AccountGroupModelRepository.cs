using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Repositories
{
    // Репозиторий для работы с сущностью AccountGroupModel
    public class AccountGroupModelRepository : BaseRepository<B1TestTask2DBContext, AccountGroupModel>, IAccountGroupModelRepository
    {
        // Конструктор с передачей контекста базы данных
        public AccountGroupModelRepository(B1TestTask2DBContext context)
            : base(context) { }
    }

}
