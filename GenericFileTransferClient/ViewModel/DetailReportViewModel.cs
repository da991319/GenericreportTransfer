using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GenericFileTransferClient.GenericFileTransferService;

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
    public class DetailReportViewModel : FormViewModelBase
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
                    FilePath = CurrentReport.FileName;
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

        private string _filePath;

        [Required(AllowEmptyStrings= false,ErrorMessage="Please Specify a File Path")]
        [RegularExpression(@".+\.(CSV|XLS|XLSX|csv|xls|xlsx)$", ErrorMessage="The file must have a CSV, XLS or XLSX extension")]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; 
                RaisePropertyChanged("FilePath");
                //set the selected type of file based on name
                if (System.Text.RegularExpressions.Regex.IsMatch(FilePath, @"\.xlsx$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    SelectedButton = ListButtons.XLSX;
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(FilePath, @"\.csv$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    SelectedButton = ListButtons.CSV;
                }
                else
                {
                    SelectedButton = ListButtons.XLS;
                }
            }
        }
        
        public Mode Mode { get; set; }

        private ListButtons _selectedButton;

        public ListButtons SelectedButton
        {
            get { return _selectedButton; }

            set { _selectedButton = value; RaisePropertyChanged("SelectedButton"); }
        }

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

                //always add an empty column
                listColumns.Add(new Column() { Position = -1, Header = "N/A" });

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
            return this.IsValid;
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
        {
                    
        }

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