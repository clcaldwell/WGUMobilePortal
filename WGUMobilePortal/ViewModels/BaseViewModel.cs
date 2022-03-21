using System;
using System.ComponentModel;

namespace WGUMobilePortal.ViewModels
{
    public class BaseViewModel : MvvmHelpers.BaseViewModel, IDataErrorInfo
    {
        //private bool isBusy = false;

        //private string title = string.Empty;

        //public event PropertyChangedEventHandler? PropertyChanged;

        string IDataErrorInfo.Error => throw new NotImplementedException();

        //public bool IsBusy
        //{
        //    get => isBusy;
        //    set => SetProperty(ref isBusy, value);
        //}

        //public string Title
        //{
        //    get => title;
        //    set => SetProperty(ref title, value);
        //}

        string IDataErrorInfo.this[string columnName] => throw new NotImplementedException();

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this,
        //        new PropertyChangedEventArgs(propertyName));
        //}

        //protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        //{
        //    if (object.Equals(member, val)) return;
        //    member = val;
        //    OnPropertyChanged(propertyName);
        //    OnPropertyChanged(nameof(member));
        //    var nameOf = nameof(member);
        //    _ = propertyName;
        //    _ = nameOf;
        //}
    }
}