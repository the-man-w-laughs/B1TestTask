namespace B1TestTask.BLL.Dtos
{
    public class ClassModelDto
    {        
        public string ClassName { get; set; } = string.Empty;   
        public ICollection<AccountGroupModelDto> AccountGroups { get; set; } = new List<AccountGroupModelDto>();
    }
}
