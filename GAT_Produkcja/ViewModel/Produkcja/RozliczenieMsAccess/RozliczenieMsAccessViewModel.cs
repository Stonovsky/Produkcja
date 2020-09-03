using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbComarch.Models;
using GAT_Produkcja.dbComarch.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Dictionaries;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess
{
    [AddINotifyPropertyChangedInterface]

    public class RozliczenieMsAccessViewModel : SaveCommandGenericViewModelBase
    {
        #region Fields
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private readonly IRozliczenieMsAccesHelper helper;
        private readonly IKonfekcjaHelper konfekcjaHelper;
        private readonly IUnitOfWorkComarch unitOfWorkComarch;
        private IEnumerable<Konfekcja> listaPozycjiKonfekcji;
        private IEnumerable<Kalander> listaPozycjiKalandra;
        private IEnumerable<dbMsAccess.Models.Produkcja> listaPozycjiProdukcji;
        private IEnumerable<NormyZuzycia> listaNormZuzycia;
        private IEnumerable<Surowiec> listaSurowcow;
        private IEnumerable<vwMagazynRuchGTX> listaSurowcowZCenamiSubiekt;
        #endregion

        #region Properties

        #region Wybor produktu oraz zlecen
        public IEnumerable<Artykuly> ListaProduktow { get; set; }
        public Artykuly WybranyProdukt { get; set; }
        public decimal WybranaSzerokosc { get; set; }
        public decimal WybranaDlugosc { get; set; }
        #endregion

        #region Naglowek
        public tblProdukcjaRozliczenie_Naglowek Naglowek { get; set; } = new tblProdukcjaRozliczenie_Naglowek();
        public tblProdukcjaRozliczenie_Naglowek NaglowekOrg { get; set; } = new tblProdukcjaRozliczenie_Naglowek();
        #endregion

        #region ListaPozycjiKonfekcji - typ Konfekcja - nieuzywany
        public IEnumerable<IProdukcjaRuchTowar> ListaKonfekcjiDlaZadanychParametrow { get; set; } = new List<KonfekcjaAdapter>();
        public IEnumerable<IProdukcjaRuchTowar> ListaKonfekcjiDlaZadanychParametrowOrg { get; set; } = new List<KonfekcjaAdapter>();
        public Konfekcja WybranaPozycjaKonfekcji { get; set; }
        public PwPodsumowanieModel PodsumowanieListyPozycjiKonfekcji { get; set; }
        public ObservableCollection<PwPodsumowanieModel> ListaProduktowWgZlecen { get; set; }

        private IEnumerable<tblProdukcjaRozliczenie_PW> listaKonfekcjiDoRozliczeniaAll;
        #endregion

        #region ListaPozycjiKonfekcjiDoRozliczenia - uzywany
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaPozycjiKonfekcjiDoRozliczenia
        {
            get => listaPozycjiKonfekcjiDoRozliczenia;
            set
            {
                listaPozycjiKonfekcjiDoRozliczenia = value;
                try
                {
                    PodsumowanieListyPozycjiKonfekcji = helper.PodsumujListe(ListaPozycjiKonfekcjiDoRozliczenia);
                }
                catch (Exception ex)
                {
                    DialogService.ShowInfo_BtnOK(ex.Message);
                }
            }
        }
        public tblProdukcjaRozliczenie_PW WybranaPozycjiaKonfekcjiDoRozliczenia { get; set; }
        #endregion

        #region RW
        public ObservableCollection<tblProdukcjaRozliczenie_RW> ListaRWOrg { get; set; } = new ObservableCollection<tblProdukcjaRozliczenie_RW>();
        public ObservableCollection<tblProdukcjaRozliczenie_RW> ListaRW { get; set; } = new ObservableCollection<tblProdukcjaRozliczenie_RW>();
        public tblProdukcjaRozliczenie_RW WybraneRW { get; set; }
        public RwPodsumowanieModel PodsumowanieRW { get; set; }
        #endregion        

        #region PW
        public ObservableCollection<tblProdukcjaRozliczenie_PW> ListaPW { get; set; } = new ObservableCollection<tblProdukcjaRozliczenie_PW>();
        public ObservableCollection<tblProdukcjaRozliczenie_PW> ListaPWOrg { get; set; } = new ObservableCollection<tblProdukcjaRozliczenie_PW>();
        public tblProdukcjaRozliczenie_PW WybranePW { get; set; }
        public PwPodsumowanieModel PodsumowaniePW { get; set; }
        public ObservableCollection<tblProdukcjaRozliczenie_PW> PodsumowaniePW_Towar { get; set; }
        public tblProdukcjaRozliczenie_PW WybranePodumowaniePW_Towar { get; set; }
        #endregion


        public List<RozliczenieSurowcaModel> ListaRozliczenSurowca { get; set; }
        public RozliczenieSurowcaModel WybraneRozliczenieSurowca { get; set; }

        public decimal Ilosc_m2 { get; set; }
        public IEnumerable<Konfekcja> ListaPozycjiDoRozliczenia { get; set; }

        public IEnumerable<Dyspozycje> ListaZlecen { get; set; }
        public Dyspozycje WybraneZlecenie { get; set; }
        public IEnumerable<NormyZuzycia> MieszankaDlaZlecenia { get; private set; }


        public IEnumerable<tblProdukcjaRuchTowar> ListaPozycjiKonfekcjiDlaZlecenia { get; set; }

        public override bool IsChanged => !ListaRW.Compare(ListaRWOrg)
                                       && !ListaPW.Compare(ListaPWOrg);

        public override bool IsValid => true;



        private SurowceDictionary surowceDictionary = new SurowceDictionary();
        private IEnumerable<tblProdukcjaRozliczenie_PW> listaPozycjiKonfekcjiDoRozliczenia;
        //private IEnumerable<tblProdukcjaRuchTowar> listaPozycjiKonfekcjiDlaZlecenia;

        public bool CzySaveButtonAktywny { get; set; } = true;
        public bool CzyProgressBarAtywny { get; set; } = false;
        public string Tytul { get; set; }


        #region Filtr
        public RozliczenieMsAccessFiltr Filtr { get; set; } = new RozliczenieMsAccessFiltr();
        public List<string> ListaPrzychodow { get; set; } = new List<string> { "Linia", "Magazyn" };
        public string WybranyPrzychod { get; set; }
        #endregion
        public List<tblProdukcjaRozliczenie_PWPodsumowanie> PodsumowaniePW_TowarBaza { get; set; } = new List<tblProdukcjaRozliczenie_PWPodsumowanie>();
        #endregion

        #region Commands
        public RelayCommand SprawdzZleceniaCommand { get; set; }
        public RelayCommand RozliczCommand { get; set; }
        public RelayCommand ExportPlikowCommand { get; set; }
        public RelayCommand FiltrujCommand { get; set; }
        #endregion

        public RozliczenieMsAccessViewModel(IViewModelService viewModelService,
                                            IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                            IRozliczenieMsAccesHelper helper,
                                            IKonfekcjaHelper konfekcjaHelper)
            : base(viewModelService)
        {
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
            this.helper = helper;
            this.konfekcjaHelper = konfekcjaHelper;

            RozliczCommand = new RelayCommand(RozliczCommandExecute, RozliczCommandCanExecute);
            SprawdzZleceniaCommand = new RelayCommand(SprawdzZleceniaCommandExecute, SprawdzZleceniaCommandCanExecute);
            ExportPlikowCommand = new RelayCommand(ExportPlikowCommandExecute, ExportPlikowCommandCanExecute);
            FiltrujCommand = new RelayCommand(FiltrujCommandExecute);

            WybranyPrzychod = ListaPrzychodow.First();
            Filtr.Przychod = WybranyPrzychod;

            Tytul = "Rozliczenie produkcji";
        }

        private void FiltrujCommandExecute()
        {
            ListaPozycjiKonfekcjiDoRozliczenia = listaKonfekcjiDoRozliczeniaAll;

            if (ListaPozycjiKonfekcjiDoRozliczenia is null) return;

            if (!string.IsNullOrEmpty(Filtr.TowarNazwa))
            {
                ListaPozycjiKonfekcjiDoRozliczenia = ListaPozycjiKonfekcjiDoRozliczenia.Where(f => f.NazwaTowaruSubiekt.ToLower()
                                                            .Contains(Filtr.TowarNazwa)).ToList();
            }
            if (Filtr.IdZlecenie.HasValue && Filtr.IdZlecenie != 0)
            {
                ListaPozycjiKonfekcjiDoRozliczenia = ListaPozycjiKonfekcjiDoRozliczenia
                    .Where(f => f.IDZlecenie == Filtr.IdZlecenie).ToList();
            }

            if (!string.IsNullOrEmpty(Filtr.Przychod))
            {
                ListaPozycjiKonfekcjiDoRozliczenia = ListaPozycjiKonfekcjiDoRozliczenia
                    .Where(f => f.Przychod.ToLower().Contains(Filtr.Przychod.ToLower())).ToList();
            }
        }

        private bool ExportPlikowCommandCanExecute()
        {
            if (ListaRW == null || !ListaRW.Any()) return false;
            if (ListaPW == null || !ListaPW.Any()) return false;

            return true;
        }
        private async void ExportPlikowCommandExecute()
        {
            string sciezka = helper.DirectoryHelper.GenerujSciezke(ListaPW);

            ExportujPlikXlsx(sciezka);
            await ExportujPlikiEpp(sciezka);

            //DialogService.ShowInfo_BtnOK("Pliki *.epp oraz *xlsx zawierające RW oraz PW zostały zapisane.");
        }

        private void ExportujPlikXlsx(string sciezka)
        {
            try
            {
                string nazwaPliku = $"{DateTime.Now:yyyy-MM-dd}_-_ZP_{ListaRW.First().NrZlecenia}_RW.xlsx";
                helper.ExcelReportGenerator.CreateExcelReport(ListaRW, "RW", null, sciezka + nazwaPliku);

                nazwaPliku = $"{DateTime.Now:yyyy-MM-dd}_-_ZP_{ListaPW.First().NrZlecenia}_PW.xlsx";
                helper.ExcelReportGenerator.CreateExcelReport(PodsumowaniePW_Towar, "PW", null, sciezka + nazwaPliku);
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }

        private async Task ExportujPlikiEpp(string sciezka)
        {
            try
            {
                //RW
                string nazwaPliku = $"{DateTime.Now.ToString("yyyy-MM-dd")}_-_ZP_{ListaRW.First().NrZlecenia}_RW.epp";
                await helper.EppFileGenerator.GenerujPlikEPP(db.Enums.StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, Naglowek, ListaRW, sciezka + nazwaPliku);

                //PW - Magazyn tylko te pozycje ktore nie są taśmami (czyli półproduktem)
                nazwaPliku = $"{DateTime.Now.ToString("yyyy-MM-dd")}_-_ZP_{ListaRW.First().NrZlecenia}_PW_MAG.epp";
                var listaTowarowPW = PodsumowaniePW_Towar.Where(t => !t.NazwaTowaruSubiekt.ToLower().Contains("taśmy"));
                await helper.EppFileGenerator.GenerujPlikEPP(db.Enums.StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, Naglowek, listaTowarowPW, sciezka + nazwaPliku);

                //PW - PółProdukt - tylko taśmy!
                nazwaPliku = $"{DateTime.Now.ToString("yyyy-MM-dd")}_-_ZP_{ListaRW.First().NrZlecenia}_PW_PŁP.epp";
                listaTowarowPW = PodsumowaniePW_Towar.Where(t => t.NazwaTowaruSubiekt.ToLower().Contains("taśmy"));
                if (listaTowarowPW.Sum(s => s.Ilosc) != 0)
                    await helper.EppFileGenerator.GenerujPlikEPP(db.Enums.StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, Naglowek, listaTowarowPW, sciezka + nazwaPliku);
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }
        private bool SprawdzZleceniaCommandCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Pobiera z bazy liste konfekcji dla wybranych parametrow produktu aby okreslic w jakich zleceniach produkt wystepuje.
        /// </summary>
        private async void SprawdzZleceniaCommandExecute()
        {
            await PobierzKonfekcjeDoRozliczenia();
            FiltrujCommandExecute();
        }

        private async Task PobierzKonfekcjeDoRozliczenia()
        {
            try
            {
                ListaKonfekcjiDlaZadanychParametrow = await PobierzKonfekcjeAsync(ProdukcjaRozliczenieStatusEnum.NieRozliczono);
                ListaPozycjiKonfekcjiDoRozliczenia = GenerujListeDoRozliczenia();
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }

        }

        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzKonfekcjeAsync(ProdukcjaRozliczenieStatusEnum produkcjaRozliczenieStatusEnum)
        {
            var lista= await UnitOfWork.tblProdukcjaRuchTowar.WhereAsync(e => e.IDProdukcjaRozliczenieStatus == (int)produkcjaRozliczenieStatusEnum
                                                                       && e.KierunekPrzychodu == "Linia"
                                                                       && e.NrPalety != 0
                                                                       && e.IDZleceniePodstawowe > 0
                                                                       && e.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji);

            if (lista is null || !lista.Any())
                throw new ArgumentException("Brak pozycji PW do rozliczenia");

            return lista;
        }

        private IEnumerable<tblProdukcjaRozliczenie_PW> GenerujListeDoRozliczenia()
        {
            var listaPW = helper.GenerujRozliczeniePW(ListaKonfekcjiDlaZadanychParametrow, 0);
            listaKonfekcjiDoRozliczeniaAll = helper.PodsumujPWPodzialTowar(listaPW);
            return helper.PodsumujPWPodzialTowar(listaPW);
        }

        /// <summary>
        /// Pobiera niezbedne dane z bazy
        /// </summary>
        protected override async void LoadCommandExecute()
        {
            ListaProduktow = await unitOfWorkMsAccess.Artykuly.GetAllAsync();
            listaSurowcowZCenamiSubiekt = await UnitOfWork.vwMagazynRuchGTX.GetAllAsync();

            await helper.LoadAsync();

            SprawdzZleceniaCommandExecute();
        }


        public override void IsChanged_False()
        {
            ListaPWOrg = ListaPW.DeepClone();
            ListaRWOrg = ListaRW.DeepClone();
            NaglowekOrg = Naglowek.DeepClone();
            ListaKonfekcjiDlaZadanychParametrowOrg = ListaKonfekcjiDlaZadanychParametrow.DeepClone();
        }

        protected override bool DeleteCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteCommandExecute()
        {
            throw new NotImplementedException();
        }

        #region Rozliczenie
        #region CanExecute
        private bool RozliczCommandCanExecute()
        {
            if (WybranaPozycjiaKonfekcjiDoRozliczenia is null)
                return false;

            return true;
        }
        #endregion

        #region Execute
        /// <summary>
        /// Rozliczenie RW <see cref="ListaRW"/> oraz PW <see cref="ListaPW"/> bez zapisu w bazie
        /// </summary>
        private async void RozliczCommandExecute()
        {
            CzyscListy();
            UzupelnijNaglowek();

            try
            {
                await GenerujRWAsync();
                await GenerujPWAsync();

                await helper.DodajIlosciKgIWartoscDoRW(ListaRW, ListaPW);

                await GenerujZestawieniaPotrzebneDoExportuPlikow();
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }


        /// <summary>
        /// Aktualizacja naglowka o niezbedne dane oraz wyzerowanie IDProdukcjaRozliczenie_Naglowek
        /// </summary>
        private void UzupelnijNaglowek()
        {
            Naglowek = new tblProdukcjaRozliczenie_Naglowek
            {
                DataDodania = DateTime.Now,
                IDPracownikGAT = UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT ?? 7,
                IDTowarAccess = 0,//WybranyProdukt.Id,
                IDZlecenie = WybranaPozycjiaKonfekcjiDoRozliczenia.IDZlecenie,
                NrZlecenia = WybranaPozycjiaKonfekcjiDoRozliczenia.NrZlecenia,
                TowarNazwa = WybranaPozycjiaKonfekcjiDoRozliczenia.NazwaTowaruSubiekt,
                Szerokosc = WybranaPozycjiaKonfekcjiDoRozliczenia.Szerokosc_m,
                Dlugosc = WybranaPozycjiaKonfekcjiDoRozliczenia.Dlugosc_m,
            };
        }


        /// <summary>
        /// Generuje zestawienie <see cref="PodsumowaniePW_Towar"/> niezbedne do sporzadzenia raportu w formie plikow xlsx oraz epp
        /// </summary>
        private async Task GenerujZestawieniaPotrzebneDoExportuPlikow()
        {
            //PW Towar => Export Excel => Subiekt
            PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>(helper.PodsumujPWPodzialTowar(ListaPW));
            await DodajOdpadDoPodsumowania();

            PodsumowanieRW = helper.PodsumujRW(ListaRW);
            PodsumowaniePW = helper.PodsumujPW(ListaPW);
        }

        private async Task DodajOdpadDoPodsumowania()
        {
            var odpad = await helper.GenerujOdpadDlaPW(ListaRW.First().IDZlecenie);

            if (odpad != null)
                PodsumowaniePW_Towar.Add(odpad);
        }

        /// <summary>
        /// Generuje PW z <see cref="ListaPozycjiKonfekcjiDlaZlecenia"/> z pomoca helpera
        /// </summary>
        /// <returns></returns>
        private async Task GenerujPWAsync()
        {
            ListaPozycjiKonfekcjiDlaZlecenia = await PobierzListeKonfekcjiDlaZlecenia(WybranaPozycjiaKonfekcjiDoRozliczenia.IDZlecenie);
            //Generuje liste PW z pomoca helpera
            var cenaKgMieszanki = helper.GenerujCeneMieszanki(ListaRW);

            ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>
                                            (helper.GenerujRozliczeniePW(ListaPozycjiKonfekcjiDlaZlecenia, cenaKgMieszanki));
        }

        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia)
        {
            if (idZlecenia == 0)
                throw new ArgumentException(nameof(idZlecenia));

            return await UnitOfWork.tblProdukcjaRuchTowar
                                    .WhereAsync(e => e.IDZleceniePodstawowe == idZlecenia
                                                && e.IDProdukcjaRozliczenieStatus == (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono
                                                && e.KierunekPrzychodu == "Linia"
                                                && e.NrPalety != 0
                                                && e.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji);
        }

        /// <summary>
        /// Generuje RW z pomoca helpera
        /// </summary>
        /// <returns></returns>
        private async Task GenerujRWAsync()
        {
            ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>
                        (await helper.GenerujRozliczenieRWAsync(WybranaPozycjiaKonfekcjiDoRozliczenia));
        }


        #endregion
        #endregion

        #region SaveCommand

        #region CanExecute
        protected override bool SaveCommandCanExecute()
        {
            if (ListaRW is null || !ListaRW.Any()) return false;
            if (ListaPW is null || !ListaPW.Any()) return false;
            if (!CzySaveButtonAktywny) return false;

            return true;
        }
        #endregion

        #region Execute
        protected override async void SaveCommandExecute()
        {
            CzySaveButtonAktywny = false;
            CzyProgressBarAtywny = true;

            //ExportPlikowCommandExecute();

            #region SQL w jednej transakcji
            DodajNaglowekDoBazy();
            DodajRWdoBazy();
            DodajPWdoBazy();
            DodajPWPodsumowanieDoBazy();
            UzupelnijStatusRozliczenia();
            await UnitOfWork.SaveAsync();

            #endregion

            CzySaveButtonAktywny = true;
            CzyProgressBarAtywny = false;

            DialogService.ShowInfo_BtnOK("Zlecenie zostało rozliczone i zapisane w bazie.");

            CzyscListy();
            await PobierzKonfekcjeDoRozliczenia();
        }

        private void UzupelnijStatusRozliczenia()
        {
            //foreach (var konfekcja in ListaPozycjiKonfekcjiDlaZlecenia)
            //{
            //    konfekcja.IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.Rozliczono;
            //    var state = UnitOfWork.tblProdukcjaRuchTowar.GetState(konfekcja);
            //}
            ListaPozycjiKonfekcjiDlaZlecenia
                .ToList()
                .ForEach(e => e.IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.Rozliczono);
        }


        /// <summary>
        /// Czysci listy <see cref="ListaRW"/>, <see cref="ListaPW"/>, <see cref="PodsumowaniePW_TowarBaza"/>
        /// </summary>
        public virtual void CzyscListy()
        {
            ListaRW = new ObservableCollection<tblProdukcjaRozliczenie_RW>();
            ListaPW = new ObservableCollection<tblProdukcjaRozliczenie_PW>();
            PodsumowaniePW_Towar = new ObservableCollection<tblProdukcjaRozliczenie_PW>();
            PodsumowaniePW_TowarBaza = new List<tblProdukcjaRozliczenie_PWPodsumowanie>();
            PodsumowanieRW = new RwPodsumowanieModel();
            PodsumowaniePW = new PwPodsumowanieModel();
        }


        /// <summary>
        /// Dodaje do bazy danych -> <see cref="tblProdukcjaRozliczenie_PWPodsumowanie"/> podsumowanie PW 
        /// zeby mozna bylo szybko i sprawnie wyszukiwac rozliczenia po towarach i ilosciach
        /// </summary>
        private void DodajPWPodsumowanieDoBazy()
        {
            foreach (var pozycja in PodsumowaniePW_Towar)
            {
                PodsumowaniePW_TowarBaza.Add(pozycja.Cast<tblProdukcjaRozliczenie_PWPodsumowanie>());
            }

            var pozycjeDoDodania = PodsumowaniePW_TowarBaza.Where(p => p.IDProdukcjaRozliczenie_PWPodsumowanie == 0);

            UnitOfWork.tblProdukcjaRozliczenie_PWPodsumowanie.AddRange(pozycjeDoDodania);
        }


        private void DodajNaglowekDoBazy()
        {

            if (Naglowek.IDProdukcjaRozliczenie_Naglowek == 0)
                UnitOfWork.tblProdukcjaRozliczenie_Naglowek.Add(Naglowek);

            //await unitOfWork.SaveAsync();
        }


        private void DodajPWdoBazy()
        {
            DodajIdNaglowkaDoListyPW(ListaPW);
            var listaElementowDoDodania = ListaPW.Where(s => s.IDProdukcjaRozliczenie_PW == 0);

            if (listaElementowDoDodania.Any())
                UnitOfWork.tblProdukcjaRozliczenie_PW.AddRange(listaElementowDoDodania);
        }

        private void DodajIdNaglowkaDoListyPW(ObservableCollection<tblProdukcjaRozliczenie_PW> listaPW)
        {
            if (Naglowek.IDProdukcjaRozliczenie_Naglowek == 0) return;

            listaPW.ToList()
                   .ForEach(f => f.IDProdukcjaRozliczenie_Naglowek = Naglowek.IDProdukcjaRozliczenie_Naglowek);
        }

        private void DodajRWdoBazy()
        {
            DodajIdNaglowkaDoListyRW(ListaRW);
            var listaElementowDoDodania = ListaRW.Where(s => s.IDProdukcjaRozliczenie_RW == 0);
            if (listaElementowDoDodania.Any())
                UnitOfWork.tblProdukcjaRozliczenie_RW.AddRange(listaElementowDoDodania);
        }

        private void DodajIdNaglowkaDoListyRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRW)
        {
            if (Naglowek.IDProdukcjaRozliczenie_Naglowek == 0) return;

            listaRW.ToList()
                   .ForEach(f => f.IDProdukcjaRozliczenie_Naglowek = Naglowek.IDProdukcjaRozliczenie_Naglowek);
        }
        #endregion

        #endregion
    }
}
