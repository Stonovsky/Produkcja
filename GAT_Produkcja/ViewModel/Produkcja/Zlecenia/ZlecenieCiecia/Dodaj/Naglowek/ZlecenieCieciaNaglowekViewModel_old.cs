using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
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

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek
{
    [AddINotifyPropertyChangedInterface]

    public class ZlecenieCieciaNaglowekViewModel_old : SaveCommandGenericViewModelBase
    {
        #region Fields
        private tblProdukcjaZlecenieCiecia zlecenieCieciaPrzeslane;
        private tblKontrahent kontrahent;

        #endregion

        #region Properties
        public tblProdukcjaZlecenieCiecia Naglowek { get; set; } = new tblProdukcjaZlecenieCiecia();
        public tblProdukcjaZlecenieCiecia NaglowekOrg { get; set; } = new tblProdukcjaZlecenieCiecia();

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
                Naglowek.IDKontrahent = Kontrahent.ID_Kontrahent;
            }
        }

        public string Tytul { get; set; }

        public IZlecenieCieciaTowarViewModel ZlecenieCieciaTowarViewModel { get; }

        #endregion

        #region Commands
        public RelayCommand WybierzKontrahentaCommand { get; set; }
        #endregion

        #region CTOR
        public ZlecenieCieciaNaglowekViewModel_old(IViewModelService viewModelService,
                                               IZlecenieCieciaTowarViewModel ZlecenieCieciaTowarViewModel
                                               )
            : base(viewModelService)
        {
            this.ZlecenieCieciaTowarViewModel = ZlecenieCieciaTowarViewModel;

            WybierzKontrahentaCommand = new RelayCommand(WybierzKontrahentaCommandExecute);

            Messenger.Register<tblProdukcjaZlecenieCiecia>(this, GdyPrzeslanoZlecenieCiecia);
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
            await PobierzZlecenieCieciaGdyPrzeslano();

            Tytul = $"Zlecenie nr {Naglowek.NrDokumentu}";
        }

        private async Task PobierzZlecenieCieciaGdyPrzeslano()
        {
            if (zlecenieCieciaPrzeslane is null)
                return;
            if (zlecenieCieciaPrzeslane.IDProdukcjaZlecenieCiecia == 0)
                return;

            Naglowek = await UnitOfWork.tblProdukcjaZlecenieCiecia.GetByIdAsync(zlecenieCieciaPrzeslane.IDProdukcjaZlecenieCiecia);
            WybranyStatus = ListaStatusow.SingleOrDefault(s => s.IDProdukcjaZlecenieStatus == Naglowek.IDProdukcjaZlecenieStatus);
            WybranyPracownikZlecajacy = ListaPracownikow.SingleOrDefault(s => s.ID_PracownikGAT == Naglowek.IDZlecajacy);
            WybranyPracownikWykonujacy = ListaPracownikow.SingleOrDefault(s => s.ID_PracownikGAT == Naglowek.IDWykonujacy);
            Kontrahent = await UnitOfWork.tblKontrahent.GetByIdAsync(Naglowek.IDKontrahent);
            await ZlecenieCieciaTowarViewModel.LoadAsync(Naglowek.IDProdukcjaZlecenieCiecia);
        }

        private async Task PrzypiszDaneWyjsciowe()
        {
            PrzypiszZlecajacego();
            Naglowek.DataZlecenia = DateTime.Now;
            Naglowek.DataWykonania = DateTime.Now;
            Naglowek.NrZleceniaCiecia = await UnitOfWork.tblProdukcjaZlecenieCiecia.GetNewNumberAsync(n => n.DataZlecenia.Year == DateTime.Now.Year, n => n.NrZleceniaCiecia);
            Naglowek.NrDokumentu = await UnitOfWork.tblProdukcjaZlecenieCiecia.GetNewFullNumberAsync(n => n.DataZlecenia.Year == DateTime.Now.Year, n => n.NrZleceniaCiecia,"ZLC");
            Naglowek.KodKreskowy = Naglowek.NrDokumentu?.Replace("/", "");
        }

        private void PrzypiszZlecajacego()
        {
            if (UzytkownikZalogowany.Uzytkownik == null)
                return;

            WybranyPracownikZlecajacy = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT);
            if (WybranyPracownikZlecajacy != null)
                Naglowek.IDZlecajacy = WybranyPracownikZlecajacy.ID_PracownikGAT;
        }
        #endregion

        private void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            if (obj is null || obj.ID_Kontrahent == 0)
                return;

            Kontrahent = obj;

            ViewService.Close<DodajKontrahentaViewModel_old>();
            ViewService.Close<EwidencjaKontrahentowViewModel_old>();
        }

        private void GdyPrzeslanoZlecenieCiecia(tblProdukcjaZlecenieCiecia obj)
        {
            zlecenieCieciaPrzeslane = obj;
        }

        private void WybierzKontrahentaCommandExecute()
        {
            ViewService.Show<EwidencjaKontrahentowViewModel_old>();
        }

        public override bool IsChanged => !Naglowek.Compare(NaglowekOrg);

        public override bool IsValid => Naglowek.IsValid
                                     && ZlecenieCieciaTowarViewModel.IsValid;


        public override void IsChanged_False()
        {
            NaglowekOrg = Naglowek.DeepClone();
        }

        #region DeleteCommand
        protected override bool DeleteCommandCanExecute()
        {
            return Naglowek.IDProdukcjaZlecenieCiecia != 0;
        }

        protected override async void DeleteCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć bieżące zlecenie cięcia?"))
                return;

            await ZlecenieCieciaTowarViewModel.DeleteAsync(Naglowek.IDProdukcjaZlecenieCiecia);

            UnitOfWork.tblProdukcjaZlecenieCiecia.Remove(Naglowek);
            await UnitOfWork.SaveAsync();

            Messenger.Send(Naglowek);
            DialogService.ShowInfo_BtnOK($"Zlecenie cięcia nr: {Naglowek.NrDokumentu} zostało usunięte.");
            ViewService.Close(this.GetType().Name);

        }
        #endregion
        
        #region SaveCommand
        protected override bool SaveCommandCanExecute()
        {
            return IsValid;
        }

        protected override async void SaveCommandExecute()
        {
            if (Naglowek.IDProdukcjaZlecenieCiecia == 0)
            {
                Naglowek.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
                UnitOfWork.tblProdukcjaZlecenieCiecia.Add(Naglowek);
            }

            await UnitOfWork.SaveAsync();

            await ZlecenieCieciaTowarViewModel.SaveAsync(Naglowek.IDProdukcjaZlecenieCiecia);

            Messenger.Send(Naglowek);
            DialogService.ShowInfo_BtnOK($"Zlecenie cięcia nr: {Naglowek.NrDokumentu} zostało zapisane.");
            ViewService.Close(this.GetType().Name);
        }

        #endregion

    }
}
