using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GenericFileTransfer.Model;

namespace GenericFileTransfer.ViewModel
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
    public class ReportViewModel : ViewModelBase
    {
        private ObservableCollection<Report> _reports = new ObservableCollection<Report>();
        private Report _currentReport;

        /// <summary>
        /// Initializes a new instance of the ReportViewModel class.
        /// </summary>
        public ReportViewModel()
        {
            _reports = new ObservableCollection<Report>(BusinessLayer.GetAllReports());
        }


        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
        }

        public Report CurrentReport
        {
            get { return _currentReport; }
            set { _currentReport = value; RaisePropertyChanged("CurrentReport"); }
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}