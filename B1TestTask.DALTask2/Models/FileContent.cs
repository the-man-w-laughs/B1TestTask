namespace B1TestTask.DALTask2.Models
{
    // Класс, представляющий информацию о содержимом файла.
    public class FileContent    
    {
        // Название банка.
        public string BankName { get; set; } = string.Empty;
        // Название файла.
        public string FileTitle { get; set; } = string.Empty;
        // Период, к которому относится содержимое файла.
        public string Period { get; set; } = string.Empty;
        // Дополнительная информация.
        public string AdditionalInfo { get; set; } = string.Empty;
        // Дата генерации файла.
        public DateTime GenerationDate { get; set; }
        // Валюта файла.
        public string Currency { get; set; } = string.Empty; 
    }
}
