using B1TestTask.BLLTask1.Contracts;
using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IFileModelService _fileModelService;
        private readonly ITrialBalanceParser _trialBalanceParser;
        private readonly ITask1FileService _task1FileService;

        public RelayCommand LoadExcelCommand { get; }
        public RelayCommand LoadDataFromDBCommand { get; }
        public RelayCommand GenerateFilesCommand { get; }
        public RelayCommand GroupFilesCommand { get; }
        public RelayCommand LoadFileToDBCommand { get; }
        public RelayCommand CalculateParametersCommand { get; }

        private ObservableCollection<FileModelDto> _fileModels;
        public ObservableCollection<FileModelDto> FileModels
        {
            get => _fileModels;
            set => SetProperty(ref _fileModels, value, nameof(FileModels));
        }

        private FileModelDto _selectedFile;
        public FileModelDto SelectedFile
        {
            get => _selectedFile;
            set
            {
                SetProperty(ref _selectedFile, value, nameof(SelectedFile));
                UpdateClassGridViewItems();
            }
        }

        private ObservableCollection<ClassModelDto> _classGridViewItems;
        public ObservableCollection<ClassModelDto> ClassGridViewItems
        {
            get => _classGridViewItems;
            set => SetProperty(ref _classGridViewItems, value, nameof(ClassGridViewItems));
        }

        private void UpdateClassGridViewItems()
        {
            if (SelectedFile != null)
            {
                ClassGridViewItems = new ObservableCollection<ClassModelDto>(SelectedFile.FileContent.ClassList);
            }
            else
            {
                ClassGridViewItems = null;
            }
        }

        private bool _isLoadExcelButtonBusy;
        public bool IsLoadExcelButtonBusy
        {
            get => _isLoadExcelButtonBusy;
            set => SetProperty(ref _isLoadExcelButtonBusy, value, nameof(IsLoadExcelButtonBusy));
        }

        private bool _isLoadDataFromDBButtonBusy;
        public bool IsLoadDataFromDBButtonBusy
        {
            get => _isLoadDataFromDBButtonBusy;
            set => SetProperty(ref _isLoadDataFromDBButtonBusy, value, nameof(IsLoadDataFromDBButtonBusy));
        }

        private bool _isGenerateFilesButtonBusy;
        public bool IsGenerateFilesButtonBusy
        {
            get => _isGenerateFilesButtonBusy;
            set => SetProperty(ref _isGenerateFilesButtonBusy, value, nameof(IsGenerateFilesButtonBusy));
        }

        private bool _isGroupFilesButtonBusy;
        public bool IsGroupFilesButtonBusy
        {
            get => _isGroupFilesButtonBusy;
            set => SetProperty(ref _isGroupFilesButtonBusy, value, nameof(IsGroupFilesButtonBusy));
        }

        private bool _isLoadFileToDBButtonBusy;
        public bool IsLoadFileToDBButtonBusy
        {
            get => _isLoadFileToDBButtonBusy;
            set => SetProperty(ref _isLoadFileToDBButtonBusy, value, nameof(IsLoadFileToDBButtonBusy));
        }

        private bool _isCalculateSumAndMedianButtonBusy;
        public bool IsCalculateSumAndMedianButtonBusy
        {
            get => _isCalculateSumAndMedianButtonBusy;
            set => SetProperty(ref _isCalculateSumAndMedianButtonBusy, value, nameof(IsCalculateSumAndMedianButtonBusy));
        }

        private string _syncStatus = "Готов";
        public string SyncStatus
        {
            get => _syncStatus;
            set => SetProperty(ref _syncStatus, value, nameof(SyncStatus));
        }

        private string _substringInput;
        public string SubstringInput
        {
            get => _substringInput;
            set => SetProperty(ref _substringInput, value, nameof(SubstringInput));
        }

        private string _sumResult;
        public string SumResult
        {
            get => _sumResult;
            set => SetProperty(ref _sumResult, value, nameof(SumResult));
        }

        private string _medianResult;
        public string MedianResult
        {
            get => _medianResult;
            set => SetProperty(ref _medianResult, value, nameof(MedianResult));
        }

        public MainViewModel(IFileModelService fileModelService, ITrialBalanceParser trialBalanceParser, ITask1FileService task1FileService)
        {
            LoadExcelCommand = new RelayCommand(async parameter => await OpenExcelAsync(), (_) => !IsLoadExcelButtonBusy);
            LoadDataFromDBCommand = new RelayCommand(async parameter => await LoadDataFromDBAsync(), (_) => !IsLoadDataFromDBButtonBusy);
            GenerateFilesCommand = new RelayCommand(async parameter => await GenerateFilesAsync(), (_) => !IsGenerateFilesButtonBusy);
            GroupFilesCommand = new RelayCommand(async parameter => await GroupFilesAsync(), (_) => !IsGroupFilesButtonBusy);
            LoadFileToDBCommand = new RelayCommand(async parameter => await LoadFileToDBAsync(), (_) => !IsLoadFileToDBButtonBusy);
            CalculateParametersCommand = new RelayCommand(async parameter => await CalculateMedialAndSumAsync(), (_) => !IsCalculateSumAndMedianButtonBusy);

            _fileModelService = fileModelService;
            _trialBalanceParser = trialBalanceParser;
            _task1FileService = task1FileService;
        }

        private async Task CalculateMedialAndSumAsync()
        {
            var result = await _task1FileService.CalculateSumAndMedianAsync();

            SumResult = result.Item1.ToString("F2");
            MedianResult = result.Item2.ToString("F2"); 
        }

        private async Task OpenExcelAsync()
        {
            if (IsLoadExcelButtonBusy)
                return;

            try
            {
                IsLoadExcelButtonBusy = true;

                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

                if (openFileDialog.ShowDialog() == true)
                {
                    SyncStatus = "Извлечение информации...";
                    string filePath = openFileDialog.FileName;
                    var fileContentDto = _trialBalanceParser.ParseTrialBalance(filePath);

                    SyncStatus = "Добавление файла в базу данных...";

                    var newFileModelDto = new FileModelDto()
                    {
                        FileName = filePath,
                        FileContent = fileContentDto
                    };

                    await _fileModelService.AddFile(newFileModelDto);

                }
            }
            finally
            {
                SyncStatus = "Готов";
                IsLoadExcelButtonBusy = false;            
                CommandManager.InvalidateRequerySuggested();
            }
        }        

        private async Task LoadDataFromDBAsync()
        {
            if (IsLoadDataFromDBButtonBusy) return;

            try
            {
                IsLoadDataFromDBButtonBusy = true;
                SyncStatus = "Загрузка...";

                var fileModelDtos = await _fileModelService.GetAllFiles();

                FileModels = new ObservableCollection<FileModelDto>(fileModelDtos);

            }
            finally
            {
                SyncStatus = "Готов";
                IsLoadDataFromDBButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
            
        }

        private async Task GenerateFilesAsync()
        {
            if (IsGenerateFilesButtonBusy) return;

            try
            {
                IsGenerateFilesButtonBusy = true;
                SyncStatus = "Генерация файлов...";

                var openFileDialog = new CommonOpenFileDialog();
                openFileDialog.IsFolderPicker = true;

                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {                    
                    string directoryPath = openFileDialog.FileName;
                    await Task.Run(() => _task1FileService.GenerateTextFiles(directoryPath));
                }
            }
            finally
            {
                SyncStatus = "Готов";
                IsGenerateFilesButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }


        private async Task LoadFileToDBAsync()
        {           
            if (IsLoadFileToDBButtonBusy)
                return;

            try
            {
                IsLoadFileToDBButtonBusy = true;

                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files|*.txt;";

                if (openFileDialog.ShowDialog() == true)
                {                    
                    SyncStatus = "Добавление файла в базу данных...";

                    var progress = new Progress<(int, int)>((totals) =>
                    {
                        SyncStatus = $"Загружено {totals.Item1} / Осталось {totals.Item2}";
                    });

                    string filePath = openFileDialog.FileName;
                    await _task1FileService.ImportDataToDatabaseAsync(filePath, progress);
                }
            }
            finally
            {
                SyncStatus = "Готов";
                IsLoadFileToDBButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private async Task GroupFilesAsync()
        {
            if (IsGroupFilesButtonBusy) return;

            try
            {
                IsGroupFilesButtonBusy = true;
                SyncStatus = "Выбор директории...";

                var inputDialog = new CommonOpenFileDialog();
                inputDialog.IsFolderPicker = true;

                if (inputDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string inputDirectory = inputDialog.FileName;

                    SyncStatus = "Выбор директории...";

                    var outputDialog = new CommonOpenFileDialog();
                    outputDialog.IsFolderPicker = true;

                    if (outputDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        string outputDirectory = outputDialog.FileName;

                        SyncStatus = "Объединение файлов...";
                        var totalLinesDeleted = await Task.Run(() => _task1FileService.CombineAndRemoveLines(inputDirectory, outputDirectory, SubstringInput));
                        MessageBox.Show($"Всего строк '{SubstringInput}' удалено: {totalLinesDeleted}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            finally
            {
                SyncStatus = "Готов";
                IsGroupFilesButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}