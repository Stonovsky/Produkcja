using Autofac;
using DocumentFormat.OpenXml.Wordprocessing;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntityValidation;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.Startup;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Diagnostics;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    [AddINotifyPropertyChangedInterface]

    public abstract class ListAddEditDeleteMethodGenericViewModelBase<T> : ViewModelServiceBase
                    where T: ValidationBase
    {
        private T elementToReload;

        public virtual ObservableCollection<T> ListOfVMEntities { get; set; } = (ObservableCollection<T>)Activator.CreateInstance(typeof(ObservableCollection<T>));
        public virtual ObservableCollection<T> ListOfVMEntitiesBase{ get; set; } = (ObservableCollection<T>)Activator.CreateInstance(typeof(ObservableCollection<T>));
        public T SelectedVMEntity { get; set; } //= Activator.CreateInstance<T>();

        public abstract string Title { get; }

        public abstract IGenericRepository<T> Repository { get; }

        public virtual bool IsChanged => !ListOfVMEntities.CompareWithList(ListOfVMEntitiesBase);

        #region Commands
        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public virtual bool IsValid => ListOfVMEntities.Where(e => e.IsValid == false).Count()==0;
        #endregion

        #region CTOR
        public ListAddEditDeleteMethodGenericViewModelBase(IViewModelService viewModelService) 
            : base(viewModelService)
        {
            AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
            EditCommand = new RelayCommand(EditCommandExecute, EditCommandCanExecute);
            DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);

            MessengerRegistration();

        }
        protected virtual void MessengerRegistration()
        {
            Messenger.Register<T>(this, nameof(RefreshListMessage), OnElementSentRefreshList);
        }

        #endregion

        #region IsChanged_False
        public virtual void IsChanged_False()
        {
            ListOfVMEntitiesBase = ListOfVMEntities.DeepClone();
        }
        #endregion

        #region Messenger

        /// <summary>
        /// Delegat wskazujacy na id elementu, gdy przeslano messengerem
        /// </summary>
        public abstract Func<T, int> GetElementId { get; }
        
        /// <summary>
        /// Metoda odswiezajaca zestawienie gdy przesalno element.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual async void OnElementSentRefreshList(T obj)
        {
            elementToReload = obj;
            await RefreshList(elementToReload);
        }

        /// <summary>
        /// Metoda odswiezajaca liste
        /// </summary>
        /// <param name="elementToReload"></param>
        /// <returns></returns>
        protected async Task RefreshList(T elementToReload)
        {
            var element = await Repository.GetByIdAsync(GetElementId(elementToReload));

            if (element is null)
                await LoadAsync(null);
            else
            {
                await Repository.Reload(element);
                await LoadAsync(null);
            }
        }
        #endregion

        #region Load
        /// <summary>
        /// Get all elements from repository
        /// </summary>
        /// <returns></returns>
        public abstract Task LoadAsync(int? id);
        #endregion

        #region SaveAsync

        protected abstract Action<int?> UpdateEntityBeforeSaveAction { get; }
        public virtual async Task SaveAsync(int? id)
        {
            if (id is null) return;

            UpdateEntityBeforeSaveAction(id);

            var entitiesToAdd = ListOfVMEntities.Where(t => GetElementId(t) == 0);

            if (entitiesToAdd.Any())
                Repository.AddRange(entitiesToAdd);

            await UnitOfWork.SaveAsync();
            //await LoadAsync(id);
        }

        #endregion

        #region Add

        public abstract Action ShowAddEditWindow { get; }

        protected virtual bool AddCommandCanExecute()
        {
            return true;
        }

        protected virtual void AddCommandExecute()
        {
            ShowAddEditWindow?.Invoke();
        }
        #endregion        

        #region Edit
        /// <summary>
        /// Metoda zwracajaca false gdy element DataGrid nie jest zaznaczony
        /// </summary>
        /// <returns></returns>
        protected virtual bool EditCommandCanExecute()
        {
            return SelectedVMEntity != null;
        }
        
        /// <summary>
        /// Edycja elementu, DODAC: otwieranie okna dodawania/edytowania
        /// </summary>
        protected virtual void EditCommandExecute()
        {
            ShowAddEditWindow?.Invoke();
            EditCommandMessage();
        }

        protected virtual void EditCommandMessage()
        {
            Messenger.Send(SelectedVMEntity);
        }
        #endregion

        #region Delete

        public virtual Action DeleteAction { get; }
        public virtual Action AfterDeleteAction { get; }

        /// <summary>
        /// Metoda usuwająca element z bazy
        /// </summary>
        protected virtual async void DeleteCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć wskazaną pozycję?")) return;

            if (DeleteAction is null)
            {
                var entityInDB = await Repository.GetByIdAsync(GetElementId(SelectedVMEntity));
                if (entityInDB is null)
                    ListOfVMEntities.Remove(SelectedVMEntity);
                else
                {
                    Repository.Remove(SelectedVMEntity);
                    ListOfVMEntities.Remove(SelectedVMEntity);
                }
            }
            else
            {
                DeleteAction.Invoke();
            }

            await UnitOfWork.SaveAsync();

            AfterDeleteAction?.Invoke();

            Messenger.Send(SelectedVMEntity);
            //Messenger.Send(new RefreshListMessage());
            //DialogService.ShowInfo_BtnOK("Pozycja została usunięta");

            //await LoadAsync(null);
        }
        /// <summary>
        /// Metoda zwracajaca false gdy element DataGrid nie jest zaznaczony
        /// </summary>
        /// <returns></returns>
        protected virtual bool DeleteCommandCanExecute()
        {
            return SelectedVMEntity != null;
        } 
        #endregion
    }
}
