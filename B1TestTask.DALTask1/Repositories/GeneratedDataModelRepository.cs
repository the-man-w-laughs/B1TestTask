using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask1.Repositories
{
    public class GeneratedDataModelRepository : BaseRepository<B1TestTask1DBContext, GeneratedDataModel>, IGeneratedDataModelRepository
    {
        public GeneratedDataModelRepository(B1TestTask1DBContext context)
            : base(context) { }
    }
}
