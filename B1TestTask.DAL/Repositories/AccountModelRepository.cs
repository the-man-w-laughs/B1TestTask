using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Repositories
{
    public class AccountModelRepository : BaseRepository<B1TestTask2DBContext, AccountModel>, IAccountModelRepository
    {
        public AccountModelRepository(B1TestTask2DBContext context)
            : base(context) { }
    }
}
