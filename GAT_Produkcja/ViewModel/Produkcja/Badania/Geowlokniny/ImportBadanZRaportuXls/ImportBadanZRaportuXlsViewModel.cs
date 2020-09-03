using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls.ImportZPliku;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.WeryfikacjaWynikowBadan;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls
{
    public class ImportBadanZRaportuXlsViewModel:ViewModelBase
    {
        #region Properties
        private tblWynikiBadanGeowloknin wynikiBadania;
        private IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IImportBadanZRaportuZXls importBadanZPliku;
        private readonly IWeryfikacjaWynikowBadan weryfikacja;
        private readonly IMessenger messenger;
        private ImportZPlikuModel wyniki;
        private string tytul;

        public tblWynikiBadanGeowloknin WynikiBadania
        {
            get { return wynikiBadania; }
            set { wynikiBadania = value; RaisePropertyChanged(); }
        }


        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        private List<tblPracownikGAT> pracownicy;

        public List<tblPracownikGAT> Pracownicy
        {
            get { return pracownicy; }
            set { pracownicy = value;RaisePropertyChanged(); }
        }

        private tblPracownikGAT wybranyPracownik;

        public tblPracownikGAT WybranyPracownik 
        {
            get { return wybranyPracownik; }
            set { wybranyPracownik = value; RaisePropertyChanged(); }
        }

        private List<tblWynikiBadanDlaProbek> listaWynikowBadanSzczegolowych;

        public List<tblWynikiBadanDlaProbek> ListaWynikowBadanSzczegolowych
        {
            get { return listaWynikowBadanSzczegolowych; }
            set { listaWynikowBadanSzczegolowych = value; RaisePropertyChanged(); }
        }
        private tblWynikiBadanDlaProbek wybraneBadanieSzczegolowe;

        public tblWynikiBadanDlaProbek WybraneBadanieSzczegolowe
        {
            get { return wybraneBadanieSzczegolowe; }
            set { wybraneBadanieSzczegolowe = value; RaisePropertyChanged(); }
        }




        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand PobierzWynikBadaniaZPlikuCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        #endregion


        #region CTOR
        public ImportBadanZRaportuXlsViewModel(IUnitOfWork unitOfWork,
                                                IUnitOfWorkFactory unitOfWorkFactory,
                                                IViewService viewService,
                                                IDialogService dialogService,
                                                IImportBadanZRaportuZXls importBadanZPliku,
                                                IWeryfikacjaWynikowBadan weryfikacja,
                                                IMessenger messenger
            )
        {
            this.unitOfWork = unitOfWorkFactory.Create();
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.importBadanZPliku = importBadanZPliku;
            this.weryfikacja = weryfikacja;
            this.messenger = messenger;

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            PobierzWynikBadaniaZPlikuCommand = new RelayCommand(PobierzWynikBadaniaZPlikuCommandExecute);
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);

            messenger.Register<tblWynikiBadanGeowloknin>(this, GdyPrzeslanoBadanie);

            WynikiBadania = new tblWynikiBadanGeowloknin();
            ListaWynikowBadanSzczegolowych = new List<tblWynikiBadanDlaProbek>();
        }

        private void GdyPrzeslanoBadanieProbek(tblWynikiBadanDlaProbek obj)
        {
            throw new NotImplementedException();
        }

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            Tytul = "Dodaj badanie";
            WybranyPracownik = UzytkownikZalogowany.Uzytkownik;
            WynikiBadania.IDPracownikGAT = WybranyPracownik.ID_PracownikGAT;
            RaisePropertyChanged(nameof(WynikiBadania));

            using (var uow = unitOfWorkFactory.Create())
            {
                Pracownicy = await uow.tblPracownikGAT.PobierzPracownikowPracujacychAsync().ConfigureAwait(false);
            }

        }
        private async void GdyPrzeslanoBadanie(tblWynikiBadanGeowloknin obj)
        {
            try
            {
                Pracownicy = await unitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync().ConfigureAwait(false);
                WynikiBadania = await unitOfWork.tblWynikiBadanGeowloknin.GetByIdAsync(obj.IDWynikiBadanGeowloknin).ConfigureAwait(false);
                ListaWynikowBadanSzczegolowych = await unitOfWork.tblWynikiBadanDlaProbek
                                                .WhereAsync(p => p.IDWynikiBadanGeowloknin == WynikiBadania.IDWynikiBadanGeowloknin)
                                                .ConfigureAwait(false) as List<tblWynikiBadanDlaProbek>;

                Tytul = "Badanie dla próbki nr:" + WynikiBadania.NrRolki + ", nr kodu:" + WynikiBadania.KodKreskowy;
                WybranyPracownik = Pracownicy.Where(p => p.ID_PracownikGAT == obj.IDPracownikGAT).SingleOrDefault();
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK(ex.Message + "\n\r" + ex.StackTrace);
                //throw;
            }
        }
        private async void PobierzWynikBadaniaZPlikuCommandExecute()
        {
            try
            {
                Tytul = "Pobieram dane z pliku Excel ...";
                wyniki = await importBadanZPliku.PobierzWynikiBadan();

                if (wyniki == null)
                    return;
                wyniki.WynikiOgolne.IDPracownikGAT = UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT;
                WynikiBadania = wyniki.WynikiOgolne;
                WybranyPracownik = UzytkownikZalogowany.Uzytkownik;

                ListaWynikowBadanSzczegolowych = wyniki.WynikiSzczegoloweDlaProbek;

                Tytul = "Badanie pobrane";
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK(ex.Message);
            }

        }

        private bool UsunCommandCanExecute()
        {
            if (WynikiBadania.IDWynikiBadanGeowloknin==0)
            {
                return false;
            }
            return true;
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć badanie?"))
            {
                await UsunBadania();
            }
        }

        private async Task UsunBadania()
        {
            if (WynikiBadania.IDWynikiBadanGeowloknin > 0)
            {
                var listaBadanSzczegolowych = await unitOfWork.tblWynikiBadanDlaProbek.WhereAsync(b => b.IDWynikiBadanGeowloknin == WynikiBadania.IDWynikiBadanGeowloknin);
                unitOfWork.tblWynikiBadanDlaProbek.RemoveRange(listaBadanSzczegolowych);

                unitOfWork.tblWynikiBadanGeowloknin.Remove(WynikiBadania);

                await unitOfWork.SaveAsync();
            }
        }

        #region Zapis
        private bool ZapiszCommandCanExecute()
        {
            if (ListaWynikowBadanSzczegolowych.Count() == 0)
            {
                return false;
            }

            if (WybranyPracownik==null)
            {
                return false;
            }
            return true;
        }

        private async void ZapiszCommandExecute()
        {
            if (await CzyBadanieIstniejeWBazie())
            {
                if (dialogService.ShowQuestion_BoolResult("Badanie o tej samej dacie oraz nr rolki znajduje się w bazie. Czy nadpisać?"))
                {
                    await UsunBadanieIstniejace();

                    await ZapiszWynikiBadanOgolnych();
                    await ZapiszWynikiBadanDlaProbek();
                }
            }
            else
            {
                await ZapiszWynikiBadanOgolnych();
                await ZapiszWynikiBadanDlaProbek();
            }

            try
            {
                await weryfikacja.SprawdzCzyWynikiBadanWTolerancjach(WynikiBadania);
            }
            catch (Exception)
            {
                throw;
            }

            viewService.Close<ImportBadanZRaportuXlsViewModel>();
            messenger.Send("Odswiez","EwidencjaBadan");
        }

        private async Task UsunBadanieIstniejace()
        {
            var badanieOgolne = await unitOfWork.tblWynikiBadanGeowloknin.PobierzBadanieZNrProbkiIDaty(WynikiBadania.NrRolki, WynikiBadania.DataBadania.GetValueOrDefault());
            var listaBadanSzczegolowych = await unitOfWork.tblWynikiBadanDlaProbek.WhereAsync(b => b.IDWynikiBadanGeowloknin == badanieOgolne.IDWynikiBadanGeowloknin);

            unitOfWork.tblWynikiBadanDlaProbek.RemoveRange(listaBadanSzczegolowych);
            unitOfWork.tblWynikiBadanGeowloknin.Remove(badanieOgolne);

            await unitOfWork.SaveAsync();
        }
        private async Task<bool> CzyBadanieIstniejeWBazie()
        {
            var badaniaPowtorzone = await unitOfWork.tblWynikiBadanGeowloknin.PobierzBadanieZNrProbkiIDaty(WynikiBadania.NrRolki, WynikiBadania.DataBadania.GetValueOrDefault());

            if (badaniaPowtorzone!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task ZapiszWynikiBadanOgolnych()
        {
            unitOfWork.tblWynikiBadanGeowloknin.Add(WynikiBadania);
            await unitOfWork.SaveAsync();
        }

        private async Task ZapiszWynikiBadanDlaProbek()
        {
            foreach (var badanie in ListaWynikowBadanSzczegolowych)
            {
                badanie.IDWynikiBadanGeowloknin = WynikiBadania.IDWynikiBadanGeowloknin;
            }
            
            unitOfWork.tblWynikiBadanDlaProbek.AddRange(ListaWynikowBadanSzczegolowych);
            await unitOfWork.SaveAsync();
        }

        #endregion
        #endregion



    }
}
