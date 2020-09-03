using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using System.Collections.ObjectModel;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;
using PropertyChanged;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.Utilities.BarCodeGenerator;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.RuchTowaru;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using System.Runtime.CompilerServices;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.Messages;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.KodKreskowy;
using GAT_Produkcja.ViewModel.Magazyn.Helpers;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;

namespace GAT_Produkcja.ViewModel.Magazyn.RuchTowaru
{
    [AddINotifyPropertyChangedInterface]
    public class RuchTowaruViewModel : ViewModelBase
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMessenger messenger;
        private readonly ITblRuchTowarHelper tblRuchTowarHelper;
        private tblKontrahent kontrahent;
        private tblRuchStatus wybranyStatusRuchu;
        private IEnumerable<tblMagazyn> listaWszystkichMagazynow;
        private tblFirma wybranaFirmaGAT_Z;

        #endregion

        #region Properties
        public IEnumerable<tblFirma> ListaFirmGAT { get; set; }
        public tblFirma WybranaFirmaGAT_Z
        {
            get => wybranaFirmaGAT_Z; set
            {
                wybranaFirmaGAT_Z = value;
                FiltrujListeMagazynowPoZmianieFirmy();
            }
        }

        private tblFirma wybranaFirmaGAT_Do;
        private tblMagazyn wybranyMagazyn_Z;
        private tblMagazyn wybranyMagazyn_Do;

        public tblFirma WybranaFirmaGAT_Do
        {
            get { return wybranaFirmaGAT_Do; }
            set
            {
                wybranaFirmaGAT_Do = value;
                FiltrujListeMagazynowPoZmianieFirmy();
            }
        }

        public IEnumerable<tblTowar> ListaTowarow { get; set; }
        public IEnumerable<string> ListaTowarowString { get; set; } //TextboxAutosuggestion z MaterialDesignExtensions
        public string WybranyString { get; set; }
        public tblTowar WybranyTowar { get; set; }
        public IEnumerable<tblPracownikGAT> ListaPracownikowGAT { get; set; }
        public tblPracownikGAT WybranyPracownikGAT { get; set; }
        public ObservableCollection<tblRuchTowar> ListaRuchuTowarow { get; set; }
        public tblRuchTowar WybranyTowarRuch { get; set; }
        public tblRuchNaglowek RuchNaglowek { get; set; }
        public IEnumerable<tblRuchStatus> ListaStatusowRuchu { get; set; }
        public tblRuchStatus WybranyStatusRuchu
        {
            get => wybranyStatusRuchu;
            set
            {
                wybranyStatusRuchu = value;
                //if (wybranyStatusRuchu!=null)
                //{
                //    OkreslAktywnyStatus((StatusRuchuTowarowEnum)WybranyStatusRuchu.IDRuchStatus);
                //    messenger.Send((StatusRuchuTowarowEnum)WybranyStatusRuchu?.IDRuchStatus);
                //}

            }
        }

        public IEnumerable<tblJm> ListaJm { get; set; }
        public tblJm WybranaJm { get; set; }
        public IEnumerable<tblMagazyn> ListaMagazynow_Z { get; set; }
        public IEnumerable<tblMagazyn> ListaMagazynow_Do { get; set; }
        public IEnumerable<tblDokumentTyp> ListaTypowDokumentow { get; set; }
        public tblMagazyn WybranyMagazyn_Z
        {
            get => wybranyMagazyn_Z;

            set
            {
                wybranyMagazyn_Z = value;
                if (wybranyMagazyn_Z != null)
                {
                    IsDataGridEnabled = true;
                    ToolTipDataGrid = String.Empty;
                }
            }
        }
        public tblMagazyn WybranyMagazyn_Do
        {
            get => wybranyMagazyn_Do;
            set
            {
                wybranyMagazyn_Do = value;
                if (wybranyMagazyn_Do != null)
                {
                    IsDataGridEnabled = true;
                    ToolTipDataGrid = String.Empty;
                }
            }
        }
        public string Tytul { get; set; }
        public string KontrahentPelnaNazwa { get; set; }
        public IEnumerable<tblProdukcjaZlecenie> ListaZlecenProdukcyjnych { get; set; }
        public tblProdukcjaZlecenie WybraneZlecenieProdukcyjne { get; set; }


