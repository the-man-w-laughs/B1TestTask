namespace B1TestTask.BLLTask2.Dtos
{
    public class ClassModelDto
    {        
        public string ClassName { get; set; } = string.Empty;   
        public ICollection<AccountGroupModelDto> AccountGroups { get; set; } = new List<AccountGroupModelDto>();
    }
}
