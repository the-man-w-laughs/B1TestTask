namespace B1TestTask.DALTask2.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;        
        public int AccountGroupId { get; set; }
        public AccountGroupModel? AccountGroup { get; set; } = null;
        public ICollection<RowModel> Rows { get; set; } = new List<RowModel>();
    }
}
