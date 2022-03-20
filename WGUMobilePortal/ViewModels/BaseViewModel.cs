using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WGUMobilePortal.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private bool isBusy = false;

        private string title = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        string IDataErrorInfo.Error => throw new NotImplementedException();

        //public DBService<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        string IDataErrorInfo.this[string columnName] => throw new NotImplementedException();

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)

        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(ref T member, T val,
                                                    [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}