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
        private int _columnId;

        public int ColumnId
        {
            get { return _columnId; }
            set { _columnId = value; OnPropertyChanged("ColumnId"); }
        }

        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged("Position"); }
        }
        
        public TransferModel()
        {

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
