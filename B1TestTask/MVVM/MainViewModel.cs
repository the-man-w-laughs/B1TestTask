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

        private ObservableCollection<FileModelDto> _fileModels;
        public ObservableCollection<FileModelDto> FileModels
        {
            get { return _fileModels; }
            set
            {
                if (_fileModels != value)
                {
                    _fileModels = value;
                    OnPropertyChanged(nameof(FileModels));
                }
            }
        }

        private FileModelDto _selectedFile;
        public FileModelDto SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    OnPropertyChanged(nameof(SelectedFile));
                    UpdateClassGridViewItems();
                }
            }
        }

        private ObservableCollection<ClassModelDto> _ClassGridViewItems;
        public ObservableCollection<ClassModelDto> ClassGridViewItems
        {
            get { return _ClassGridViewItems; }
            set
            {
                if (_ClassGridViewItems != value)
                {
                    _ClassGridViewItems = value;
                    OnPropertyChanged(nameof(ClassGridViewItems));
                }
            }
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
            get { return _isLoadExcelButtonBusy; }
            set
            {
                _isLoadExcelButtonBusy = value;
                OnPropertyChanged(nameof(IsLoadExcelButtonBusy));
            }
        }

        private bool _isLoadDataFromDBButtonBusy;

        public bool IsLoadDataFromDBButtonBusy
        {
            get { return _isLoadDataFromDBButtonBusy; }
            set
            {
                _isLoadDataFromDBButtonBusy = value;
                OnPropertyChanged(nameof(IsLoadDataFromDBButtonBusy));
            }
        }

        private string _syncStatus = "Ready";
        public string SyncStatus
        {
            get { return _syncStatus; }
            set
            {
                if (_syncStatus != value)
                {
                    _syncStatus = value;
                    OnPropertyChanged(nameof(SyncStatus));
                }
            }
        }

        public MainViewModel(IFileModelService fileModelService, ITrialBalanceParser trialBalanceParser)
        {
            LoadExcelCommand = new RelayCommand(async parameter => await OpenExcelAsync(), (_) => !IsLoadExcelButtonBusy);
            LoadDataFromDBCommand = new RelayCommand(async parameter => await LoadDataFromDBAsync(), (_) => !IsLoadDataFromDBButtonBusy);

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
        

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}