        public bool CzyStatusPZ { get; set; }
        public bool CzyStatusRW { get; set; }
        public bool CzyStatusMM_PW { get; set; }
        public bool CzyStatusWZ { get; set; }
        public string ToolTipZapiszCommand { get; set; }
        public bool IsDataGridEnabled { get; set; }
        public string ToolTipDataGrid { get; set; } = "Proszę wybrać magazyn/y";
        #endregion

        #region Commands
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand SzukajTowarCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiDataGridCommand { get; set; }
        public RelayCommand PobierzDaneZKoduKreskowegoCommand { get; set; }
        public RelayCommand UsunTowarRuchCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand GenerujKodKreskowyCommand { get; set; }
        public RelayCommand PokazEwidencjeKontrahentowCommand { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
        public RelayCommand DrukujKodKreskowyCommand { get; set; }
        public RelayCommand PokazOknoDoWpisywaniaKoduKreskowegoCommand { get; set; }
        #endregion

        #region CTOR
        public RuchTowaruViewModel(IUnitOfWork unitOfWork,
                                   IViewService viewService,
                                   IDialogService dialogService,
                                   IUnitOfWorkFactory unitOfWorkFactory,
                                   IMessenger messenger,
            ITblRuchTowarHelper tblRuchTowarHelper)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.messenger = messenger;
            this.tblRuchTowarHelper = tblRuchTowarHelper;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            SzukajTowarCommand = new RelayCommand(SzukajTowarCommandExecute);
            PoEdycjiKomorkiDataGridCommand = new RelayCommand(PoEdycjiKomorkiDataGridCommandExecute);
            PobierzDaneZKoduKreskowegoCommand = new RelayCommand(PobierzDaneZKoduKreskowegoCommandExecute, PobierzDaneZKoduKreskowegoCommandCanExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunTowarRuchCommand = new RelayCommand(UsunTowarRuchCommandExecute, UsunTowarRuchCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            GenerujKodKreskowyCommand = new RelayCommand(GenerujKodKreskowyCommandExecute, GenerujKodKreskowyCommandCanExecute);
            PokazEwidencjeKontrahentowCommand = new RelayCommand(PokazEwidencjeKontrahentowCommandExecute);
            DrukujKodKreskowyCommand = new RelayCommand(DrukujKodKreskowyCommandExecute);
            PokazOknoDoWpisywaniaKoduKreskowegoCommand = new RelayCommand(PokazOknoDoWpisywaniaKoduKreskowegoCommandExecute);



            messenger.Register<vwStanTowaru>(this, GdyPrzeslanoTowar);
            messenger.Register<tblTowar>(this, GdyPrzeslanoTowar);
            messenger.Register<tblKontrahent>(this, GdyPrzeslanoKontrahenta);
            messenger.Register<StatusRuchuTowarowEnum>(this, GdyPrzeslanoStatusRuchu);
            messenger.Register<string>(this,"KodKreskowy", GdyPrzeslanoKodKreskowy);


            RuchNaglowek = new tblRuchNaglowek();
            RuchNaglowek.DataPrzyjecia = DateTime.Now;
            ListaRuchuTowarow = new ObservableCollection<tblRuchTowar>();
            WybranyTowarRuch = new tblRuchTowar();
            WybranyStatusRuchu = new tblRuchStatus();
        }

        private void PokazOknoDoWpisywaniaKoduKreskowegoCommandExecute()
        {
            viewService.ShowDialog<RuchTowaruKodKreskowyDodajViewModel>();
        }

        private async void GdyPrzeslanoKodKreskowy(string obj)
        {
            var towaryODanymKodzieKreskowym = await unitOfWork.tblRuchTowar.WhereAsync(t => t.NrParti == obj);
            var towarZNajnowszaData = towaryODanymKodzieKreskowym.OrderByDescending(d => d.tblRuchNaglowek.DataPrzyjecia).Distinct();

        }

        private void DrukujKodKreskowyCommandExecute()
        {
            throw new NotImplementedException();
        }

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            ListaFirmGAT = await unitOfWork.tblFirma.GetAllAsync().ConfigureAwait(false);
            ListaPracownikowGAT = await unitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync().ConfigureAwait(false);
            ListaJm = await unitOfWork.tblJm.GetAllAsync().ConfigureAwait(false);
            listaWszystkichMagazynow = await unitOfWork.tblMagazyn.GetAllAsync().ConfigureAwait(false);
            ListaTypowDokumentow = await unitOfWork.tblDokumentTyp.GetAllAsync().ConfigureAwait(false);
            ListaZlecenProdukcyjnych = await unitOfWork.tblProdukcjaZlecenie.GetAllAsync().ConfigureAwait(false);

            if (ListaPracownikowGAT != null)
                WybranyPracownikGAT = ListaPracownikowGAT.FirstOrDefault(p => p.ID_PracownikGAT == UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT);

            if (ListaStatusowRuchu == null)
                ListaStatusowRuchu = await unitOfWork.tblRuchStatus.GetAllAsync().ConfigureAwait(false);
        }
        #endregion

        #region Status Ruchu
        private async void GdyPrzeslanoStatusRuchu(StatusRuchuTowarowEnum obj)
        {
            if (ListaStatusowRuchu == null)
            {
                using (var uow = unitOfWorkFactory.Create())
                {
                    ListaStatusowRuchu = await uow.tblRuchStatus.GetAllAsync().ConfigureAwait(false);
                }
            }
            OkreslAktywnyStatus(obj);
            await NrDokumentuGenerator();
        }

        private void OkreslAktywnyStatus(StatusRuchuTowarowEnum? obj)
        {
            if (obj == null)
                return;

            ResetStatusow();

            switch (obj)
            {
                case StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ:
                    CzyStatusPZ = true;
                    WybierzStatus("PZ");
                    break;
                case StatusRuchuTowarowEnum.RozchodWewnetrzny_RW:
                    CzyStatusRW = true;
                    WybierzStatus("RW");
                    break;
                case StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM:
                    CzyStatusMM_PW = true;
                    WybierzStatus("MM");
                    break;
                case StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW:
                    CzyStatusMM_PW = true;
                    WybierzStatus("PW");
                    break;
                case StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ:
                    CzyStatusWZ = true;
                    WybierzStatus("WZ");
                    break;
                default:
                    break;
            }
        }
        private void WybierzStatus(string skrotStatusu)
        {
            WybranyStatusRuchu = ListaStatusowRuchu.FirstOrDefault(s => s.Symbol.Contains(skrotStatusu));
        }
        private void ResetStatusow()
        {
            CzyStatusMM_PW = false;
            CzyStatusPZ = false;
            CzyStatusWZ = false;
        }

        #endregion

        #region Kontrahent
        private void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            viewService.Close<EwidencjaKontrahentowViewModel_old>();
            viewService.Close<DodajKontrahentaViewModel_old>();

            kontrahent = obj;
            RuchNaglowek.IDKontrahent = obj.ID_Kontrahent;
            KontrahentPelnaNazwa = kontrahent.Nazwa + ", " + kontrahent.Ulica + ", " + kontrahent.KodPocztowy + " " + kontrahent.Miasto;
        }

