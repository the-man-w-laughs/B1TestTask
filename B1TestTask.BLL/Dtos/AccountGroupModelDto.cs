namespace B1TestTask.BLLTask2.Dtos
{
    public class AccountGroupModelDto
    {        
        public string GroupName { get; set; } = string.Empty;                     
        public List<RowContentDto> Rows { get; set; } = new List<RowContentDto>();
    }
}
