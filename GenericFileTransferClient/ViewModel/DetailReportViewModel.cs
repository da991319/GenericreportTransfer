﻿using GalaSoft.MvvmLight;
using GenericFileTransferClient.GenericFileTransferService;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;

namespace GenericFileTransferClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class DetailReportViewModel : ViewModelBase
    {
        private GenericFileTransferServiceClient serviceClient = new GenericFileTransferServiceClient();
        private Report _currentReport;

        public Report CurrentReport
        {
            get { return _currentReport; }
            set { _currentReport = value; 
                RaisePropertyChanged("CurrentReport");
                if (Mode.Equals(ViewModel.Mode.Edit) && value != null)
                {
                    LoadColumns();
                }
                
            }
        }

        private ObservableCollection<Report> _reports;

        public ObservableCollection<Report> Reports
        {
            get { return _reports; }

            set
            {
                _reports = value;
                RaisePropertyChanged("Reports");
            }
        }

        //private ObservableCollection<Column> _columns;

        //public ObservableCollection<Column> Columns
        //{
        //    get { return _columns; }
        //}

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged("FilePath"); }
        }
        
        public Mode Mode { get; set; } 

        #region Commands
        private RelayCommand _browseCommand;

        public RelayCommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(ExecuteBrowseCommand, CanExecuteBrowseCommand);
                }
                return _browseCommand;
            }
        }

        private RelayCommand _saveReportCommand;

        public RelayCommand SaveReportCommand
        {
            get {
                if (_saveReportCommand == null)
                {
                    _saveReportCommand = new RelayCommand(ExecuteSaveReportCommand, CanExecuteSaveReportCommand);
                }
                return _saveReportCommand;
            }
            
        }

        private RelayCommand _deleteReportCommand;

        public RelayCommand DeleteReportCommand
        {
            get {
                if (_deleteReportCommand == null)
                {
                    _deleteReportCommand = new RelayCommand(ExecuteDeleteReportCommand, CanExecuteDeleteReportCommand);
                }
                return _deleteReportCommand;
            }
        }
        
        
        #endregion

        #region command implementation
        
        private void ExecuteBrowseCommand()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".csv|*.xls|*.xlsx"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv|Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx"; // Filter files by extension

            // Show open file dialog box
            System.Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FilePath  = dlg.FileName;
            }
        }

        private bool CanExecuteBrowseCommand()
        {
            return true;
        }

        private void ExecuteSaveReportCommand()
        {
            Report tempReport = CurrentReport;
            if (Mode.Equals(ViewModel.Mode.Add))
            {
                //get the list of headers and column number
                Dictionary<int, string> headers = Utils.GetHeaders(FilePath, CurrentReport.Separator,CurrentReport.HeaderRow,CurrentReport.SheetName);

                List<Column> listColumns = new ObservableCollection<Column>(headers.Select(k => new Column
                {
                    Position = k.Key,
                    Header = k.Value,

                })).ToList();

                tempReport.Columns = listColumns;
                tempReport.FileName = Utils.GetFileName(FilePath);
            }
            //save report
            serviceClient.UpsertReport(tempReport);

            //save template
            Utils.SaveTemplateFile(FilePath);
        }

        private bool CanExecuteSaveReportCommand()
        {
            return true;
        }

        private void ExecuteDeleteReportCommand()
        {
            //get file name
            string fileName = CurrentReport.FileName;

            serviceClient.DeleteReport(CurrentReport);
            
            //delete template
            Utils.DeleteTemplate(fileName);

            CurrentReport = null;
            //update combobox datasource
            LoadReports();
        }

        private bool CanExecuteDeleteReportCommand()
        {
            return true;
        }
        #endregion
        /// <summary>
        /// Initializes a new instance of the DetailReportViewModel class.
        /// </summary>
        public DetailReportViewModel(Mode mode)
        {
            Mode = mode;
            if (mode.Equals(ViewModel.Mode.Edit))
            {
                LoadReports();
            }
            else
            {
                CurrentReport = new Report();
            }
        }

        public DetailReportViewModel()
        {}

        private void LoadReports()
        {
            //_reports = new ObservableCollection<Report>(serviceClient.GetAllReports());
            Reports = new ObservableCollection<Report>(serviceClient.GetAllReports());
        }

        private void LoadColumns()
        {
            //Columns = new ObservableCollection<Column>(serviceClient.GetColumnsByReport(CurrentReport.Id));
            CurrentReport.Columns = serviceClient.GetColumnsByReport(CurrentReport.Id);
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}