        private void PokazEwidencjeKontrahentowCommandExecute()
        {
            viewService.ShowDialog<EwidencjaKontrahentowViewModel_old>();
        }
        #endregion

        #region KodKreskowy
        private bool GenerujKodKreskowyCommandCanExecute()
        {
            if (WybranyTowarRuch == null)
                return false;

            return true;
        }

        private void GenerujKodKreskowyCommandExecute()
        {
            WybranyTowarRuch.NrParti = BarCodeGenerator.GetUniqueId();
        }

        #endregion

        #region KodKreskowy
        private bool PobierzDaneZKoduKreskowegoCommandCanExecute()
        {
            if (WybranyTowarRuch == null)
                return false;

            if (String.IsNullOrEmpty(WybranyTowarRuch.NrParti))
                return false;

            return true;
        }

        private async void PobierzDaneZKoduKreskowegoCommandExecute()
        {
            WybranyTowarRuch = await unitOfWork.tblRuchTowar.SingleOrDefaultAsync(t => t.NrParti == WybranyTowarRuch.NrParti);
            if (WybranyTowarRuch != null)
            {
                var towar = await unitOfWork.tblTowar.GetByIdAsync(WybranyTowarRuch.IDTowar.GetValueOrDefault());
                var jm = await unitOfWork.tblJm.GetByIdAsync(WybranyTowarRuch.IDJm.GetValueOrDefault());
                WybranyTowarRuch.TowarNazwa = towar.Nazwa;
                WybranyTowarRuch.Jm = jm.Jm;
            }
        }

