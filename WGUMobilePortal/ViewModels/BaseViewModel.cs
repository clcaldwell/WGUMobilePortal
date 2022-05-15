using System;
using System.ComponentModel;

namespace WGUMobilePortal.ViewModels
{
    public class BaseViewModel : MvvmHelpers.BaseViewModel, IDataErrorInfo
    {
        string IDataErrorInfo.Error => throw new NotImplementedException();

        string IDataErrorInfo.this[string columnName] => throw new NotImplementedException();
    }
}