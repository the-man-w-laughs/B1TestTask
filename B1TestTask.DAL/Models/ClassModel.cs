namespace B1TestTask.DALTask2.Models
{
    public class ClassModel
    {
        public int ClassModelId { get; set; }
        public string ClassName { get; set; } = string.Empty;   
        public ICollection<AccountGroupModel> AccountGroups { get; set; } = new List<AccountGroupModel>();
    }
}