        #endregion

        #region PoEdycjiKomorkiWDataGrid PrzeliczeniaIlosci
        private async void PoEdycjiKomorkiDataGridCommandExecute()
        {
            if (WybranyTowarRuch.IDTowar == 0 ||
                RuchNaglowek.IDMagazynDo == 0)
                return;

            WybranyTowarRuch.IloscPrzed = await ObliczIloscDostepnaNaMagazynie(WybranyTowarRuch.IDTowar.GetValueOrDefault(),
                                                                               RuchNaglowek.IDMagazynDo.GetValueOrDefault());

            switch (WybranyStatusRuchu.IDRuchStatus)
            {
                case (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ:

                    WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed + WybranyTowarRuch.Ilosc;
                    break;
                case (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW:
                    WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed + WybranyTowarRuch.Ilosc;
                    break;
                case (int)StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ:
                    //WybranyTowarRuch.IloscPrzed = await ObliczIloscDostepnaNaMagazynie(WybranyTowarRuch.IDTowar.GetValueOrDefault(),
                    //                                               RuchNaglowek.IDMagazynZ.GetValueOrDefault());
                    WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed - WybranyTowarRuch.Ilosc;
                    break;
                case (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW:
                    WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed - WybranyTowarRuch.Ilosc;
                    break;
                case (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM:
                    WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed + WybranyTowarRuch.Ilosc;
                    break;
                default:
                    break;
            }
            //WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed - WybranyTowarRuch.Ilosc;
        }

        private async Task<decimal> ObliczIloscDostepnaNaMagazynie(int idTowar, int idMagazyn, bool czyOdjacIlosciZarezerwowane = false)
        {
            var listaIlosciDostepnychDlatowaru = await unitOfWork.tblRuchTowar.WhereAsync(t => t.IDTowar == idTowar && t.IDMagazyn == idMagazyn);
            decimal iloscDostepna = 0;
            if (czyOdjacIlosciZarezerwowane)
                iloscDostepna = listaIlosciDostepnychDlatowaru.Sum(s => s.Ilosc) - listaIlosciDostepnychDlatowaru.Sum(s => s.IloscZarezerwowana).GetValueOrDefault();
            else
                iloscDostepna = listaIlosciDostepnychDlatowaru.Sum(s => s.Ilosc);

            return iloscDostepna;
        }
        #endregion

        #region Towar
        private void GdyPrzeslanoTowar(vwStanTowaru obj)
        {
            if (obj == null)
                return;

            viewService.Close<MagazynStanTowaruViewModel>();

            WybranyTowarRuch.IDTowar = obj.IDTowar;
            WybranyTowarRuch.TowarNazwa = obj.Nazwa;
            WybranyTowarRuch.IDMagazyn = obj.IDMagazyn;
            WybranyTowarRuch.IloscPrzed = obj.IloscDostepna;
            WybranyTowarRuch.IDJm = obj.IDJm;
            WybranyTowarRuch.Jm = obj.Jm;

            RaisePropertyChanged(nameof(WybranyTowarRuch));
        }

        private void GdyPrzeslanoTowar(tblTowar obj)
        {
            if (obj == null)
                return;

            viewService.Close<TowarEwidencjaViewModel>();

            WybranyTowarRuch.IDTowar = obj.IDTowar;
            WybranyTowarRuch.TowarNazwa = obj.Nazwa;
            WybranyTowarRuch.IDJm = obj.IDJm;
        }

        private void SzukajTowarCommandExecute()
        {
            if (WybranyStatusRuchu == null ||
               WybranyStatusRuchu.IDRuchStatus == (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ)
            {
                viewService.Show<TowarEwidencjaViewModel>();
            }
            else
            {
                viewService.Show<MagazynStanTowaruViewModel>();
                messenger.Send<MagazynMessage>(new MagazynMessage
                {
                    IdMagazyn = RuchNaglowek.IDMagazynZ.GetValueOrDefault()
                });
            }
        }

        #endregion

        #region ZapiszCommand
        private bool ZapiszCommandCanExecute()
        {
            if (RuchNaglowek == null)
                return false;

            if (!RuchNaglowek.IsValid)
            {
                ToolTipZapiszCommand = "Proszę uzupełnić pola formularza";
                return false;
            }

            if ((StatusRuchuTowarowEnum)WybranyStatusRuchu.IDRuchStatus == StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
            {
                if (RuchNaglowek.IDProdukcjaZlecenieProdukcyjne == null ||
                    RuchNaglowek.IDProdukcjaZlecenieProdukcyjne == 0)
                {
                    ToolTipZapiszCommand = "Proszę uzupełnić nr zlecenia produkcyjnego";
                    return false;
                }
            }

            if (ListaRuchuTowarow == null ||
                ListaRuchuTowarow.Count() == 0)
                return false;

            foreach (var towar in ListaRuchuTowarow)
            {
                if (!towar.IsValid)
                {
                    ToolTipZapiszCommand = "Proszę uzupełnić pola w tabeli";
                    return false;
                }
            }

            return true;
        }

        private async void ZapiszCommandExecute()
        {
            if (RuchNaglowek.IDRuchNaglowek == 0)
                unitOfWork.tblRuchNaglowek.Add(RuchNaglowek);

            await unitOfWork.SaveAsync();

            foreach (var towar in ListaRuchuTowarow)
            {
                towar.IDRuchNaglowek = RuchNaglowek.IDRuchNaglowek;

                if (towar.IDRuchTowar == 0)
                {
                    await tblRuchTowarHelper.DodajDoBazyDanych(towar, WybranyStatusRuchu, RuchNaglowek);
                }
            }
            await unitOfWork.SaveAsync();

            //foreach (var towar in ListaRuchuTowarow)
            //{
            //    towar.IDRuchNaglowek = RuchNaglowek.IDRuchNaglowek;
            //    if (towar.IDRuchTowar == 0)
            //        unitOfWork.tblRuchTowar.Add(towar);
            //}

            dialogService.ShowInfo_BtnOK("Pozcja została zapisana");
            WyslijWiadomoscOdswiezDoEwidencjiruchuMagazynowego();
            ZamknijFormularz();
        }

        private async Task OkreslIlosciPrzedIPoDlatowaru(tblRuchTowar towar, int idMagazynu, bool czyOdejmujemyZMagazynu)
        {
            towar.IDMagazyn = idMagazynu;

            var towarRuch = await unitOfWork.tblRuchTowar.WhereAsync(t => t.IDTowar == towar.IDTowar && t.IDMagazyn == towar.IDMagazyn).ConfigureAwait(false);

            towar.IloscPrzed = towarRuch.Sum(s => s.Ilosc);
            if (czyOdejmujemyZMagazynu)
            {
                if (towar.Ilosc > 0)
                {
                    towar.Ilosc = decimal.Negate(towar.Ilosc);
                }
            }

            towar.IloscPo = towar.IloscPrzed + towar.Ilosc;
        }
        #endregion

        #region UsunTowarRuchCommand
        private bool UsunTowarRuchCommandCanExecute()
        {
            if (WybranyTowarRuch == null)
                return false;

            return true;
        }

        private async void UsunTowarRuchCommandExecute()
        {
            if (WybranyTowarRuch.IDRuchTowar != 0)
            {
                unitOfWork.tblRuchTowar.Remove(WybranyTowarRuch);
                await unitOfWork.SaveAsync();
            }
            ListaRuchuTowarow.Remove(WybranyTowarRuch);

            if (ListaRuchuTowarow.Count() == 0)
                ListaRuchuTowarow.Add(new tblRuchTowar());
        }

        #endregion

        #region UsunCommand
        private async void UsunCommandExecute()
        {

            if (dialogService.ShowQuestion_BoolResult("Czy usunąć bieżący ruch magazynowy?"))
            {
                UsunListeRuchuTowarow();
                UsunNaglowekRuchuTowaru();

                await unitOfWork.SaveAsync();
                dialogService.ShowInfo_BtnOK("Bieżąca pozycja została usunięta");
                WyslijWiadomoscOdswiezDoEwidencjiruchuMagazynowego();
                ZamknijFormularz();
            }
        }

        private void ZamknijFormularz()
        {
            viewService.Close<RuchTowaruViewModel>();
        }

        private void WyslijWiadomoscOdswiezDoEwidencjiruchuMagazynowego()
        {
            messenger.Send<string, MagazynRuchTowaruViewModel>("Odswiez");
        }

        private void UsunNaglowekRuchuTowaru()
        {
            unitOfWork.tblRuchNaglowek.Remove(RuchNaglowek);
        }

        private void UsunListeRuchuTowarow()
        {
            if (ListaRuchuTowarow.Count() != 0)
            {
                foreach (var towar in ListaRuchuTowarow)
                {
                    if (towar.IDRuchTowar != 0)
                        unitOfWork.tblRuchTowar.Remove(towar);
                }
            }
        }

        private bool UsunCommandCanExecute()
        {
            if (RuchNaglowek.IDRuchNaglowek == 0)
                return false;

            return true;
        }

        #endregion

        #region NrDokumentuGenerator
        private async Task NrDokumentuGenerator()
        {
            if (WybranyStatusRuchu == null)
                return;
            if (WybranyStatusRuchu.Symbol == null)
                return;

            int nrDokumentu = await PobierzNowyNrDokumentu();

            if (nrDokumentu == 0)
                return;
            RuchNaglowek.NrDokumentu = nrDokumentu;
            RuchNaglowek.NrDokumentuPelny = $"{WybranyStatusRuchu.Symbol.Trim()} {nrDokumentu}/{DateTime.Now.Year}";

            RaisePropertyChanged(nameof(RuchNaglowek));

        }

        private async Task<int> PobierzNowyNrDokumentu()
        {
            int nrDokumentu = 0;
            using (var uow = unitOfWorkFactory.Create())
            {
                nrDokumentu = await uow.tblRuchNaglowek.PobierzNrDokumentuWewnetrznegoAsync(WybranyStatusRuchu.IDRuchStatus);
            }

            return nrDokumentu;
        }
        #endregion

        #region Magazyny
        private void FiltrujListeMagazynowPoZmianieFirmy([CallerMemberName] string wybranyMagazynPropName = "")
        {
            if (wybranyMagazynPropName == nameof(WybranaFirmaGAT_Z))
                ListaMagazynow_Z = listaWszystkichMagazynow.Where(m => m.IDFirma == WybranaFirmaGAT_Z.IDFirma);
            else
                ListaMagazynow_Do = listaWszystkichMagazynow.Where(m => m.IDFirma == WybranaFirmaGAT_Do.IDFirma);
        }
        #endregion
    }
}
