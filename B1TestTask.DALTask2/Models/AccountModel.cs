namespace B1TestTask.DALTask2.Models
{
    // Модель счета.
    public class AccountModel
    {
        // Идентификатор счета.
        public int AccountId { get; set; }
        // Номер счета.
        public string AccountNumber { get; set; } = string.Empty;
        // Идентификатор группы счетов, к которой принадлежит счет.
        public int AccountGroupId { get; set; }
        // Ссылка на модель группы счетов.
        public AccountGroupModel? AccountGroup { get; set; } = null;
        // Коллекция строк, принадлежащих счету.
        public ICollection<RowModel> Rows { get; set; } = new List<RowModel>(); 
    }

}
