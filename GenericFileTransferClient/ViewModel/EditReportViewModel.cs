using GalaSoft.MvvmLight;
using GenericFileTransferClient.GenericFileTransferService;
using System.Collections.ObjectModel;

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
    public class EditReportViewModel : ViewModelBase
    {
        private GenericFileTransferServiceClient serviceClient = new GenericFileTransferServiceClient();
        private Report _currentReport;

        public Report CurrentReport
        {
            get { return _currentReport; }
            set { _currentReport = value; RaisePropertyChanged("CurrentReport"); }
        }

        private ObservableCollection<Report> _reports;

        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
        }

        //set the mode of the detail page
        public Mode Mode { get; set; }

        /// <summary>
        /// Initializes a new instance of the EditReportViewModel class.
        /// </summary>
        public EditReportViewModel(Mode mode)
        {
            Mode = mode;
            if (mode.Equals(ViewModel.Mode.Edit))
            {
                LoadReports();
            }
            
        }

        public EditReportViewModel()
        {
        }

        private void LoadReports()
        {
            _reports = new ObservableCollection<Report>(serviceClient.GetAllReports());
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}