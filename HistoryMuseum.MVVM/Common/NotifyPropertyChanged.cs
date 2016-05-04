using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HistoryMuseum.MVVM.Common
{
    public class NotifyPropertyChanged : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged 成员
        public event PropertyChangedEventHandler PropertyChanged;



        virtual internal protected void OnPropertyChanged(string propertyName)
        {

            if (this.PropertyChanged != null)
            {

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }
        #endregion

        #region IDataErrorInfo 成员

        private string _dataError = string.Empty;
        private Dictionary<string, string> _dataErrors = new Dictionary<string, string>();

        public string Error
        {
            get { return _dataError; }
        }

        public string this[string columnName]
        {
            get
            {
                if (_dataErrors.ContainsKey(columnName))
                    return _dataErrors[columnName];
                else
                    return null;
            }
        }

        #endregion

        public void AddError(string name, string error)
        {
            _dataErrors[name] = error;
            this.OnPropertyChanged(name);
        }

        public void RemoveError(string name)
        {
            if (_dataErrors.ContainsKey(name))
            {
                _dataErrors.Remove(name);
                this.OnPropertyChanged(name);
            }
        }

        public void ClearError()
        {
            var keys = new string[_dataErrors.Count];
            _dataErrors.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                this.RemoveError(key);
            }
        }
    }
}
