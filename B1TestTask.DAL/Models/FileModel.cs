namespace B1TestTask.DAL.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public ICollection<RowModel> Rows { get; set; } = new List<RowModel>();
    }
}
