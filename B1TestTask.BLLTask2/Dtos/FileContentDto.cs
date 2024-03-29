﻿namespace B1TestTask.BLLTask2.Dtos
{
    // Класс DTO для содержимого файла
    public class FileContentDto
    {
        public string BankName { get; set; } = string.Empty;
        public string FileTitle { get; set; } = string.Empty; 
        public string Period { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public DateTime GenerationDate { get; set; }
        public string Currency { get; set; } = string.Empty;
        public List<ClassModelDto> ClassList { get; set; } = new List<ClassModelDto>();
    }
}
