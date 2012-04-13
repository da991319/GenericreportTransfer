using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GenericFileTransferClient.GenericFileTransferService;
using GenericFileTransferClient.Model;
using GalaSoft.MvvmLight.Command;

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
    public class TransferViewModel : ViewModelBase
    {
        private GenericFileTransferServiceClient serviceClient = new GenericFileTransferServiceClient();

        private List<Transfer> _transferViewModelList;

        private ObservableCollection<Report> _reportsFrom;

        public ObservableCollection<Report> ReportsFrom
        {
            get { return _reportsFrom; }

            set
            {
                _reportsFrom = value;
                RaisePropertyChanged("ReportsFrom");
            }
        }

        private ObservableCollection<Report> _reportsTo;

        public ObservableCollection<Report> ReportsTo
        {
            get { return _reportsTo; }

            set
            {
                _reportsTo = value;
                RaisePropertyChanged("Reports");
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

        private Report _selectedReportFrom;

        public Report SelectedReportFrom
        {
            get { return _selectedReportFrom; }
            set { _selectedReportFrom = value;
                _selectedReportFrom.Columns = LoadColumns(_selectedReportFrom.Id);
                //check for combined List
                FillCombinedList();
                RaisePropertyChanged("SelectedReportFrom");
            }
        }

        private Report _selectReportTo;

        public Report SelectedReportTo
        {
            get { return _selectReportTo; }
            set { _selectReportTo = value;
                _selectReportTo.Columns = LoadColumns(_selectReportTo.Id);
                //check for combined List
                FillCombinedList();
                RaisePropertyChanged("SelectedReportTo");
            }
        }

        private ObservableCollection<TransferModel> _listMappingFrom;

        public ObservableCollection<TransferModel> ListMappingFrom
        {
            get { return _listMappingFrom; }
            set { _listMappingFrom = value; RaisePropertyChanged("ListMappingFrom"); }
        }

        private ObservableCollection<TransferModel> _listMappingTo;

        public ObservableCollection<TransferModel> ListMappingTo
        {
            get { return _listMappingTo; }
            set { _listMappingTo = value; RaisePropertyChanged("ListMappingTo"); }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged("FilePath"); }
        }

        //private int[] _columnNumbers;

        //public int[] ColumnNumbers
        //{
        //    get { return _columnNumbers; }
        //    set { _columnNumbers = value; }
        //}
        

        private RelayCommand _saveMappings;

        public RelayCommand  SaveMappings
        {
            get
            {
                if (_saveMappings == null)
                {
                    _saveMappings = new RelayCommand(ExecuteSaveMappingsCommand, CanExecuteSaveMappingsCommand);
                }
                return _saveMappings;
            }
        }

        private RelayCommand _executeTransfer;

        public RelayCommand ExecuteTransfer
        {
            get
            {
                if (_executeTransfer == null)
                {
                    _executeTransfer = new RelayCommand(ExecuteTransferCommand, CanExecuteTransferCommand);
                }
                return _executeTransfer;
            }
        }

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
        
        private void ExecuteTransferCommand()
        {
            Utils.ExecuteTransfer(FilePath, SelectedReportFrom, SelectedReportTo, ListMappingFrom.ToList());//, ListMappingTo.Select(m => m.ColumnId).ToList());
        }

        private bool CanExecuteTransferCommand()
        {
            return true;
        }

        private void ExecuteSaveMappingsCommand()
        {
            foreach (TransferModel tm in ListMappingFrom)
            {
                if (_transferViewModelList.Exists(t => t.ColumnFromId.Equals(tm.ColumnId)))
                {
                    //update columntoId
                    _transferViewModelList.Where(t => t.ColumnFromId.Equals(tm.ColumnId)).FirstOrDefault()
                        .columnToId = ListMappingTo.Where(m => m.Position.Equals(tm.Position)).FirstOrDefault().ColumnId;
                }
                else
                {
                    //create
                    _transferViewModelList.Add(new Transfer()
                    {
                        ColumnFromId = tm.ColumnId,
                        columnToId = ListMappingTo.Where(m => m.Position.Equals(tm.Position)).FirstOrDefault().ColumnId,
                        ReportFromId = SelectedReportFrom.Id,
                        ReportToId = SelectedReportTo.Id
                    });
                }
            }
            //foreach (TransferModel tm in ListMappingTo)
            //{
            //    _transferViewModelList.Where(t => t.columnToId.Equals(tm.ColumnId)).FirstOrDefault()
            //        .ColumnFromId = ListMappingFrom.Where(m => m.Position.Equals(tm.Position)).FirstOrDefault().ColumnId;
               
            //}

            serviceClient.UpsertTransfer(_transferViewModelList);
        }

        private bool CanExecuteSaveMappingsCommand()
        {
            return true;
        }

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
                FilePath = dlg.FileName;
            }
        }

        private bool CanExecuteBrowseCommand()
        {
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the TransferViewModel class.
        /// </summary>
        public TransferViewModel()
        {
            LoadReports();            
        }

        private void LoadReports()
        {
            Reports = new ObservableCollection<Report>(serviceClient.GetAllReports());
        }

        private List<Column> LoadColumns(int reportId)
        {
            return serviceClient.GetColumnsByReport(reportId); 
        }

        private void FillCombinedList()
        {
            if (SelectedReportFrom != null && SelectedReportTo != null)
            {
                _transferViewModelList = serviceClient.GetTransferMapping(SelectedReportFrom.Id, SelectedReportTo.Id);

                if (_transferViewModelList.Count != 0)
                {
                    ListMappingFrom = new ObservableCollection<TransferModel>(_transferViewModelList.Select(t => new TransferModel
                    {
                        Position = SelectedReportTo.Columns.Where(c => c.Id.Equals(t.columnToId)).FirstOrDefault().Position,
                        ColumnId = t.ColumnFromId
                    }).ToList());

                    //ListMappingTo = new ObservableCollection<TransferModel>(_transferViewModelList.Select(t => new TransferModel
                    //{
                    //    Position = SelectedReportTo.Columns.Where(c => c.Id.Equals(t.columnToId)).FirstOrDefault().Position,
                    //    ColumnId = t.columnToId
                    //}).ToList());
                }
                else
                {
                    int defaultPosition = SelectedReportFrom.Columns.Where(cTo => cTo.Position.Equals(-1)).FirstOrDefault().Position;
                    //we don't want the empty row to be displayed as a row
                    ListMappingFrom = new ObservableCollection<TransferModel>(SelectedReportFrom.Columns.Where(c => !c.Position.Equals(-1))
                        .Select(c => new TransferModel
                    {
                        Position = defaultPosition,
                        ColumnId = c.Id
                    }).ToList());
                }

                ListMappingTo = new ObservableCollection<TransferModel>(SelectedReportTo.Columns
                    .Select(c => new TransferModel
                {
                    Position = c.Position,
                    ColumnId = c.Id
                }).ToList());
                

                //set the array of column number of the target file
                //ColumnNumbers = ListMappingTo.Select(tm => tm.Position).ToArray();
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}