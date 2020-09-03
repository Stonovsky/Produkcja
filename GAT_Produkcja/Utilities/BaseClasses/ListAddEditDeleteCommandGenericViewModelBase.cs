using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntityValidation;
using GAT_Produkcja.db.Enums;
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

namespace GAT_Produkcja.Utilities.BaseClasses
{
    [AddINotifyPropertyChangedInterface]

    public abstract class ListAddEditDeleteCommandGenericViewModelBase<T> : ViewModelServiceBase
                    where T : ValidationBase, new()
    {
        private T elementToReload;
        private ListViewModelStatesEnum? selectState;

        public ObservableCollection<T> ListOfVMEntities { get; set; } = new ObservableCollection<T>(); // Activator.CreateInstance<T>(typeof(ObservableCollection<>).MakeGenericType(new T()));
        public ObservableCollection<T> ListOfVMEntitiesOrg { get; set; } = new ObservableCollection<T>(); // Activator.CreateInstance<T>(typeof(ObservableCollection<>).MakeGenericType(new T()));
        public T SelectedVMEntity { get; set; }
        public virtual bool IsChanged => !ListOfVMEntities.Compare(ListOfVMEntitiesOrg);
        public virtual bool IsValid => ListOfVMEntities.Where(t => t.IsValid == false).ToList().Count() == 0;

        public abstract string Title { get; }
        public string SelectEditButtonTitle { get; set; }

        public abstract IGenericRepository<T> Repository { get; }

        #region Commands
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        #endregion

        #region CTOR
        public ListAddEditDeleteCommandGenericViewModelBase(IViewModelService viewModelService)
            : base(viewModelService)
        {
            LoadCommand = new RelayCommand(LoadCommandExecute);
            AddCommand = new RelayCommand(AddCommandExecute);
            EditCommand = new RelayCommand(EditCommandExecute, EditCommandCanExecute);
            DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);

            RegisterMessengers();
        }
        #endregion

        #region Messenger

        #region Register
        protected virtual void RegisterMessengers()
        {
            Messenger.Register<T>(this, nameof(RefreshListMessage), OnElementSentRefreshList); //nameof(RefreshListMessage)
            Messenger.Register<ListViewModelStatesEnum>(this, WhenStateChanges);
        }

        private void WhenStateChanges(ListViewModelStatesEnum obj)
        {
            selectState = obj;
            if (selectState==ListViewModelStatesEnum.AddEdit)
            {
                SelectEditButtonTitle = "Edytuj";
            }
            else
            {
                SelectEditButtonTitle = "Wybierz";
            }
        }
        #endregion

        /// <summary>
        /// Delegat wskazujacy na id elementu, gdy przeslano messengerem
        /// </summary>
        public abstract Func<T, int> GetElementSentId { get; }

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
            var element = await Repository.GetByIdAsync(GetElementSentId(elementToReload));

            if (element is null)
                await LoadElements();
            else
            {
                await Repository.Reload(element);
                await LoadElements();
            }
        }
        #endregion

        #region Load
        /// <summary>
        /// Get all elements from repository
        /// </summary>
        /// <returns></returns>
        protected virtual async void LoadCommandExecute()
        {
            await LoadElements();
        }

        protected virtual async Task LoadElements()
        {
            ListOfVMEntities = new ObservableCollection<T>(await Repository.GetAllAsync());

            IsChanged_False();
        }

        #endregion

        #region AddEdit
        public abstract Action ShowAddEditWindow { get; }

        protected virtual void AddCommandExecute()
        {
            ShowAddEditWindow?.Invoke();
        }

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
            if (selectState == ListViewModelStatesEnum.Select)
            {
                Messenger.Send(SelectedVMEntity);
                ViewService.Close(this.GetType().Name);
            }
            else
            {
                ShowAddEditWindow?.Invoke();
                Messenger.Send(SelectedVMEntity);
            }
        }


        #endregion
        #endregion

        #region Delete

        public virtual Func<Task> DeleteAction { get; }

        /// <summary>
        /// Metoda usuwająca element z bazy
        /// </summary>
        protected virtual async void DeleteCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć wskazaną pozycję?")) return;

            if (DeleteAction is null)
                Repository.Remove(SelectedVMEntity);
            else
                await DeleteAction.Invoke();

            await UnitOfWork.SaveAsync();

            Messenger.Send(SelectedVMEntity, nameof(RefreshListMessage));
            DialogService.ShowInfo_BtnOK("Pozycja została usunięta");

            await LoadElements();
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

        #region IsChanged

        public virtual void IsChanged_False()
        {
            ListOfVMEntitiesOrg = ListOfVMEntities.DeepClone();
        }
        #endregion

    }
}
