using GalaSoft.MvvmLight;
using PropertyChanged;
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
using GAT_Produkcja.Helpers.Geokomórka;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka.Dodaj
{
    [AddINotifyPropertyChangedInterface]
    public class TowarGeokomorkaDodajViewModel : ViewModelBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IGeokomorkaHelper geokomorkaHelper;
        private readonly IMessenger messenger;
        private tblTowarGeokomorkaParametryRodzaj wybranyRodzaj;
        private tblTowarGeokomorkaParametryTyp wybranyTyp;
        private tblTowarGeokomorkaParametryZgrzew wybranyZgrzew;
        private tblTowar wybranyTowar;
        private tblZamowienieHandloweTowarGeokomorka geokomorka;
        #region Properties
        public tblZamowienieHandloweTowarGeokomorka Geokomorka
        {
            get => geokomorka;
            set
            {
                geokomorka = value;
                ObliczIloscM2iIloscSekcji();
            }
        }
        public IEnumerable<tblTowar> ListaTowarow { get; set; }
        public tblTowar WybranyTowar
        {
            get => wybranyTowar;
            set
            {
                wybranyTowar = value;
                RaisePropertyChanged();
                PobierzZgrzewZWybranegoTowaru();
                PobierzTypZWybranegoTowaru();
                PobierzRodzajZWybranegoTowaru();
                //GenerujPelneDane();
            }

        }
        public List<tblTowarGeokomorkaParametryRodzaj> ListaRodzajow { get; set; }
        public tblTowarGeokomorkaParametryRodzaj WybranyRodzaj
        {
            get => wybranyRodzaj;
            set
            {
                wybranyRodzaj = value;
                RaisePropertyChanged();
                GenerujPelneDane();
            }
        }
        public List<tblTowarGeokomorkaParametryTyp> ListaTypow { get; set; }
        public tblTowarGeokomorkaParametryTyp WybranyTyp
        {
            get => wybranyTyp;
            set
            {
                wybranyTyp = value;
                RaisePropertyChanged();
                GenerujPelneDane();
            }
        }
        public List<tblTowarGeokomorkaParametryZgrzew> ListaZgrzewow { get; set; }
        public tblTowarGeokomorkaParametryZgrzew WybranyZgrzew
        {
            get => wybranyZgrzew;
            set
            {
                wybranyZgrzew = value;
                RaisePropertyChanged();
                GenerujPelneDane();

            }
        }

        private decimal ilosc;

        public decimal Ilosc
        {
            get { return ilosc; }
            set
            {
                ilosc = value;
                Geokomorka.Ilosc_m2 = ilosc;
                RaisePropertyChanged();
                ObliczIloscM2iIloscSekcji();
                GenerujPelneDane();
            }
        }


        public List<tblTowarGeokomorkaParametryGeometryczne> ListaPrametrowGeometrycznychGeokomorki { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
        #endregion

        #region CTOR
        public TowarGeokomorkaDodajViewModel(IUnitOfWork unitOfWork,
                                                IUnitOfWorkFactory unitOfWorkFactory,
                                                IDialogService dialogService,
                                                IViewService viewService,
                                                IGeokomorkaHelper geokomorkaHelper,
                                                IMessenger messenger
                                            )
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.geokomorkaHelper = geokomorkaHelper;
            this.messenger = messenger;
            ZaladujPodczasUruchomieniaCommand = new RelayCommand(ZaladujPodczasUruchomieniaCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);

            messenger.Register<tblZamowienieHandloweTowarGeokomorka>(this, GdyPrzeslanoPozycjeGeokomorki);

            Geokomorka = new tblZamowienieHandloweTowarGeokomorka();
            ListaPrametrowGeometrycznychGeokomorki = new List<tblTowarGeokomorkaParametryGeometryczne>();
            ListaTowarow = new List<tblTowar>();
            ListaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>();
            ListaTypow = new List<tblTowarGeokomorkaParametryTyp>();
            WybranyTowar = new tblTowar();
        }

        private async void ZaladujPodczasUruchomieniaCommandExecute()
        {
            using (var uow = unitOfWorkFactory.Create())
            {
                ListaTowarow = await uow.tblTowar.GetAllAsync();
                ListaRodzajow = await uow.tblTowarGeokomorkaParametryRodzaj.GetAllAsync() as List<tblTowarGeokomorkaParametryRodzaj>;
                ListaTypow = await uow.tblTowarGeokomorkaParametryTyp.GetAllAsync() as List<tblTowarGeokomorkaParametryTyp>;
                ListaZgrzewow = await uow.tblTowarGeokomorkaParametryZgrzew.GetAllAsync() as List<tblTowarGeokomorkaParametryZgrzew>;
                ListaPrametrowGeometrycznychGeokomorki = await uow.tblTowarGeokomorkaParametryGeometryczne.GetAllAsync() as List<tblTowarGeokomorkaParametryGeometryczne>;
            }
        }
        #endregion

        private bool UsunCommandCanExecute()
        {
            if (Geokomorka.IDZamowienieHandloweTowarGeokomorka == 0)
                return false;

            return true;
        }

        private void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć bieżącą pozycję?"))
            {
                messenger.Send(Geokomorka, "Usun");
            }
        }

        private void ZapiszCommandExecute()
        {
            messenger.Send(Geokomorka, "Zapisz");
        }

        private bool ZapiszCommandCanExecute()
        {
            if (Geokomorka == null)
                return false;

            if (!Geokomorka.IsValid)
                return false;

            return true;
        }

        private async void GdyPrzeslanoPozycjeGeokomorki(tblZamowienieHandloweTowarGeokomorka obj)
        {
            Geokomorka = await unitOfWork.tblZamowienieHandloweTowarGeokomorka.GetByIdAsync(obj.IDTowarGeokomorkaParametryRodzaj);
        }

        private async Task PobierzIDTowaru()
        {
            var typ = ListaTypow.SingleOrDefault(t => t.IDTowarGeokomorkaParametryTyp == Geokomorka.IDTowarGeokomorkaParametryTyp);
            var rodzaj = ListaRodzajow.SingleOrDefault(r => r.IDTowarGeokomorkaParametryRodzaj == Geokomorka.IDTowarGeokomorkaParametryRodzaj);
            var zgrzew = ListaZgrzewow.SingleOrDefault(z => z.IDTowarGeokomorkaParametryZgrzew == Geokomorka.IDTowarGeokomorkaParametryZgrzew);

            using (var uow = unitOfWorkFactory.Create())
            {

                WybranyTowar = await uow.tblTowar.SingleOrDefaultAsync(t => t.Nazwa.Contains(typ.Typ) &&
                                                                            t.Nazwa.Contains(rodzaj.Rodzaj) &&
                                                                            t.Nazwa.Contains(zgrzew.KodZgrzewu) &&
                                                                            t.Nazwa.Contains(Geokomorka.Wysokosc_mm.ToString()));
            }
        }

        private void PobierzZgrzewZWybranegoTowaru()
        {
            if (WybranyTowar == null || WybranyTowar.IDTowar == 0)
                return;

            var zgrzew = geokomorkaHelper.PobierzZgrzewZNazwy(WybranyTowar.Nazwa);

            WybranyZgrzew = ListaZgrzewow.FirstOrDefault(z => z.KodZgrzewu == zgrzew.KodZgrzewu);
        }

        private void PobierzTypZWybranegoTowaru()
        {
            if (WybranyTowar == null || WybranyTowar.IDTowar == 0)
                return;

            var typ = geokomorkaHelper.PobierzTypZNazwy(WybranyTowar.Nazwa);

            WybranyTyp = ListaTypow.SingleOrDefault(s => s.IDTowarGeokomorkaParametryTyp == typ.IDTowarGeokomorkaParametryTyp);
        }
        private void PobierzRodzajZWybranegoTowaru()
        {
            if (WybranyTowar == null || WybranyTowar.IDTowar == 0)
                return;

            var rodzaj = geokomorkaHelper.PobierzRodzajZNazwy(WybranyTowar.Nazwa);

            WybranyRodzaj = ListaRodzajow.SingleOrDefault(s => s.IDTowarGeokomorkaParametryRodzaj == rodzaj.IDTowarGeokomorkaParametryRodzaj);
        }
        private void GenerujPelneDane()
        {
            Geokomorka.SzerokoscSekcji_mm = (int)(geokomorkaHelper.PobierzStandardowaSzerokoscSekcjiZNazwy_m(WybranyTowar.Nazwa) * 1000);
            Geokomorka.DlugoscSekcji_mm = (int)(geokomorkaHelper.PobierzStandardowaDlugoscSekcjiZNazwy_m(WybranyTowar.Nazwa) * 1000);
            Geokomorka.Wysokosc_mm = (int)geokomorkaHelper.PobierzWysokoscZNazwy(WybranyTowar.Nazwa);
            Geokomorka.NazwaPelna = geokomorkaHelper.GenerujNazwePelna(WybranyTowar.Nazwa, Geokomorka.Ilosc_m2.GetValueOrDefault());
            Geokomorka.IDTowarGeokomorkaParametryRodzaj = geokomorkaHelper.PobierzRodzajZNazwy(WybranyTowar.Nazwa).IDTowarGeokomorkaParametryRodzaj;
            Geokomorka.IDTowarGeokomorkaParametryTyp = geokomorkaHelper.PobierzTypZNazwy(WybranyTowar.Nazwa).IDTowarGeokomorkaParametryTyp;
            Geokomorka.IDTowarGeokomorkaParametryZgrzew = geokomorkaHelper.PobierzZgrzewZNazwy(WybranyTowar.Nazwa).IDTowarGeokomorkaParametryZgrzew;
            //SprawdzCzyIloscM2godnaZPowierzchniaSekcji();
            //GenerujNazwePelna();
        }

        private void ObliczIloscM2iIloscSekcji()
        {
            Geokomorka.IloscSekcji_szt = geokomorkaHelper.ObliczIloscSekcji(WybranyTowar?.Nazwa, Geokomorka.Ilosc_m2.GetValueOrDefault());
            Geokomorka.Ilosc_m2 = geokomorkaHelper.ObliczIloscM2ZgodnaZPowierzchniaSekcji(WybranyTowar?.Nazwa, Geokomorka.Ilosc_m2.GetValueOrDefault());

        }
    }
}
