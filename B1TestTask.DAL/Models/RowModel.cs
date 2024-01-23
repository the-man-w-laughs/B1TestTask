using B1TestTask.DAL.Configuration;

namespace B1TestTask.DAL.Models
{
    public class RowModel
    {
        public int RowId { get; set; }
        public RowContent Content { get; set; } = new RowContent();
        public int FileId { get; set; }
        public int AccountId { get; set; }
        public FileModel? File { get; set; } = new FileModel();
        public AccountModel Account { get; set; } = new AccountModel();
    }

}
