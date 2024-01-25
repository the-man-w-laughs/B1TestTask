namespace B1TestTask.DALTask2.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public FileContent FileContent { get; set; } = new FileContent();
        public ICollection<RowModel> Rows { get; set; } = new List<RowModel>();
    }
}
