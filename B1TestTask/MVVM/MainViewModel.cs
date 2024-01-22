using B1TestTask.DAL;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel
    {
        public MainViewModel(B1TestTaskDBContext b1TestTaskDBContext)
        {
            b1TestTaskDBContext.Classes.Add(new DAL.Models.ClassModel() { ClassModelId = 5 });
            b1TestTaskDBContext.SaveChanges();
        }
    }
}
