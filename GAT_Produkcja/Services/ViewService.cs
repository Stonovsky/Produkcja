using Autofac;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAT_Produkcja.UI.Services
{
    public class ViewService : IViewService
    {
        private Window view;
        private Type viewModelType;
        private Type viewType;
        private readonly IDialogService dialogService;

        public ViewService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void Show<T>() where T : ViewModelBase
        {
            ConvertViewModelToView<T>();
            //CloseWhenOpened<T>();
            if (IsViewOpened<T>())
            {
                view.Activate();
                return;
            }
            else
            {
                view.Show();
            }
        }

        public void ShowDialog<T>() where T : ViewModelBase
        {
            ConvertViewModelToView<T>();
            if (IsViewOpened<T>() == false)
            {
                view.ShowDialog();
            }
        }

        public void ShowDialog<T>(Action action) where T : ViewModelBase
        {
            ConvertViewModelToView<T>();
            if (IsViewOpened<T>() == false)
            {
                action?.Invoke();
                view.ShowDialog();
            }
        }

        public void Close<T>() where T : ViewModelBase
        {
            ConvertViewModelToView<T>();
            try
            {
                foreach (Window v in Application.Current.Windows)
                {
                    if (v.GetType() == viewType)
                        v.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public void Close(string viewModelName)
        {
            var view = viewModelName.Replace("Model", "");
            try
            {
                foreach (Window v in Application.Current.Windows)
                {
                    if (v.GetType().Name == view)
                        v.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void ConvertViewModelToView<T>() where T : ViewModelBase
        {
            viewModelType = typeof(T);
            viewType = Type.GetType(viewModelType.Namespace + "." + viewModelType.Name.Replace("Model", ""));
        }

        private void ActivateView()
        {
            view = (Window)Activator.CreateInstance(viewType);
        }

        private bool IsViewOpened<T>()
        {
            foreach (Window v in Application.Current.Windows)
            {
                if (v.GetType() == viewType)
                {
                    if (view is null)
                        ActivateView();
                    
                    dialogService.ShowInfo_BtnOK("Okno jest już otwarte.");
                    return true;
                }
            }
            ActivateView();
            return false;
        }

        private void CloseWhenOpened<T>()
        {
            try
            {
                foreach (Window view in Application.Current.Windows)
                {
                    if (view.GetType() == viewType)
                    {
                        view.Close();
                    }

                }
            }
            catch (Exception)
            {
            }
        }
    }
}
