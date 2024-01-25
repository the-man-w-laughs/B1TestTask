using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask1.Repositories
{
    public class GeneratedDataRepository : BaseRepository<B1TestTask1DBContext, GeneratedDataModel>, IGeneratedDataRepository
    {
        public GeneratedDataRepository(B1TestTask1DBContext context)
            : base(context) { }
    }
}
