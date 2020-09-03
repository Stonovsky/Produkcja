using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek
{
    [AddINotifyPropertyChangedInterface]

    public class ZlecenieCieciaNaglowekViewModel
        : AddEditCommandGenericViewModelBase<tblProdukcjaZlecenie>
    {
        #region Fields
        private tblProdukcjaZlecenieCiecia zlecenieCieciaPrzeslane;
        private tblKontrahent kontrahent;

        #endregion

        #region Properties

        public IEnumerable<tblPracownikGAT> ListaPracownikow { get; set; }
        public tblPracownikGAT WybranyPracownikZlecajacy { get; set; }
        public tblPracownikGAT WybranyPracownikWykonujacy { get; set; }

        public IEnumerable<tblProdukcjaZlecenieStatus> ListaStatusow { get; set; }
        public tblProdukcjaZlecenieStatus WybranyStatus { get; set; }

        public tblKontrahent Kontrahent
        {
            get => kontrahent;
            set
            {
                kontrahent = value;
                VMEntity.IDKontrahent = Kontrahent.ID_Kontrahent;
            }
        }

        public IZlecenieCieciaTowarViewModel ZlecenieCieciaTowarViewModel { get; }
        public override bool IsValid => VMEntity.IsValid
                             && ZlecenieCieciaTowarViewModel.IsValid;

        public override IGenericRepository<tblProdukcjaZlecenie> Repository => UnitOfWork.tblProdukcjaZlecenie;

        #endregion

        #region Commands
        public RelayCommand WybierzKontrahentaCommand { get; set; }
        #endregion

        #region CTOR
        public ZlecenieCieciaNaglowekViewModel(IViewModelService viewModelService,
                                               IZlecenieCieciaTowarViewModel ZlecenieCieciaTowarViewModel)
            : base(viewModelService)
        {
            this.ZlecenieCieciaTowarViewModel = ZlecenieCieciaTowarViewModel;

            WybierzKontrahentaCommand = new RelayCommand(WybierzKontrahentaCommandExecute);

            // TODO: zmienic na tblProdukcjaZlecenie
            //Messenger.Register<tblProdukcjaZlecenieCiecia>(this, GdyPrzeslanoZlecenieCiecia);
            Messenger.Register<tblKontrahent>(this, GdyPrzeslanoKontrahenta);
        }


        #endregion

        #region LoadCommand
        protected override async void LoadCommandExecute()
        {
            ListaPracownikow = await UnitOfWork.tblPracownikGAT.GetAllAsync();
            ListaStatusow = await UnitOfWork.tblProdukcjaZlecenieStatus.GetAllAsync();
            WybranyStatus = ListaStatusow.SingleOrDefault(s => s.IDProdukcjaZlecenieStatus == (int)ProdukcjaZlecenieStatusEnum.Oczekuje);
            await PrzypiszDaneWyjsciowe();
            
            await base.LoadAsync();

            //await PobierzZlecenieCieciaGdyPrzeslano();

            Title = $"Zlecenie nr {VMEntity.NrDokumentu}";
        }

        private async Task PobierzZlecenieCieciaGdyPrzeslano()
        {
            if (zlecenieCieciaPrzeslane is null)
                return;
            if (zlecenieCieciaPrzeslane.IDProdukcjaZlecenieCiecia == 0)
                return;

            VMEntity = await UnitOfWork.tblProdukcjaZlecenie.GetByIdAsync(zlecenieCieciaPrzeslane.IDProdukcjaZlecenieCiecia);
            WybranyStatus = ListaStatusow.SingleOrDefault(s => s.IDProdukcjaZlecenieStatus == VMEntity.IDProdukcjaZlecenieStatus);
            WybranyPracownikZlecajacy = ListaPracownikow.SingleOrDefault(s => s.ID_PracownikGAT == VMEntity.IDZlecajacy);
            //WybranyPracownikWykonujacy = ListaPracownikow.SingleOrDefault(s => s.ID_PracownikGAT == VMEntity.IDWykonujacy);
            Kontrahent = await UnitOfWork.tblKontrahent.GetByIdAsync(VMEntity.IDKontrahent.GetValueOrDefault());
            await ZlecenieCieciaTowarViewModel.LoadAsync(VMEntity.IDProdukcjaZlecenie);
        }

        private async Task PrzypiszDaneWyjsciowe()
        {
            PrzypiszZlecajacego();
            VMEntity.DataUtworzenia = DateTime.Now;
            VMEntity.DataZakonczeniaFakt = DateTime.Now;
            VMEntity.NrZlecenia= await UnitOfWork.tblProdukcjaZlecenie.GetNewNumberAsync(n => n.DataUtworzenia.Year == DateTime.Now.Year, n => n.NrZlecenia.GetValueOrDefault());
            VMEntity.NrDokumentu = await UnitOfWork.tblProdukcjaZlecenie.GetNewFullNumberAsync(n => n.DataUtworzenia.Year == DateTime.Now.Year, n => n.NrZlecenia.GetValueOrDefault(), "ZLC");
            VMEntity.KodKreskowy = VMEntity.NrDokumentu?.Replace("/", "");
        }

        private void PrzypiszZlecajacego()
        {
            if (UzytkownikZalogowany.Uzytkownik == null)
            {
                VMEntity.IDZlecajacy = 7;
                return;
            }

            WybranyPracownikZlecajacy = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT);
            if (WybranyPracownikZlecajacy != null)
                VMEntity.IDZlecajacy = WybranyPracownikZlecajacy.ID_PracownikGAT;
        }
        #endregion

        private void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            if (obj is null || obj.ID_Kontrahent == 0)
                return;

            Kontrahent = obj;

            //ViewService.Close<DodajKontrahentaViewModel>();
            //ViewService.Close<EwidencjaKontrahentowViewModel>();
        }

        private void GdyPrzeslanoZlecenieCiecia(tblProdukcjaZlecenieCiecia obj)
        {
            zlecenieCieciaPrzeslane = obj;
        }

        private void WybierzKontrahentaCommandExecute()
        {
            ViewService.Show<EwidencjaKontrahentowViewModel>();
            Messenger.Send(ListViewModelStatesEnum.Select);
        }


        #region Delegaty
        protected override Func<int> GetVMEntityId => () => VMEntity.IDProdukcjaZlecenie;

        public override Func<tblProdukcjaZlecenie, int> GetIdFromEntityWhenSentByMessenger => (e) => e.IDProdukcjaZlecenie;
        #endregion

        #region DeleteCommand
        //protected override bool DeleteCommandCanExecute()
        //{
        //    return VMEntity.IDProdukcjaZlecenieCiecia != 0;
        //}

        //protected override async void DeleteCommandExecute()
        //{
        //    if (!DialogService.ShowQuestion_BoolResult("Czy usunąć bieżące zlecenie cięcia?"))
        //        return;

        //    await ZlecenieCieciaTowarViewModel.DeleteAsync(VMEntity.IDProdukcjaZlecenieCiecia);

        //    UnitOfWork.tblProdukcjaZlecenieCiecia.Remove(VMEntity);
        //    await UnitOfWork.SaveAsync();

        //    Messenger.Send(VMEntity);
        //    DialogService.ShowInfo_BtnOK($"Zlecenie cięcia nr: {VMEntity.NrDokumentu} zostało usunięte.");
        //    ViewService.Close(this.GetType().Name);

        //}
        #endregion

        #region SaveCommand
        protected override bool SaveCommandCanExecute()
        {
            return IsValid;
        }

        protected override Func<Task> UpdateEntityBeforeSaveAction => async () =>
        {
            VMEntity.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
            VMEntity.IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji;
        };
        protected override Func<Task> SaveAdditional =>
                async () => await ZlecenieCieciaTowarViewModel.SaveAsync(VMEntity.IDProdukcjaZlecenie);

        //protected override async void SaveCommandExecute()
        //{
        //    if (VMEntity.IDProdukcjaZlecenieCiecia == 0)
        //    {
        //        VMEntity.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
        //        UnitOfWork.tblProdukcjaZlecenieCiecia.Add(VMEntity);
        //    }

        //    await UnitOfWork.SaveAsync();

        //    await ZlecenieCieciaTowarViewModel.SaveAsync(VMEntity.IDProdukcjaZlecenieCiecia);

        //    Messenger.Send(VMEntity);
        //    DialogService.ShowInfo_BtnOK($"Zlecenie cięcia nr: {VMEntity.NrDokumentu} zostało zapisane.");
        //    ViewService.Close(this.GetType().Name);
        //}

        #endregion

    }
}
