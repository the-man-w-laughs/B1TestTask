using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DALTask2.Repositories
{
    // Репозиторий для работы с сущностью FileModel
    public class FileModelRepository : BaseRepository<B1TestTask2DBContext, FileModel>, IFileModelRepository
    {
        private readonly B1TestTask2DBContext _context;

        // Конструктор с передачей контекста базы данных
        public FileModelRepository(B1TestTask2DBContext context)
            : base(context)
        {
            _context = context;
        }
        
        // Метод для асинхронного получения всех классов из файла по идентификатору файла
        public async Task<List<ClassModel>> GetAllClassesFromFile(int fileId)
        {
            // Используем Entity Framework для загрузки связанных данных из базы данных
            return await _context.Classes
                .Include(c => c.AccountGroups)
                .ThenInclude(ag => ag.Accounts)
                .ThenInclude(a => a.Rows)
                .Select(c => new ClassModel
                {
                    // Проекция данных из базы данных на модель ClassModel
                    ClassModelId = c.ClassModelId,
                    ClassName = c.ClassName,

                    // Проекция данных из базы данных на связанную модель AccountGroupModel
                    AccountGroups = c.AccountGroups.Select(ag => new AccountGroupModel
                    {
                        AccountGroupId = ag.AccountGroupId,
                        GroupName = ag.GroupName,

                        // Проекция данных из базы данных на связанную модель AccountModel
                        Accounts = ag.Accounts.Select(a => new AccountModel
                        {
                            AccountId = a.AccountId,
                            AccountNumber = a.AccountNumber,

                            // Фильтрация строк по идентификатору файла и проекция данных на модель RowModel
                            Rows = a.Rows.Where(r => r.FileId == fileId).Select(r => new RowModel
                            {
                                RowId = r.RowId,
                                Content = new RowContent
                                {
                                    IncomingActive = r.Content.IncomingActive,
                                    IncomingPassive = r.Content.IncomingPassive,
                                    TurnoverDebit = r.Content.TurnoverDebit,
                                    TurnoverCredit = r.Content.TurnoverCredit,
                                    OutgoingActive = r.Content.OutgoingActive,
                                    OutgoingPassive = r.Content.OutgoingPassive
                                }
                            }).ToList()
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
        }

    }

}
