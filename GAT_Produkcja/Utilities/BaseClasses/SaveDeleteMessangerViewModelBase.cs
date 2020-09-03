using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class SaveDeleteMessangerViewModelBase : ViewModelBase, IMyViewModelBase
    {
        protected readonly IMessenger messenger;

        public abstract bool IsValid { get; set; }
        public abstract bool IsChanged { get; set; }


        #region CTOR
        public SaveDeleteMessangerViewModelBase(IMessenger messenger)
        {
            this.messenger = messenger;

        }

        #endregion

        public abstract Task LoadAsync(int? id);

        public virtual void RaiseAfterSavedMessage(int id)
        {
            messenger.Send(new PoZapisieMessage
            {
                Id = id,
                ViewModelNazwa = this.GetType().Name
            }
            );
        }
        public virtual void AfterDeleteMessage(int id)
        {
            messenger.Send(new PoZapisieMessage
            {
                Id = id,
                ViewModelNazwa = this.GetType().Name
            }
            );
        }
    }
}
