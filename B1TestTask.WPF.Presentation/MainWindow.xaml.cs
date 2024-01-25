using B1TestTask.Presentation.MVVM;
using System.Windows;

namespace B1TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {            
            DataContext = mainViewModel;            
            InitializeComponent();            
        }
    }
}
