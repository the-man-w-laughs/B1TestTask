namespace B1TestTask.DALTask2.Models
{
    // Модель группы счетов.
    public class AccountGroupModel
    {
        // Идентификатор группы счетов.
        public int AccountGroupId { get; set; }
        // Название группы счетов.
        public string GroupName { get; set; } = string.Empty; 
        // Идентификатор модели класса, к которой принадлежит группа.
        public int ClassModelId { get; set; }
        // Ссылка на модель класса.
        public ClassModel? ClassModel { get; set; } = null;
        // Коллекция счетов в группе.
        public ICollection<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    }

}
