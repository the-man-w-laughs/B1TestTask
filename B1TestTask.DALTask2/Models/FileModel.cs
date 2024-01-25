namespace B1TestTask.DALTask2.Models
{
    // Класс, представляющий модель файла с его содержимым и строками.
    public class FileModel
    {
        // Идентификатор файла
        public int FileId { get; set; }

        // Имя файла
        public string FileName { get; set; } = string.Empty;

        // Содержимое файла
        public FileContent FileContent { get; set; } = new FileContent();

        // Строки файла
        public ICollection<RowModel> Rows { get; set; } = new List<RowModel>();
    }
}
