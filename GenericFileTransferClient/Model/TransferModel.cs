using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GenericFileTransferClient.GenericFileTransferService;

namespace GenericFileTransferClient.Model
{
    public class TransferModel : INotifyPropertyChanged
    {
        private List<Column> _columnsFrom;

        public List<Column> ColumnsFrom
        {
            get { return _columnsFrom; }
            set { _columnsFrom = value; }
        }

        private List<Column> _columnsTo;

        public List<Column> ColumnsTo
        {
            get { return _columnsTo; }
            set { _columnsTo = value; }
        }
        


        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Members

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Members
    }
}
