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

        private ObservableCollection<string> _treeViewItems;
        private ObservableCollection<object> _AccountGridViewItems;

        public RelayCommand LoadExcelCommand { get; }
        public RelayCommand LoadDataFromDBCommand { get; }

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

        public ObservableCollection<string> TreeViewItems
        {
            get { return _treeViewItems; }
            set
            {
                if (_treeViewItems != value)
                {
                    _treeViewItems = value;
                    OnPropertyChanged(nameof(TreeViewItems));
                }
            }
        }

        public ObservableCollection<object> AccountGridViewItems
        {
            get { return _AccountGridViewItems; }
            set
            {
                if (_AccountGridViewItems != value)
                {
                    _AccountGridViewItems = value;
                    OnPropertyChanged(nameof(AccountGridViewItems));
                }
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

            PopulateAccountGridViewItems();
        }

        private void PopulateAccountGridViewItems()
        {            
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

                    SyncStatus = "Ready";
                }
            }
            finally
            {
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

                var files = await _fileModelService.GetAllFiles();

                SyncStatus = "Ready";
            }
            finally
            {
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