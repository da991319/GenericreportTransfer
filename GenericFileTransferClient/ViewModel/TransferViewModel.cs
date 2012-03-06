using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GenericFileTransferClient.GenericFileTransferService;
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
    public class TransferViewModel : ViewModelBase
    {
        private GenericFileTransferServiceClient serviceClient = new GenericFileTransferServiceClient();

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
            }
        }

        private Transfer _combinedList;

        public Transfer CombinedList
        {
            get { return _combinedList; }
            set { _combinedList = value; }
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
            //ObservableCollection<Report> tempList = new ObservableCollection<Report>(serviceClient.GetAllReports());
            //ReportsFrom = tempList;
            //ReportsTo = tempList;
            Reports = new ObservableCollection<Report>(serviceClient.GetAllReports());
        }

        private List<Column> LoadColumns(int reportId)
        {
            return serviceClient.GetColumnsByReport(reportId);
        }

        private void FillCombinedList()
        {
            throw new System.NotImplementedException();
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}