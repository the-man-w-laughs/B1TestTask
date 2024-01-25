using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using B1TestTask.BLLTask2.Services.Database;
using B1TestTask.DALTask2;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace B1TestTask.Presentation.MVVM
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IFileModelService _fileModelService;
        private readonly ITrialBalanceParser _trialBalanceParser;        

        public RelayCommand LoadExcelCommand { get; }
        public RelayCommand LoadDataFromDBCommand { get; }
        public RelayCommand GenerateFilesCommand { get; }
        public RelayCommand GroupFilesCommand { get; }
        public RelayCommand LoadFileToDBCommand { get; }

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

        private string _syncStatus = "Ready";
        public string SyncStatus
        {
            get => _syncStatus;
            set => SetProperty(ref _syncStatus, value, nameof(SyncStatus));
        }

        public MainViewModel(IFileModelService fileModelService, ITrialBalanceParser trialBalanceParser)
        {
            LoadExcelCommand = new RelayCommand(async parameter => await OpenExcelAsync(), (_) => !IsLoadExcelButtonBusy);
            LoadDataFromDBCommand = new RelayCommand(async parameter => await LoadDataFromDBAsync(), (_) => !IsLoadDataFromDBButtonBusy);
            GenerateFilesCommand = new RelayCommand(async parameter => await GenerateFilesAsync(), (_) => !IsGenerateFilesButtonBusy);
            GroupFilesCommand = new RelayCommand(async parameter => await GroupFilesAsync(), (_) => !IsGroupFilesButtonBusy);
            LoadFileToDBCommand = new RelayCommand(async parameter => await LoadFileToDBAsync(), (_) => !IsLoadFileToDBButtonBusy);

            _fileModelService = fileModelService;
            _trialBalanceParser = trialBalanceParser;
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
                    SyncStatus = "Parsing...";
                    string filePath = openFileDialog.FileName;
                    var fileContentDto = _trialBalanceParser.ParseTrialBalance(filePath);

                    SyncStatus = "Adding file to database...";

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
                SyncStatus = "Ready";
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
                SyncStatus = "Loading...";

                var fileModelDtos = await _fileModelService.GetAllFiles();

                FileModels = new ObservableCollection<FileModelDto>(fileModelDtos);

            }
            finally
            {
                SyncStatus = "Ready";
                IsLoadDataFromDBButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
            
        }

        private async Task GenerateFilesAsync()
        {
            if (IsGenerateFilesButtonBusy) return;

            try
            {
                IsLoadDataFromDBButtonBusy = true;
                SyncStatus = "Generating files...";
            }
            finally
            {
                SyncStatus = "Ready";
                IsGenerateFilesButtonBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }

        }

        private async Task LoadFileToDBAsync()
        {
            throw new NotImplementedException();
        }

        private async Task GroupFilesAsync()
        {
            throw new NotImplementedException();
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