using B1TestTask.DAL;
using Microsoft.Win32;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel
    {
        public RelayCommand OpenExcelCommand { get; }
        public MainViewModel(B1TestTaskDBContext b1TestTaskDBContext)
        {
            b1TestTaskDBContext.Classes.Add(new DAL.Models.ClassModel() { ClassModelId = 5 });
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
