namespace B1TestTask.DALTask2.Models
{
    // Модель класса
    public class ClassModel
    {
        // Идентификатор класса.
        public int ClassModelId { get; set; }
        // Название класса.
        public string ClassName { get; set; } = string.Empty;
        // Коллекция групп счетов, принадлежащих классу.
        public ICollection<AccountGroupModel> AccountGroups { get; set; } = new List<AccountGroupModel>(); 
    }

}
