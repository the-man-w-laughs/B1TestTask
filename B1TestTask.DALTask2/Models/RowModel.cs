namespace B1TestTask.DALTask2.Models
{
    // Класс, представляющий модель строки данных
    public class RowModel
    {
        // Уникальный идентификатор строки
        public int RowId { get; set; }

        // Содержимое строки
        public RowContent Content { get; set; } = new RowContent();

        // Идентификатор файла, к которому принадлежит строка
        public int FileId { get; set; }

        // Идентификатор учетной записи, к которой принадлежит строка
        public int AccountId { get; set; }

        // Связь с файлом
        public FileModel? File { get; set; } = null;

        // Связь с учетной записью
        public AccountModel? Account { get; set; } = null;
    }
}
