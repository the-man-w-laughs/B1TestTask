namespace B1TestTask.DAL.Models
{
    public class RowModel
    {
        public int RowId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int FileId { get; set; }
        public int AccountId { get; set; }
        public FileModel? File { get; set; } = new FileModel();
        public AccountModel Account { get; set; } = new AccountModel();
    }
}
