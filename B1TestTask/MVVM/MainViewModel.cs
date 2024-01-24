using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using B1TestTask.BLLTask2.Services.Database;
using B1TestTask.DALTask2;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel
    {
        private readonly IFileModelService _fileModelService;
        private readonly ITrialBalanceParser _trialBalanceParser;

        public RelayCommand OpenExcelCommand { get; }

        public MainViewModel(IFileModelService fileModelService, ITrialBalanceParser trialBalanceParser)
        {
            OpenExcelCommand = new RelayCommand(async parameter => await OpenExcelAsync());

            _fileModelService = fileModelService;
            _trialBalanceParser = trialBalanceParser;
        }

        private async Task OpenExcelAsync()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var fileContentDto = _trialBalanceParser.ParseTrialBalance(filePath);

                var newFileModelDto = new FileModelDto()
                {
                    FileName = filePath,
                    FileContent = fileContentDto
                };

                await _fileModelService.AddFile(newFileModelDto);
            }
        }
    }

}
