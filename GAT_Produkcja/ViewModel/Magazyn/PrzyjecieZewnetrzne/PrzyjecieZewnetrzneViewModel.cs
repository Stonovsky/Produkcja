using GalaSoft.MvvmLight;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.PrzyjecieZewnetrzne.DodajTowarDoMagazynu;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using System.Windows.Controls;
using GAT_Produkcja.Utilities.BarCodeGenerator;
using PropertyChanged;

namespace GAT_Produkcja.ViewModel.Magazyn.PrzyjecieZewnetrzne
{
    [AddINotifyPropertyChangedInterface]
    public class PrzyjecieZewnetrzneViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// Ponizsze sprawdza czy [AddINotifyPropertyChangedInterface] dziala na prop bez propf - dziala
        /// </summary>
        public IEnumerable<tblFirma> ListaFirmGAT { get; set; }
        //private IEnumerable<tblFirma> listaFirmGAT;
        private tblFirma wybranaFirmaGAT;
        private tblKontrahent kontrahent;
        private IEnumerable<tblRuchStatus> listaRuchStatus;
        private tblRuchStatus wybranyRuchStatus;
        private IEnumerable<tblMagazyn> listaMagazynow;
        private tblMagazyn wybranyMagazyn;
        private tblPracownikGAT wybranyPracownikGAT;
        private IEnumerable<tblPracownikGAT> listaPracownikowGAT;
        private ObservableCollection<tblRuchTowar> listaTowarowRuch;
        private tblRuchTowar wybranyTowarRuch;
        private tblRuchNaglowek naglowekRuch;
        private string tytul;
        private IEnumerable<tblJm> listaJm;
        private tblJm wybranaJM;
        private IEnumerable<tblTowar> listaTowarow;
        private tblTowar wybranyTowar;

        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;

        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand PokazEwidencjeKontrahentowCommand { get; set; }
        public RelayCommand OtworzSzczegolyTowaruCommand { get; set; }
        public RelayCommand DodajTowarCommand { get; set; }
        public RelayCommand UsunTowarCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand PokazEwidencjeTowarowCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiDataGridCommand { get; set; }
        public RelayCommand<AddingNewItemEventArgs> OnAddingNewRowCommand { get; set; }

        public RelayCommand DrukujKodKreskowyCommand { get; set; }
        public RelayCommand GenerujKodKreskowyCommand { get; set; }

        public RelayCommand PrzyZmianieTekstuComboboxaCommand { get; set; }

        #region PropFull
        public IEnumerable<tblTowar> ListaTowarow
        {
            get { return listaTowarow; }
            set { listaTowarow = value; RaisePropertyChanged(); }
        }


        public tblTowar WybranyTowar
        {
            get { return wybranyTowar; }
            set { wybranyTowar = value; RaisePropertyChanged(); }
        }


        public tblRuchNaglowek NaglowekRuch
        {
            get { return naglowekRuch; }
            set { naglowekRuch = value; RaisePropertyChanged(); }
        }


        public IEnumerable<tblJm> ListaJm
        {
            get { return listaJm; }
            set { listaJm = value; RaisePropertyChanged(); }
        }


        public tblJm WybranaJm
        {
            get { return wybranaJM; }
            set { wybranaJM = value; RaisePropertyChanged(); }
        }


        //public IEnumerable<tblFirma> ListaFirmGAT
        //{
        //    get { return listaFirmGAT; }
        //    set { listaFirmGAT = value; RaisePropertyChanged(); }
        //}

        public tblFirma WybranaFirmaGAT
        {
            get { return wybranaFirmaGAT; }
            set { wybranaFirmaGAT = value; RaisePropertyChanged(); }
        }

