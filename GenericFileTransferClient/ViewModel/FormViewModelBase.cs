using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericFileTransferClient.ViewModel
{
    public abstract class FormViewModelBase : ValidationViewModelBase
    {
        private bool isValid;

        /// <summary>
        /// Gets a value indicating whether the form is valid in its current state. If all properties
        /// wich validation are valid, this property returns true.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
            protected set
            {
                this.isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            this.PropertyChangedCompleted(propertyName);
        }

        protected void PropertyChangedCompleted(string propertyName)
        {
            // test prevent infinite loop while settings IsValid 
            // (which causes an PropertyChanged to be raised)
            if (propertyName != "IsValid")
            {
                // update the isValid status
                if (string.IsNullOrEmpty(this.Error) && this.ValidPropertiesCount == this.TotalPropertiesWithValidationCount)
                {
                    this.IsValid = true;
                }
                else
                {
                    this.IsValid = false;
                }
            }
        }
        
    }
}
