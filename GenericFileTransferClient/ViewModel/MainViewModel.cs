using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GenericFileTransferClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
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
    public class MainViewModel : FormViewModelBase
    {
        readonly static HomeViewModel _homeViewModel = new HomeViewModel();
        readonly DetailReportViewModel _detailReportViewModel = new DetailReportViewModel();
        readonly TransferViewModel _transferViewModel = new TransferViewModel();

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        #region Command
        
        private RelayCommand _homeViewCommand;

        public RelayCommand HomeViewCommand
        {
            get {
                if (_homeViewCommand == null)
                {
                    _homeViewCommand = new RelayCommand(ExecuteHomeViewCommand, CanExecuteHomeViewCommand);
                }
                return _homeViewCommand; }
        }

        private RelayCommand _detailReportViewCommand;

        public RelayCommand DetailReportViewCommand
        {
            get {
                if (_detailReportViewCommand == null)
                {
                    _detailReportViewCommand = new RelayCommand(ExecuteDetailReportViewCommand,CanExecuteDetailReportViewCommand);
                }
                return _detailReportViewCommand; }
        }

        private RelayCommand _editReportViewCommand;

        public RelayCommand EditReportViewCommand
        {
            get
            {
                if (_editReportViewCommand == null)
                {
                    _editReportViewCommand = new RelayCommand(ExecuteEditReportViewCommand, CanExecuteEditReportViewCommand);
                }
                return _editReportViewCommand;
            }
        }

        private RelayCommand _TransferViewCommand;

        public RelayCommand TransferViewCommand
        {
            get
            {
                if (_TransferViewCommand == null)
                {
                    _TransferViewCommand = new RelayCommand(ExecuteTransferViewCommand, CanExecuteTransferViewCommand);
                }
                return _TransferViewCommand;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            CurrentViewModel = new HomeViewModel();
        }

        #region command implementation
        
        private void ExecuteHomeViewCommand()
        {
            CurrentViewModel = _homeViewModel;
        }

        private bool CanExecuteHomeViewCommand()
        {
            return true;
        }

        private void ExecuteDetailReportViewCommand()
        {
            CurrentViewModel = new DetailReportViewModel(Mode.Add);
        }

        private bool CanExecuteDetailReportViewCommand()
        {
            return true;
        }

        private void ExecuteEditReportViewCommand()
        {
            CurrentViewModel = new DetailReportViewModel(Mode.Edit);
        }

        private bool CanExecuteEditReportViewCommand()
        {
            return true;
        }

        private void ExecuteTransferViewCommand()
        {
            CurrentViewModel = _transferViewModel;
        }

        private bool CanExecuteTransferViewCommand()
        {
            return true;
        }

        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}