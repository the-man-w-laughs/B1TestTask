namespace B1TestTask.BLLTask2.Dtos
{
    public class FileModelDto
    {        
        public string FileName { get; set; } = string.Empty;
        public FileContentDto FileContent { get; set; } = new FileContentDto();        
    }
}
