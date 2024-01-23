using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Repositories
{
    public class FileRepository : BaseRepository<B1TestTask2DBContext, FileModel>, IFileRepository
    {
        public FileRepository(B1TestTask2DBContext context)
            : base(context) { }
    }
}