        public tblKontrahent Kontrahent
        {
            get { return kontrahent; }
            set { kontrahent = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblRuchStatus> ListaRuchStatus
        {
            get { return listaRuchStatus; }
            set { listaRuchStatus = value; RaisePropertyChanged(); }
        }

        public tblRuchStatus WybranyRuchStatus
        {
            get { return wybranyRuchStatus; }
            set { wybranyRuchStatus = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblMagazyn> ListaMagazynow
        {
            get { return listaMagazynow; }
            set { listaMagazynow = value; RaisePropertyChanged(); }
        }

        public tblMagazyn WybranyMagazyn
        {
            get { return wybranyMagazyn; }
            set { wybranyMagazyn = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblPracownikGAT> ListaPracownikowGAT
        {
            get { return listaPracownikowGAT; }
            set { listaPracownikowGAT = value; RaisePropertyChanged(); }
        }

        public tblPracownikGAT WybranyPracownikGAT
        {
            get { return wybranyPracownikGAT; }
            set { wybranyPracownikGAT = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<tblRuchTowar> ListaTowarowRuch
        {
            get { return listaTowarowRuch; }
            set { listaTowarowRuch = value; RaisePropertyChanged(); }
        }

        public tblRuchTowar WybranyTowarRuch
        {
            get { return wybranyTowarRuch; }
            set { wybranyTowarRuch = value; RaisePropertyChanged(); }
        }

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        private string toolTipDodajTowar;

        public string ToolTipDodajTowar
        {
            get { return toolTipDodajTowar; }
            set { toolTipDodajTowar = value; RaisePropertyChanged(); }
        }

        public string TooltipZapisz { get; private set; }


        #endregion
        #endregion

        #region CTOR
        public PrzyjecieZewnetrzneViewModel(IUnitOfWorkFactory unitOfWorkFactory,
                                            IViewService viewService,
                                            IDialogService dialogService,
                                            IMessenger messenger)
        {
            unitOfWork = unitOfWorkFactory.Create();
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            PokazEwidencjeKontrahentowCommand = new RelayCommand(PokazEwidencjeKontrahentowCommandExecute);
            DodajTowarCommand = new RelayCommand(DodajTowarCommandExecute, DodajTowarCommandCanExecute);
            UsunTowarCommand = new RelayCommand(UsunTowarCommandExecute, UsunTowarCommandCanExecute);
            PokazEwidencjeTowarowCommand = new RelayCommand(PokazEwidencjeTowarowCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            PoEdycjiKomorkiDataGridCommand = new RelayCommand(PoEdycjiKomorkiDataGridCommandExecute);
            OnAddingNewRowCommand = new RelayCommand<AddingNewItemEventArgs>(e => OnAddingNewRowCommandExecute(e));
            DrukujKodKreskowyCommand = new RelayCommand(DrukujKodKreskowyCommandExecute, DrukujKodKreskowyCommandCanExecute);
            GenerujKodKreskowyCommand = new RelayCommand(GenerujKodKreskowyCommandExecute, GenerujKodKreskowyCommandCanExecute);
            PrzyZmianieTekstuComboboxaCommand = new RelayCommand(PrzyZmianieTekstuComboboxaCommandExecute);

            messenger.Register<tblKontrahent>(this, GdyPrzeslanoKontrahenta);
            messenger.Register<tblRuchTowar>(this, GdyPrzeslanoRuchTowaru);

            NaglowekRuch = new tblRuchNaglowek();
            //NaglowekRuch.MetaSetUp();

            if (UzytkownikZalogowany.Uzytkownik != null)
                NaglowekRuch.ID_PracownikGAT = UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT;
            
            ListaTowarowRuch = new ObservableCollection<tblRuchTowar>();
        }

        private void PrzyZmianieTekstuComboboxaCommandExecute()
        {
        }

        private bool GenerujKodKreskowyCommandCanExecute()
        {
            if (WybranyTowarRuch == null)
                return false;

            return true;
        }

        private bool DrukujKodKreskowyCommandCanExecute()
        {
            if (WybranyTowarRuch == null)
                return false;

            if (String.IsNullOrEmpty(WybranyTowarRuch.NrParti))
                return false;

            return true;
        }

        private void GenerujKodKreskowyCommandExecute()
        {
            WybranyTowarRuch.NrParti = BarCodeGenerator.GetUniqueId();
        }

        private void DrukujKodKreskowyCommandExecute()
        {
        }

        private void OnAddingNewRowCommandExecute(AddingNewItemEventArgs e)
        {
            //var nowyTowar = new tblRuchTowar();
            //nowyTowar.MetaSetUp();
            //e.NewItem = nowyTowar;

            //e.NewItem = new tblRuchTowar();
            //tblRuchTowar t = (tblRuchTowar)e.NewItem;
            //t.Ilosc = 0;
            //WybranyTowarRuch = new tblRuchTowar();
            //WybranyTowarRuch.Ilosc = 0;
            //WybranyTowarRuch.IDTowar = 1;
        }

        private async void PoEdycjiKomorkiDataGridCommandExecute()
        {
            if (WybranyTowarRuch == null)
                return;

            var pozycjeDlaTowaru = await unitOfWork.tblRuchTowar.WhereAsync(t => t.IDTowar == WybranyTowarRuch.IDTowar);
            WybranyTowarRuch.IloscPrzed = pozycjeDlaTowaru.Sum(s => s.Ilosc);
            WybranyTowarRuch.IloscPo = WybranyTowarRuch.IloscPrzed + ListaTowarowRuch.Where(t=>t.IDTowar==WybranyTowarRuch.IDTowar).Sum(s=>s.Ilosc) ?? 0;
        }

        private void PokazEwidencjeTowarowCommandExecute()
        {
            viewService.Show<TowarEwidencjaViewModel>();
        }
        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            ListaFirmGAT = await unitOfWork.tblFirma.GetAllAsync();
            ListaMagazynow = await unitOfWork.tblMagazyn.GetAllAsync();
            ListaPracownikowGAT = await unitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync();
            ListaRuchStatus = await unitOfWork.tblRuchStatus.GetAllAsync();
            ListaJm = await unitOfWork.tblJm.GetAllAsync();
            ListaJm = ListaJm.OrderBy(n => n.Jm);
            ListaTowarow = await unitOfWork.tblTowar.GetAllAsync();
            ListaTowarow = ListaTowarow.OrderBy(n => n.Nazwa);
            WybranyRuchStatus = ListaRuchStatus.Single(s => s.Status.Contains("PZ"));
            WybranyPracownikGAT = UzytkownikZalogowany.Uzytkownik;
        }
        private async void GdyPrzeslanoRuchTowaru(tblRuchTowar obj)
        {
            var jm = ListaJm.Where(l => l.IDJm == obj.IDJm).SingleOrDefault();
            obj.Jm = jm.Jm;

            var towar = await unitOfWork.tblTowar.GetByIdAsync(obj.IDTowar.GetValueOrDefault());
            obj.TowarNazwa = towar.Nazwa;

            ListaTowarowRuch.Add(obj);
        }
        private void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            Kontrahent = obj;
            NaglowekRuch.IDKontrahent = Kontrahent.ID_Kontrahent;

            viewService.Close<DodajKontrahentaViewModel_old>();
            viewService.Close<EwidencjaKontrahentowViewModel_old>();
        }
        #endregion

        #region Commands
        private bool ZapiszCommandCanExecute()
        {
            if (!NaglowekRuch.IsValid)
            {
                TooltipZapisz = "Uzupełnij wskazane pola jako wymagane";
                return false;

            }

            if (ListaTowarowRuch.Count() == 0)
            {
                TooltipZapisz = "Brak dodanych towarów";
                return false;
            }

            foreach (var towar in ListaTowarowRuch)
            {
                if (!towar.IsValid)
                {
                    TooltipZapisz = "Uzupełnij wszystkie niezbędne dane dla wszystkich towarów";
                    return false;
                }
            }

            return true;
        }

        private async void ZapiszCommandExecute()
        {
            ///Zapisz nagłówka
            if (NaglowekRuch.IDRuchNaglowek == 0)
            {
                unitOfWork.tblRuchNaglowek.Add(NaglowekRuch);
            }
            await unitOfWork.SaveAsync();

            ///Zapis towarow
            if (ListaTowarowRuch.Count() > 0)
            {
                foreach (var towar in ListaTowarowRuch)
                {
                    if (towar.IDRuchTowar == 0)
                    {
                        towar.IDRuchNaglowek = NaglowekRuch.IDRuchNaglowek;
                        unitOfWork.tblRuchTowar.Add(towar);
                    }
                }
            }
            await unitOfWork.SaveAsync();

        }

        private bool UsunCommandCanExecute()
        {
            if (NaglowekRuch.IDRuchNaglowek == 0)
            {
                return false;
            }

            return true;
        }

        private void UsunCommandExecute()
        {
            throw new NotImplementedException();
        }

        private bool UsunTowarCommandCanExecute()
        {
            return true;
        }

        private async void UsunTowarCommandExecute()
        {
            if (WybranyTowarRuch.IDRuchTowar != 0)
            {
                if (dialogService.ShowQuestion_BoolResult("Czy usunąć pozycję z bazy?"))
                {
                    unitOfWork.tblRuchTowar.Remove(WybranyTowarRuch);
                    await unitOfWork.SaveAsync();
                }
            }

            ListaTowarowRuch.Remove(WybranyTowarRuch);
        }
        private bool DodajTowarCommandCanExecute()
        {
            //if (Kontrahent == null ||
            //    Kontrahent.ID_Kontrahent == 0)
            //{
            //    ToolTipDodajTowar = "Dodaj kontrahenta przed dodaniem towaru";
            //    return false;
            //}

            return true;
        }

        private void DodajTowarCommandExecute()
        {
            viewService.Show<DodajTowarDoMagazynuViewModel>();
        }

        private void PokazEwidencjeKontrahentowCommandExecute()
        {
            viewService.Show<EwidencjaKontrahentowViewModel_old>();
        }


        #endregion
    }
}
