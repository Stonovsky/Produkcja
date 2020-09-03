using GalaSoft.MvvmLight;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.DodajPrzerob
{
    public class DodajPrzerobProdukcjaGeokomorkaViewModel 
        : AddEditCommandGenericViewModelBase<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
    {
        #region Fields
        private decimal iloscWyprodukowana_kg;
        private decimal iloscNawrot_kg;
        #endregion

        #region Properties
        public decimal IloscWyprodukowana_kg
        {
            get
            {
                return VMEntity.IloscWyprodukowana_kg;
            }

            set
            {
                iloscWyprodukowana_kg = value;
                VMEntity.IloscWyprodukowana_kg = value;
                ObliczSumeIlosci();
            }
        }

        public decimal IloscNawrot_kg
        {
            get { return VMEntity.IloscNawrot_kg; }
            set
            {
                iloscNawrot_kg = value;
                VMEntity.IloscNawrot_kg = value;
                ObliczSumeIlosci();
            }
        }

        private void ObliczSumeIlosci()
        {
            VMEntity.Ilosc_kg = VMEntity.IloscWyprodukowana_kg + VMEntity.IloscNawrot_kg;
        }
        public override IGenericRepository<tblProdukcjaGeokomorkaPodsumowaniePrzerob> Repository => UnitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob;

        #endregion

        #region CTOR
        public DodajPrzerobProdukcjaGeokomorkaViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            VMEntity.DataOd = DateTime.Now.Date.StartOfWeek(DayOfWeek.Monday);
            VMEntity.DataDo = DateTime.Now.Date;
            VMEntityBaseToCompare = VMEntity.DeepClone();
            Title = "Produkcja geokomóki - przerób w datach";
        }
        #endregion

        #region Delegates
        protected override Func<int> GetVMEntityId => () => VMEntity.IdProdukcjaGeokomorkaPodsumowaniePrzerob;

        #endregion

        #region Messenger

        protected override async Task GetEntityFromDbWhenSentByMessenger()
        {
            await base.GetEntityFromDbWhenSentByMessenger();
            PrzypiszProperties();
        }
        private void PrzypiszProperties()
        {
            IloscNawrot_kg = VMEntity.IloscNawrot_kg;
            IloscWyprodukowana_kg = VMEntity.IloscWyprodukowana_kg;
        }

        #endregion

        #region LoadCommand
        public override Func<tblProdukcjaGeokomorkaPodsumowaniePrzerob, int> GetIdFromEntityWhenSentByMessenger => (entitySent) =>
        {
            if (entitySent != null)
                return entitySent.IdProdukcjaGeokomorkaPodsumowaniePrzerob;
            else
                return 0;
        };

        #endregion

        #region SaveCommand

        //protected override void SaveCommandExecute()
        //{
        //    UzupelnijVMEntity();

        //    base.SaveCommandExecute();
        //}

        protected override Func<Task> UpdateEntityBeforeSaveAction => UzupelnijVMEntity;
        
        private async Task UzupelnijVMEntity()
        {
            VMEntity.CenaJedn_kg = 2.70m;
            VMEntity.Wartosc = VMEntity.Ilosc_kg * 2.70m;
            VMEntity.DataDodania = DateTime.Now;
            VMEntity.IdOperator = UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT ?? 7;
        }


        #endregion
    }
}
