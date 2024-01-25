using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Repositories
{
    public class FileModelRepository : BaseRepository<B1TestTask2DBContext, FileModel>, IFileModelRepository
    {
        private readonly B1TestTask2DBContext _context;

        public FileModelRepository(B1TestTask2DBContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<ClassModel>> GetAllClassesFromFile(int fileId)
        {
            return await _context.Classes
                .Include(c => c.AccountGroups)
                .ThenInclude(ag => ag.Accounts)
                .ThenInclude(a => a.Rows) 
                .Select(c => new ClassModel
                {                    
                    ClassModelId = c.ClassModelId,
                    ClassName = c.ClassName,                    

                    AccountGroups = c.AccountGroups.Select(ag => new AccountGroupModel
                    {                        
                        AccountGroupId = ag.AccountGroupId,
                        GroupName = ag.GroupName,                        

                        Accounts = ag.Accounts.Select(a => new AccountModel
                        {                            
                            AccountId = a.AccountId,
                            AccountNumber = a.AccountNumber,                            

                            Rows = a.Rows.Where(r => r.FileId == fileId).ToList()
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
