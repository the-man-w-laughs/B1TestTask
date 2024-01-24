using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Repositories
{
    public class FileModelRepository : BaseRepository<B1TestTask2DBContext, FileModel>, IFileModelRepository
    {
        public FileModelRepository(B1TestTask2DBContext context)
            : base(context) { }
    }
}
