using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using TestingSample.Models;
using TestingSample.Services;

namespace TestingSample.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        // DependencyService: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/introduction
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            // First checking if the current property is the same, if so then
            // there's no need to proceed. backingStore is just the current value, 
            // while value is the new value.
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            // onChanged is just an optional delegate. It's not getting used in this file,
            // but since BaseViewModel is inherited in other classes it's feasible to have 
            // extra properties where you may want something to happen when it changes, in
            // that case it can just be passed in to SetProperty as the third parameter.
            onChanged?.Invoke(); // Or just onChanged();
            OnPropertyChanged(propertyName);
            return true;
        }

        // INotifyPropertyChanged is what will let the View that is bound to this instance 
        // know that there's a new value.
        // #region: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-region
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Or alternatively: PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
