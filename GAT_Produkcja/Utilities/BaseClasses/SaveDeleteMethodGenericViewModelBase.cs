using GAT_Produkcja.db;
using GAT_Produkcja.db.EntityValidation;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class SaveDeleteMethodGenericViewModelBase<T> : ViewModelServiceBase
        where T : ValidationBase
    {
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
        public string Title { get; set; }
        #endregion

        #region Delegates
        /// <summary>
        /// Returns Id of current <see cref="VMEntity"/>
        /// </summary>
        protected abstract Func<int> GetVMEntityId { get; }

        #endregion

        #region CTOR

        public SaveDeleteMethodGenericViewModelBase(IViewModelService viewModelService)
            : base(viewModelService)
        {
            MessengerRegistration();
        }

        protected virtual void MessengerRegistration()
        {
        }


        #endregion

        #region LoadAsync

        protected abstract Func<int?,Task> LoadEntitiesAsync { get; }

        public virtual async Task LoadAsync(int? id)
        {
            await LoadEntitiesAsync(id);
            IsChanged_False();
        }

        #endregion

        #region SaveAsync

        /// <summary>
        /// Updates VMEntity with needed property values i.e. ID, Date of adding, etc.
        /// </summary>
        protected virtual Func<int?,Task> UpdateEntityBeforeSaveAction { get; }

        /// <summary>
        /// Saves additional entities
        /// </summary>
        protected virtual Func<Task> SaveAdditional { get; }
        /// <summary>
        /// Add or update entity to DB
        /// </summary>

        public virtual async Task SaveAsync(int? idForUpdate)
        {
            if (GetVMEntityId() == 0)
            {
                if (UpdateEntityBeforeSaveAction != null)
                    await UpdateEntityBeforeSaveAction(idForUpdate);

                Repository.Add(VMEntity);
            }

            await UnitOfWork.SaveAsync();

            if (SaveAdditional != null)
                await SaveAdditional.Invoke();

            IsChanged_False();

            DialogService.ShowInfo_BtnOK("Pozycja została zapisana w bazie danych");
            Messenger.Send(VMEntity, new RefreshListMessage());
            ViewService.Close(this.GetType().Name);

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
