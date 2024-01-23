namespace B1TestTask.DALTask2.Models
{
    public class AccountGroupModel
    {
        public int AccountGroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;     
        public int ClassModelId { get; set; }
        public ClassModel ClassModel { get; set; } = new ClassModel();
        public ICollection<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    }
}
