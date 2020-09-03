using Autofac;
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
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class AddEditCommandGenericViewModelBase<T> : ViewModelServiceBase, IDetailViewModel
        where T : ValidationBase
    {
        protected T elementSent;

        #region Properties
        /// <summary>
        /// <see cref="VMEntity"/> is the main object to be fulfilled and then add to DB
        /// </summary>
        public T VMEntity { get; set; } = Activator.CreateInstance<T>();

        /// <summary>
        /// Base entity to compare with <see cref="VMEntity"/> in order to assess IsChanged Property
        /// </summary>
        public T VMEntityBaseToCompare { get; set; } = Activator.CreateInstance<T>();

        /// <summary>
        /// Set appropriate repository for <see cref="VMEntity"/> -  as this is generic abstract class
        /// </summary>
        public abstract IGenericRepository<T> Repository { get; }

        /// <summary>
        /// Assert whether something was changed
        /// </summary>
        public virtual bool IsChanged => !VMEntityBaseToCompare.Compare(VMEntity);

        /// <summary>
        /// IsValid property base on the ValidationBase class
        /// </summary>
        public virtual bool IsValid => VMEntity.IsValid;

        /// <summary>
        /// Window title
        /// </summary>
        public virtual string Title { get; set; }
        #endregion

        #region Commands
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand<CancelEventArgs> CloseWindowCommand { get; set; }
        #endregion

        #region Delegates

        /// <summary>
        /// Func that point to Id as Id property has different name i.e. ()=>VMEntity.IDTowar
        /// This Func is used in Save method to find out wheter entity Id is 0
        /// </summary>
        protected abstract Func<int> GetVMEntityId { get; }
        #endregion

        #region CTOR
        public AddEditCommandGenericViewModelBase(IViewModelService viewModelService) : base(viewModelService)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            CloseWindowCommand = new RelayCommand<CancelEventArgs>(CloseWindowCommandExecute);

            RegisterMessengers();
        }
        #endregion

        #region MessengerRegistration
        /// <summary>
        /// Method for register messengers.
        /// </summary>
        protected virtual void RegisterMessengers()
        {
            Messenger.Register<T>(this, OnElementSent);
        }
        #endregion


        #region Messenger

        /// <summary>
        /// When entity sent load it from DB to trace by Entity Framework
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnElementSent(T obj)
        {
            elementSent = obj;
            //Messenger.Unregister<T>(this, OnElementSent);
        }

        /// <summary>
        /// Gets Entity from DB When entity was sent by Messenger in order to trace it in EF
        /// </summary>
        /// <returns></returns>
        protected virtual async Task GetEntityFromDbWhenSentByMessenger()
        {
            if (elementSent is null) return;
            var id = GetIdFromEntityWhenSentByMessenger(elementSent);
            VMEntity = await Repository.GetByIdAsync(id);

            if (VMEntity is null)
                VMEntity = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Delegate that returns Id of Entity when sent by Messenger. Different Entities have different name of Id property.
        /// This Id is used in LoadAsync method to get Entity from DB
        /// </summary>
        public abstract Func<T, int> GetIdFromEntityWhenSentByMessenger { get; }
        public virtual Func<Task> LoadAdditionally { get; }
        #endregion

        #region Close Window

        /// <summary>
        /// Method invokes when user want to close window and send information depends on the IsChanged prop.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void CloseWindowCommandExecute(CancelEventArgs args)
        {
            if (!IsChanged)
            {
                ViewService.Close(this.GetType().Name);
                return;
            }

            if (DialogService.ShowQuestion_BoolResult("Wprowadzone zmiany nie będą zapisane. Czy kontynuować?"))
            {
                ViewService.Close(this.GetType().Name);
            }
            else
            {
                if (args != null)
                    args.Cancel = true;
            }
        }

        #endregion

        #region LoadCommand

        /// <summary>
        /// Loads from external source (DB) different entities when form loads. 
        /// This method should be overridden and base call should be at the end.
        /// </summary>
        protected virtual async void LoadCommandExecute()
        {
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            await Task.Delay(200);
            await GetEntityFromDbWhenSentByMessenger();

            if (LoadAdditionally != null)
                await LoadAdditionally();

            IsChanged_False();
        }

        #endregion

        #region SaveCommand
        /// <summary>
        /// Updates VMEntity with needed property values i.e. Date of adding, etc.
        /// </summary>
        protected virtual Func<Task> UpdateEntityBeforeSaveAction { get; }

        /// <summary>
        /// Saves additional entities
        /// </summary>
        protected virtual Func<Task> SaveAdditional { get; }

        /// <summary>
        /// Add or update entity to DB
        /// </summary>
        protected virtual async Task SaveAsync()
        {
            if (GetVMEntityId() == 0)
            {
                if (UpdateEntityBeforeSaveAction != null)
                    await UpdateEntityBeforeSaveAction?.Invoke();

                Repository.Add(VMEntity);
            }

            await UnitOfWork.SaveAsync();

            if (SaveAdditional != null)
                await SaveAdditional.Invoke();

            IsChanged_False();

            ShowDialogAfterSaving();
            SendMessegAfterSaving();
            CloseWindowAfterSaving();
        }

        protected virtual void CloseWindowAfterSaving()
        {
            ViewService.Close(this.GetType().Name);
        }

        protected virtual  void SendMessegAfterSaving()
        {
            Messenger.Send(VMEntity, nameof(RefreshListMessage));
        }

        protected virtual void ShowDialogAfterSaving()
        {
            DialogService.ShowInfo_BtnOK("Pozycja została zapisana w bazie danych");
        }

        protected virtual bool SaveCommandCanExecute()
        {
            return VMEntity.IsValid;
        }

        protected virtual async void SaveCommandExecute()
        {
            await SaveAsync();
        }


        #endregion

        #region IsChanged

        public virtual void IsChanged_False()
        {
            VMEntityBaseToCompare = VMEntity.DeepClone();
        }
        #endregion
    }
}
