using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using PropertyChanged;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.Helpers.Geokomórka;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Messages;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka
{
    [AddINotifyPropertyChangedInterface]
    public class TowarGeokomorkaViewModel : ViewModelBase, ITowarGeokomorkaViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IGeokomorkaHelper geokomorkaHelper;
        private readonly IMessenger messenger;
        private tblZamowienieHandlowe zamowienieHandlowe;
        private ObservableCollection<tblZamowienieHandloweTowarGeokomorka> listaPozycjiGeokomorek;
        #region Properties
        public IEnumerable<tblTowar> ListaTowarow { get; set; }
        public tblTowar WybranyTowar { get; set; }
        public ObservableCollection<tblZamowienieHandloweTowarGeokomorka> ListaPozycjiGeokomorek
        {
            get => listaPozycjiGeokomorek;
            set
            {
                listaPozycjiGeokomorek = value;

            }
        }
        public tblZamowienieHandloweTowarGeokomorka WybranaPozycjaGeokomorki { get; set; }
        public ObservableCollection<tblTowarGeokomorkaParametryRodzaj> ListaRodzajowGeokomorek { get; set; }
        public tblTowarGeokomorkaParametryRodzaj WybranyRodzaj { get; set; }
        public IEnumerable<tblTowarGeokomorkaParametryZgrzew> ListaZgrzewow { get; set; }
        public tblTowarGeokomorkaParametryZgrzew WybranyZgrzew { get; set; }
        public IEnumerable<tblTowarGeokomorkaParametryTyp> ListaTypowGeokomorek { get; set; }
        public tblTowarGeokomorkaParametryTyp WybranyTypGeokomorki { get; set; }
        public List<tblTowarGeokomorkaParametryGeometryczne> ListaPrametrowGeometrycznychGeokomorki { get; set; }

        public bool IsValid { get; set; }

        public RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiCommand { get; set; }
        public RelayCommand OnAddingNewRowCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand DblClkCommand { get; set; }
        #endregion

        #region CTOR
        public TowarGeokomorkaViewModel(IUnitOfWork unitOfWork,
                                        IViewService viewService,
                                        IDialogService dialogService,
                                        IUnitOfWorkFactory unitOfWorkFactory,
                                        IGeokomorkaHelper geokomorkaHelper,
                                        IMessenger messenger
                                            )

        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.geokomorkaHelper = geokomorkaHelper;
            this.messenger = messenger;

            ZaladujPodczasUruchomieniaCommand = new RelayCommand(ZaladujPodczasUruchomieniaCommandExecute);
            PoEdycjiKomorkiCommand = new RelayCommand(PoEdycjiKomorkiCommandExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            DblClkCommand = new RelayCommand(DblClkCommandExecute);

            messenger.Register<tblZamowienieHandlowe>(this, "PrzeslaneZamowienie", GdyPrzeslanoZamowinieHandlowe);
            messenger.Register<tblZamowienieHandlowe>(this, "ZapiszTowar", GdyPrzeslanoZapisz);

            WybranaPozycjaGeokomorki = new tblZamowienieHandloweTowarGeokomorka();
            ListaPozycjiGeokomorek = new ObservableCollection<tblZamowienieHandloweTowarGeokomorka>();
            ListaPrametrowGeometrycznychGeokomorki = new List<tblTowarGeokomorkaParametryGeometryczne>();
            ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>();
            ListaRodzajowGeokomorek = new ObservableCollection<tblTowarGeokomorkaParametryRodzaj>();
            ListaTypowGeokomorek = new List<tblTowarGeokomorkaParametryTyp>();
            ListaTowarow = new List<tblTowar>();

            //WybranyZgrzew = new tblTowarGeokomorkaParametryZgrzew();
            //WybranyTypGeokomorki = new tblTowarGeokomorkaParametryTyp();
            //WybranyTowar = new tblTowar();
            //WybrayRodzaj = new tblTowarGeokomorkaParametryRodzaj();
        }
        private async void ZaladujPodczasUruchomieniaCommandExecute()
        {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
                ListaTowarow = await unitOfWork.tblTowar.GetAllAsync().ConfigureAwait(false);
                ListaPrametrowGeometrycznychGeokomorki = await unitOfWork.tblTowarGeokomorkaParametryGeometryczne.GetAllAsync().ConfigureAwait(false) as List<tblTowarGeokomorkaParametryGeometryczne>;
                ListaRodzajowGeokomorek = new ObservableCollection<tblTowarGeokomorkaParametryRodzaj>(await unitOfWork.tblTowarGeokomorkaParametryRodzaj.GetAllAsync().ConfigureAwait(false));
                ListaZgrzewow = await unitOfWork.tblTowarGeokomorkaParametryZgrzew.GetAllAsync().ConfigureAwait(false);
                ListaTypowGeokomorek = await unitOfWork.tblTowarGeokomorkaParametryTyp.GetAllAsync().ConfigureAwait(false);
            }

        }

        private void DblClkCommandExecute()
        {
            throw new NotImplementedException();
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć zaznaczoną pozycję?"))
            {
                unitOfWork.tblZamowienieHandloweTowarGeokomorka.Remove(WybranaPozycjaGeokomorki);
                await unitOfWork.SaveAsync();
            }
        }

        private bool UsunCommandCanExecute()
        {
            if (ListaPozycjiGeokomorek.Count() == 0)
                return false;
            if (WybranaPozycjaGeokomorki == null)
                return false;

            return true;
        }
        #endregion

        private async void PoEdycjiKomorkiCommandExecute()
        {
            WalidujModel();

            if (WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryZgrzew != 0)
                WybranyZgrzew = ListaZgrzewow.SingleOrDefault(s => s.IDTowarGeokomorkaParametryZgrzew == WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryZgrzew);

            if (WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryRodzaj != 0)
                WybranyRodzaj = ListaRodzajowGeokomorek.SingleOrDefault(s => s.IDTowarGeokomorkaParametryRodzaj == WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryRodzaj);

            if (WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryTyp != 0)
                WybranyTypGeokomorki = ListaTypowGeokomorek.SingleOrDefault(s => s.IDTowarGeokomorkaParametryTyp == WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryTyp);

            if (WybranyZgrzew == null)
                return;

            if (WybranaPozycjaGeokomorki.IDTowarGeokomorkaParametryZgrzew != 0)
            {
                WstawDlugoscISzerokoscSekcji();
                SprawdzCzyIloscM2godnaZPowierzchniaSekcji();
            }


            if (WybranaPozycjaGeokomorki.IsValid)
            {
                WybranaPozycjaGeokomorki.Waga_kg = geokomorkaHelper.ObliczWage(WybranyZgrzew.Zgrzew,
                                                    WybranyTypGeokomorki.Typ,
                                                    WybranaPozycjaGeokomorki.Wysokosc_mm,
                                                    WybranaPozycjaGeokomorki.Ilosc_m2.GetValueOrDefault());
                WybranaPozycjaGeokomorki.IloscSekcji_szt = geokomorkaHelper.ObliczIloscSekcji(WybranyZgrzew.KodZgrzewu,
                                                    WybranaPozycjaGeokomorki.SzerokoscSekcji_mm,
                                                    WybranaPozycjaGeokomorki.DlugoscSekcji_mm,
                                                    WybranaPozycjaGeokomorki.Ilosc_m2.GetValueOrDefault());

                GenerujNazwePelna();
                await PobierzIDTowaru();
                RaisePropertyChanged(nameof(ListaPozycjiGeokomorek));

                messenger.Send<TabTablesMessage, ZamowienieOdKlientaViewModel>(new TabTablesMessage()
                {
                    IloscPozycjiWTabeli = ListaPozycjiGeokomorek.Count(),
                    KlasaPrzesylajaca = this
                });
            }
        }

        private async Task PobierzIDTowaru()
        {
            var towar = await geokomorkaHelper.PobierzTowarAsync(WybranyRodzaj, WybranyTypGeokomorki, WybranyZgrzew, WybranaPozycjaGeokomorki.Wysokosc_mm);

            if (towar != null)
                WybranaPozycjaGeokomorki.IDTowar = towar.IDTowar;
        }

        private void GenerujNazwePelna()
        {
            WybranaPozycjaGeokomorki.NazwaPelna = geokomorkaHelper.GenerujNazwePelna(WybranyTypGeokomorki.Typ,
                    WybranyRodzaj.Rodzaj,
                    WybranyZgrzew.KodZgrzewu,
                    WybranyZgrzew.Zgrzew,
                    WybranaPozycjaGeokomorki.Wysokosc_mm,
                    WybranaPozycjaGeokomorki.SzerokoscSekcji_mm,
                    WybranaPozycjaGeokomorki.DlugoscSekcji_mm,
                    WybranaPozycjaGeokomorki.Ilosc_m2.GetValueOrDefault());
        }

        private void WalidujModel()
        {
            if (ListaPozycjiGeokomorek.Count() == 0)
            {
                IsValid = true;
                return;
            }

            foreach (var geokomorkaPozycjaDodana in ListaPozycjiGeokomorek)
            {
                if (geokomorkaPozycjaDodana.IsValid)
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }
        }

        private void SprawdzCzyIloscM2godnaZPowierzchniaSekcji()
        {
            if (WybranaPozycjaGeokomorki.Ilosc_m2 == null ||
                WybranaPozycjaGeokomorki.Ilosc_m2 == 0)
                return;

            var iloscPodana = WybranaPozycjaGeokomorki.Ilosc_m2;
            var iloscWlasciwa = geokomorkaHelper.ObliczIloscM2ZgodnaZPowierzchniaSekcji(WybranyZgrzew.KodZgrzewu,
                                                                                    (decimal)WybranaPozycjaGeokomorki.SzerokoscSekcji_mm / 1000,
                                                                                    (decimal)WybranaPozycjaGeokomorki.DlugoscSekcji_mm / 1000,
                                                                                    WybranaPozycjaGeokomorki.Ilosc_m2.GetValueOrDefault());

            if (iloscPodana == iloscWlasciwa)
                return;

            if (dialogService.ShowQuestion_BoolResult("Podana ilość nie jest wyliczona do pełnej sekcji. Czy przeliczyć?"))
                WybranaPozycjaGeokomorki.Ilosc_m2 = iloscWlasciwa;
        }


        private void WstawDlugoscISzerokoscSekcji()
        {
            if (WybranyZgrzew == null)
                return;

            var parametryGeometryczne = geokomorkaHelper.PobierzParametryGeometryczneZeZgrzewu(WybranyZgrzew.KodZgrzewu);
            var szerokoscSekcji = parametryGeometryczne.SzerokoscStandardowaSekcji_m;
            var dlugoscSekcji = parametryGeometryczne.DlugoscStandardowaSekcji_m;

            WybranaPozycjaGeokomorki.SzerokoscSekcji_mm = (int)(szerokoscSekcji * 1000);
            WybranaPozycjaGeokomorki.DlugoscSekcji_mm = (int)(dlugoscSekcji * 1000);
        }


        private async void GdyPrzeslanoZapisz(tblZamowienieHandlowe obj)
        {
            if (ListaPozycjiGeokomorek.Count == 0)
                return;

            foreach (var pozycja in ListaPozycjiGeokomorek)
            {
                pozycja.IDZamowienieHandlowe = zamowienieHandlowe.IDZamowienieHandlowe;

                if (pozycja.IDZamowienieHandloweTowarGeokomorka == 0)
                {
                    unitOfWork.tblZamowienieHandloweTowarGeokomorka.Add(pozycja);
                }

            }
            await unitOfWork.SaveAsync();
        }

        private async void GdyPrzeslanoZamowinieHandlowe(tblZamowienieHandlowe obj)
        {
            zamowienieHandlowe = obj;
            ListaPozycjiGeokomorek = new ObservableCollection<tblZamowienieHandloweTowarGeokomorka>(
                                                                        await unitOfWork.tblZamowienieHandloweTowarGeokomorka
                                                                        .WhereAsync(z => z.IDZamowienieHandlowe == zamowienieHandlowe.IDZamowienieHandlowe));
        }
    }
}
