using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek
{
    [AddINotifyPropertyChangedInterface]

    public class GPRuchNaglowekViewModel_old : SaveCommandGenericViewModelBase
    {
        #region Fields
    private tblProdukcjaZlecenie wybraneAktywneZlecenieProdukcyjne;
        private tblProdukcjaGniazdoProdukcyjne wybraneGniazdo;
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;
        private ProdukcjaRuchZlecenieProdukcyjneMessage zlecenieProdPrzeslane;
        private ProdukcjaRuchNaglowekMessage naglowekPrzeslanyMessage;
        private tblProdukcjaRuchNaglowek naglowekPrzesalnyTbl;
        private tblProdukcjaZlecenieCiecia wybraneZlecenieCiecia;
        private string kodKreskowyZleceniaProdukcyjnego;
        private string kodKreskowyZleceniaCiecia;
        private readonly IGPRuchTowar_Naglowek_Helper naglowekHelper;
        private tblProdukcjaZlecenieCiecia zlecenieCiecia;
        private tblProdukcjaZlecenie zlecenieProdukcyjne;

        #endregion

        #region Properties
        public tblProdukcjaRuchNaglowek Naglowek { get; set; } = new tblProdukcjaRuchNaglowek();
        public tblProdukcjaRuchNaglowek NaglowekOrg { get; set; } = new tblProdukcjaRuchNaglowek();

        public IEnumerable<tblPracownikGAT> ListaPracownikow { get; set; }
        public tblPracownikGAT WybranyPracownik_1 { get; set; }
        public tblPracownikGAT WybranyPracownik_2 { get; set; }
        public IEnumerable<tblProdukcjaGniazdoProdukcyjne> ListaGniazdProdukcyjnych { get; set; }
        public tblProdukcjaGniazdoProdukcyjne WybraneGniazdo
        {
            get => wybraneGniazdo;
            set
            {
                wybraneGniazdo = value;
                WidocznoscZlecen();
                BlokadaRWdlaGniazdaWloknin();
                Messenger.Send(wybraneGniazdo);
            }
        }


        public tblProdukcjaZlecenieTowar ZlecenieTowar { get; set; }


        public string Tytul { get; set; }
        public override bool IsChanged
        {
            get
            {
                return RuchTowarRWViewModel.IsChanged
                      || RuchTowarPWViewModel.IsChanged
                      || !Naglowek.Compare(NaglowekOrg);
            }
        }
        public override bool IsValid
        {
            get
            {
                return RuchTowarRWViewModel.IsValid
                    && RuchTowarPWViewModel.IsValid
                    && Naglowek.IsValid;
            }
        }


        public bool CzyZlecProdMaBycWidoczne { get; set; } = true;
        public bool CzyZlecCieciaMaBycWidoczne { get; set; } = false;

        public IGPRuchTowarRWViewModel RuchTowarRWViewModel { get; }
        public IGPRuchTowarPWViewModel RuchTowarPWViewModel { get; }
        public bool RwEnabled { get; set; }
        #endregion

        #region Commands
        public RelayCommand DodajZlecenieCieciaCommand { get; set; }
        public RelayCommand DodajZlecenieProdukcyjneCommand { get; set; }
        public RelayCommand AnulujCommand { get; set; }
        public string KodKreskowyZleceniaProdukcyjnego1 { get => kodKreskowyZleceniaProdukcyjnego; set => kodKreskowyZleceniaProdukcyjnego = value; }
        #endregion

        #region CTOR
        public GPRuchNaglowekViewModel_old(IViewModelService viewModelService,
                                       IGPRuchTowarRWViewModel ruchTowarRWViewModel,
                                       IGPRuchTowarPWViewModel ruchTowarPWViewModel,
                                       IGPRuchTowar_Naglowek_Helper naglowekHelper
                                       )
            : base(viewModelService)
        {
            this.naglowekHelper = naglowekHelper;

            RuchTowarRWViewModel = ruchTowarRWViewModel;
            RuchTowarPWViewModel = ruchTowarPWViewModel;

            DodajZlecenieCieciaCommand = new RelayCommand(DodajZlecenieCieciaCommandExecute);
            DodajZlecenieProdukcyjneCommand = new RelayCommand(DodajZlecenieProdukcyjneCommandExecute);
            AnulujCommand = new RelayCommand(AnulujCommandExecute);

            Messenger.Register<tblProdukcjaRuchNaglowek>(this, GdyPrzeslanoNaglowek);
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdoProdukcyjne);
            // Ze ZlecenieCieciaEwidencja lub ZlecenieProdukcyjneEwidencja
            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowar);

            IsChanged_False();
        }

        private void DodajZlecenieProdukcyjneCommandExecute()
        {
            ViewService.Show<ZlecenieProdukcyjneEwidencjaViewModel_old>();
            Messenger.Send<tblProdukcjaGniazdoProdukcyjne, ZlecenieProdukcyjneEwidencjaViewModel_old>(WybraneGniazdo);
        }

        private void AnulujCommandExecute()
        {
            if (IsChanged)
            {
                if (!DialogService.ShowQuestion_BoolResult("Zmiany nie zostaną zachowane. Kontynuować?"))
                    return;
            }

            ViewService.Close(this.GetType().Name);
        }

        private void WidocznoscZlecen()
        {
            if (WybraneGniazdo is null) CzyZlecCieciaMaBycWidoczne = true;

            if (WybraneGniazdo.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji)
            {
                CzyZlecCieciaMaBycWidoczne = true;
                CzyZlecProdMaBycWidoczne = false;
            }
            else
            {
                CzyZlecCieciaMaBycWidoczne = false;
                CzyZlecProdMaBycWidoczne = true;
            }
        }

        private void BlokadaRWdlaGniazdaWloknin()
        {
            if (WybraneGniazdo is null) return;

            if (WybraneGniazdo.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                RwEnabled = false;
                return;
            }

            RwEnabled = true;
        }

        private void GdyPrzeslanoZlecenieTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            ZamknijOknaEwidencji();
            PrzypiszZlecenia(obj);
            ZlecenieTowar.TowarNazwa = new NazwaTowaruGenerator().GenerujNazweTowaru(obj);
        }

        private void PrzypiszZlecenia(tblProdukcjaZlecenieTowar obj)
        {
            ZlecenieTowar = obj;
            //zlecenieCiecia = obj.tblProdukcjaZlecenieCiecia;
            //zlecenieProdukcyjne = obj.tblProdukcjaZlcecenieProdukcyjne;
        }

        private void ZamknijOknaEwidencji()
        {
            ViewService.Close<ZlecenieProdukcyjneNaglowekViewModel_old>();
            ViewService.Close<ZlecenieProdukcyjneEwidencjaViewModel_old>();
            ViewService.Close<ZlecenieDodajTowarViewModel>();
            ViewService.Close<ZlecenieCieciaNaglowekViewModel_old>();
            ViewService.Close<ZlecenieCieciaEwidencjaViewModel>();
        }

        private void DodajZlecenieCieciaCommandExecute()
        {
            ViewService.Show<ZlecenieCieciaEwidencjaViewModel>();
        }

        private void GdyPrzeslanoGniazdoProdukcyjne(tblProdukcjaGniazdoProdukcyjne obj)
        {
            gniazdoProdukcyjne = obj;
        }

        #endregion

        #region Messengers

        private void GdyPrzeslanoNaglowek(tblProdukcjaRuchNaglowek obj)
        {
            if (obj?.IDProdukcjaRuchNaglowek is null) return;

            naglowekPrzesalnyTbl = obj;

        }

        private async Task<tblProdukcjaRuchNaglowek> PobierzNaglowek(int idNaglowek)
        {
            return await UnitOfWork.tblProdukcjaRuchNaglowek.GetByIdAsync(idNaglowek);
        }
        #endregion

        #region Delete Command
        protected override bool DeleteCommandCanExecute()
        {
            return Naglowek.IDProdukcjaRuchNaglowek != 0;
        }

        protected override async void DeleteCommandExecute()
        {
            if (DialogService.ShowQuestion_BoolResult("Czy usunąć bieżący rekord?"))
            {
                
                // przeniesc usuwanie na ewidencje!!

                //await RuchTowarRWViewModel.DeleteAsync(Naglowek.IDProdukcjaRuchNaglowek);
                //await RuchTowarPWViewModel.DeleteAsync(Naglowek.IDProdukcjaRuchNaglowek);

                ViewService.Close(this.GetType().Name);
            }
        }


        #endregion

        #region Load Command
        protected override async void LoadCommandExecute()
        {
            ListaPracownikow = await UnitOfWork.tblPracownikGAT.GetAllAsync();
            ListaGniazdProdukcyjnych = await UnitOfWork.tblProdukcjaGniazdoProdukcyjne.GetAllAsync();

            await AktualizujPozycje();

            await RuchTowarRWViewModel.LoadAsync(Naglowek?.IDProdukcjaRuchNaglowek);
            await RuchTowarPWViewModel.LoadAsync(Naglowek?.IDProdukcjaRuchNaglowek);

            CzyRwAktywne();
            WybranyPracownik_1 = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT);

            await PobierzNaglowek();
        }

        private async Task PobierzNaglowek()
        {
            if (naglowekPrzesalnyTbl is null) return;
            Naglowek = await PobierzNaglowek(naglowekPrzesalnyTbl.IDProdukcjaRuchNaglowek);
            if (Naglowek is null)
                Naglowek = new tblProdukcjaRuchNaglowek();
        }

        private void CzyRwAktywne()
        {
            if (WybraneGniazdo is null)
            {
                RwEnabled = true;
                return;
            }
            if (WybraneGniazdo.IDProdukcjaGniazdoProdukcyjne == 0)
            {
                RwEnabled = false;
                return;
            }

            RwEnabled = CzyFormularzRwAktywny((GniazdaProdukcyjneEnum)WybraneGniazdo?.IDProdukcjaGniazdoProdukcyjne);
        }

        private async Task AktualizujPozycje()
        {
            //AktualizujWybraneAktywneZlecenieProdukcyjne();
            AktualizujWybraneGniazdo();
            await AktualizujNaglowek();
        }

        private async Task AktualizujNaglowek()
        {
            if (naglowekPrzeslanyMessage != null && naglowekPrzeslanyMessage.IDProdukcjaRuchNaglowek != 0)
                Naglowek = await PobierzNaglowek(naglowekPrzeslanyMessage.IDProdukcjaRuchNaglowek);
            else if (naglowekPrzesalnyTbl != null && naglowekPrzesalnyTbl.IDProdukcjaRuchNaglowek != 0)
                Naglowek = await PobierzNaglowek(naglowekPrzesalnyTbl.IDProdukcjaRuchNaglowek);

            if (Naglowek == null)
            {
                Naglowek = new tblProdukcjaRuchNaglowek();
            }
        }

        private void AktualizujWybraneGniazdo()
        {
            if (gniazdoProdukcyjne != null && gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne != 0)
                WybraneGniazdo = ListaGniazdProdukcyjnych
                                                    .SingleOrDefault(g => g.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
        }

        private bool CzyFormularzRwAktywny(GniazdaProdukcyjneEnum? gniazdaProdukcyjneEnum)
        {
            if (gniazdaProdukcyjneEnum is null) return true;

            switch (gniazdaProdukcyjneEnum)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    return false;
                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    return true;
                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region Save Command
        protected override bool SaveCommandCanExecute()
        {
            if (gniazdoProdukcyjne is null) return false;

            //Z uwagi na brak RW dla Linii Wloknin sprawdzamy tylko PW
            if (gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                return RuchTowarPWViewModel.IsValid
                    && RuchTowarPWViewModel.IsChanged;
            }

            return RuchTowarRWViewModel.IsValid
                && RuchTowarRWViewModel.IsChanged
                && RuchTowarPWViewModel.IsValid
                && RuchTowarPWViewModel.IsChanged
                && Naglowek.IsValid;
        }

        protected override async void SaveCommandExecute()
        {
            UzupelnijDaneNaglowka();
            if (Naglowek.IDProdukcjaRuchNaglowek == 0)
                UnitOfWork.tblProdukcjaRuchNaglowek.Add(Naglowek);

            await UnitOfWork.SaveAsync();

            
            await RuchTowarRWViewModel.SaveAsync(Naglowek.IDProdukcjaRuchNaglowek);
            await RuchTowarPWViewModel.SaveAsync(Naglowek.IDProdukcjaRuchNaglowek);

            Messenger.Send<tblProdukcjaRuchNaglowek, ProdukcjaRuchEwidencjaUCViewModel_old>(Naglowek);

            DialogService.ShowInfo_BtnOK("Zapis w bazie danych zakończony powodzeniem.");
        }

        private void UzupelnijDaneNaglowka()
        {
            Naglowek.IDMagazyn = (int)MagazynyEnum.ProdukcjaGeowlokniny_PRGW;
            Naglowek.IDProdukcjaZlecenieCiecia = ZlecenieTowar?.IDProdukcjaZlecenieCiecia;
            Naglowek.IDProdukcjaZlecenieProdukcyjne = ZlecenieTowar?.IDProdukcjaZlecenie;
            Naglowek.DataDodania = DateTime.Now;
            Naglowek.IDProdukcjaGniazdoProdukcyjne = gniazdoProdukcyjne?.IDProdukcjaGniazdoProdukcyjne ?? 0;
        }

        #endregion

        public override void IsChanged_False()
        {
            NaglowekOrg = Naglowek.DeepClone();
        }

    }
}
