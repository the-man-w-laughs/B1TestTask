using B1TestTask.DALTask2;
using Microsoft.Win32;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel
    {
        public RelayCommand OpenExcelCommand { get; }
        public MainViewModel(B1TestTask2DBContext b1TestTaskDBContext)
        {
            b1TestTaskDBContext.Classes.Add(new DALTask2.Models.ClassModel() { ClassModelId = 5 });
            b1TestTaskDBContext.SaveChanges();
            OpenExcelCommand = new RelayCommand(OpenExcel);
        }        

        private void OpenExcel(object parameter)
        {            
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;                
            }
        }
    }
}
