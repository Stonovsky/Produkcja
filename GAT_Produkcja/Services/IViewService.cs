using System;
using System.Windows;
using GalaSoft.MvvmLight;

namespace GAT_Produkcja.UI.Services
{
    public interface IViewService
    {
        void Close<T>() where T : ViewModelBase;
        void Close(string viewModelName);
        void Show<T>() where T : ViewModelBase;
        void ShowDialog<T>() where T : ViewModelBase;
        void ShowDialog<T>(Action action) where T : ViewModelBase;

    }
}