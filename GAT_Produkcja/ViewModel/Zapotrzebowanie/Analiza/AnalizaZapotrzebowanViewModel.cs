using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Services;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Analiza
{
    public class AnalizaZapotrzebowanViewModel : ViewModelBase
    {
        #region Properties

        private IEnumerable<tblKlasyfikacjaOgolna> listaKlasyfikacjiOgolnych;
        private tblKlasyfikacjaOgolna wybranaKlasyfikacjaOgolna;
        private IEnumerable<tblKlasyfikacjaSzczegolowa> listaKlasyfikacjiSzczegolowych;
        private tblKlasyfikacjaSzczegolowa wybranaKlasyfikacjaSzczegolowa;
        private IEnumerable<tblUrzadzenia> listaUrzadzen;
        private tblUrzadzenia wybraneUrzadzenie;
        private IEnumerable<vwZapotrzebowanieEwidencja> listaZapotrzebowan;

        private decimal sumaKlasyfikacjaOgolna;
        private decimal sumaKlasyfikacjaSzczegolowa;
        private decimal sumaUrzadzenia;
        private decimal sumaZapotrzebowan;
        private string klasyfikacjaSzczegolowaTytul;
        private string urzadzeniaTytul;
        private string zapotrzebowanieTytul;

        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;

        public decimal SumaZapotrzebowan
        {
            get { return sumaZapotrzebowan; }
            set { sumaZapotrzebowan = value; RaisePropertyChanged(); }
        }

        public IEnumerable<vwZapotrzebowanieEwidencja> ListaZapotrzebowan
        {
            get { return listaZapotrzebowan; }
            set { listaZapotrzebowan = value; RaisePropertyChanged(); }
        }

        public string ZapotrzebowanieTytul
        {
            get { return zapotrzebowanieTytul; }
            set { zapotrzebowanieTytul = value; RaisePropertyChanged(); }
        }


        public string KlasyfikacjaSzczegolowaTytul
        {
            get { return klasyfikacjaSzczegolowaTytul; }
            set { klasyfikacjaSzczegolowaTytul = value; RaisePropertyChanged(); }
        }


        public string UrzadzeniaTytul
        {
            get { return urzadzeniaTytul; }
            set { urzadzeniaTytul = value; RaisePropertyChanged(); }
        }


        public decimal SumaUrzadzenia
        {
            get { return sumaUrzadzenia; }
            set { sumaUrzadzenia = value; RaisePropertyChanged(); }
        }


        public decimal SumaKlasyfikacjaSzczegolowa
        {
            get { return sumaKlasyfikacjaSzczegolowa; }
            set { sumaKlasyfikacjaSzczegolowa = value; RaisePropertyChanged(); }
        }

        public decimal SumaKlasyfikacjaOgolna
        {
            get { return sumaKlasyfikacjaOgolna; }
            set { sumaKlasyfikacjaOgolna = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblKlasyfikacjaOgolna> ListaKlasyfikacjiOgolnych
        {
            get { return listaKlasyfikacjiOgolnych; }
            set { listaKlasyfikacjiOgolnych = value; RaisePropertyChanged(); }
        }
        public tblKlasyfikacjaOgolna WybranaKlasyfikacjaOgolna
        {
            get { return wybranaKlasyfikacjaOgolna; }
            set { wybranaKlasyfikacjaOgolna = value;
                RaisePropertyChanged();
                WybranaKlasyfikacjaSzczegolowa = null;
                WybraneUrzadzenie = null;
                PobierzListeZapotrzebowan();
            }
        }

        public IEnumerable<tblKlasyfikacjaSzczegolowa> ListaKlasyfikacjiSzczegolowych
        {
            get { return listaKlasyfikacjiSzczegolowych; }
            set { listaKlasyfikacjiSzczegolowych = value;
                RaisePropertyChanged();
            }
        }

        public tblKlasyfikacjaSzczegolowa WybranaKlasyfikacjaSzczegolowa
        {
            get { return wybranaKlasyfikacjaSzczegolowa; }
            set { wybranaKlasyfikacjaSzczegolowa = value; RaisePropertyChanged();
                WybraneUrzadzenie = null;

                PobierzListeZapotrzebowan(); }
        }

        public IEnumerable<tblUrzadzenia> ListaUrzadzen
        {
            get { return listaUrzadzen; }
            set { listaUrzadzen = value; RaisePropertyChanged(); PobierzListeZapotrzebowan(); }
        }

        public tblUrzadzenia WybraneUrzadzenie
        {
            get { return wybraneUrzadzenie; }
            set { wybraneUrzadzenie = value; RaisePropertyChanged(); PobierzListeZapotrzebowan(); }
        }


        private IEnumerable<vwZapotrzebowanieEwidencja> zapotrzebowanieEwidencja;

        public RelayCommand PobierzWartosciPoczatkoweCommand { get; set; }
        public RelayCommand PokazKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejCommand { get; set; }
        public RelayCommand PokazUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowejCommand { get; set; }
        #endregion

        #region CTOR

        public AnalizaZapotrzebowanViewModel(IUnitOfWork unitOfWork,
                                            IDialogService dialogService)
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;

            PobierzWartosciPoczatkoweCommand = new RelayCommand(PobierzWartosciPoczatkoweCommandExecute);
            PokazKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejCommand = new RelayCommand(PokazKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejCommandExecute);
            PokazUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowejCommand = new RelayCommand(PokazUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowejCommandExecute);
        }

        private async void PobierzWartosciPoczatkoweCommandExecute()
        {
            var klasyfikacjeOgolne = await unitOfWork.tblKlasyfikacjaOgolna.GetAllAsync();
            var klasyfikacjeSzczegolowe = await unitOfWork.tblKlasyfikacjaSzczegolowa.GetAllAsync();

            var urzadzenia = await unitOfWork.tblUrzadzenia.GetAllAsync();
            zapotrzebowanieEwidencja = await unitOfWork.vwZapotrzebowanieEwidencja.WhereAsync(s => s.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Akceptacja);

            foreach (var klasyfikacja in klasyfikacjeOgolne)
            {
                klasyfikacja.SumaZapotrzebowania = zapotrzebowanieEwidencja.Where(s => s.IDKlasyfikacjaOgolna == klasyfikacja.IDKlasyfikacjaOgolna)
                                                                            .Where(s => s.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Akceptacja)
                                                                            .Sum(s => s.SumaOfKoszt)
                                                                            .GetValueOrDefault();
            }
            ListaKlasyfikacjiOgolnych = klasyfikacjeOgolne.OrderByDescending(s => s.SumaZapotrzebowania);
            SumaKlasyfikacjaOgolna = ListaKlasyfikacjiOgolnych.Sum(s => s.SumaZapotrzebowania);


            foreach (var klasyfikacja in klasyfikacjeSzczegolowe)
            {
                klasyfikacja.SumaZapotrzebowania = zapotrzebowanieEwidencja.Where(s => s.IDKlasyfikacjaSzczegolowa == klasyfikacja.IDKlasyfikacjaSzczegolowa)
                                                                            .Where(s => s.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Akceptacja)
                                                                            .Sum(s => s.SumaOfKoszt)
                                                                            .GetValueOrDefault();
            }
            ListaKlasyfikacjiSzczegolowych = klasyfikacjeSzczegolowe.OrderByDescending(s => s.SumaZapotrzebowania);

            KlasyfikacjaSzczegolowaTytul = "Klasyfikacja szczegółowa";
            UrzadzeniaTytul = "Urządzenia";
        }
        #endregion

        private async void PokazKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejCommandExecute()
        {
            var klasyfikacjeSzczegolowe = await unitOfWork.tblKlasyfikacjaSzczegolowa.PobierzKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejAsync(WybranaKlasyfikacjaOgolna.IDKlasyfikacjaOgolna);
            foreach (var klasyfikacja in klasyfikacjeSzczegolowe)
            {
                klasyfikacja.SumaZapotrzebowania = zapotrzebowanieEwidencja.Where(s => s.IDKlasyfikacjaSzczegolowa == klasyfikacja.IDKlasyfikacjaSzczegolowa)
                                                                            .Where(s => s.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Akceptacja)
                                                                            .Sum(s => s.SumaOfKoszt)
                                                                            .GetValueOrDefault();
            }
            ListaKlasyfikacjiSzczegolowych = klasyfikacjeSzczegolowe.OrderByDescending(s => s.SumaZapotrzebowania);
            KlasyfikacjaSzczegolowaTytul = "Klasyfikacja szczegółowa dla: " + WybranaKlasyfikacjaOgolna.Nazwa;
            SumaKlasyfikacjaSzczegolowa = ListaKlasyfikacjiSzczegolowych.Sum(s => s.SumaZapotrzebowania);
            ListaUrzadzen = null;
            UrzadzeniaTytul = "Urządzenia";

        }
        private async void PokazUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowejCommandExecute()
        {
            var urzadzenia = await unitOfWork.tblUrzadzenia.PobierzUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowej(WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa);
            foreach (var urzadzenie in urzadzenia)
            {
                urzadzenie.SumaZapotrzebowania = zapotrzebowanieEwidencja.Where(s => s.IDUrzadzenia == urzadzenie.IDUrzadzenia)
                                                                            .Where(s=>s.IDKlasyfikacjaSzczegolowa==WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa)
                                                                            .Where(s => s.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Akceptacja)
                                                                            .Sum(s => s.SumaOfKoszt)
                                                                            .GetValueOrDefault();
            }
            ListaUrzadzen = urzadzenia.OrderByDescending(s => s.SumaZapotrzebowania);
            SumaUrzadzenia = ListaUrzadzen.Sum(s => s.SumaZapotrzebowania);
            UrzadzeniaTytul = "Urządzenia dla produkcji: " + WybranaKlasyfikacjaSzczegolowa.Nazwa;

        }


        private void PobierzListeZapotrzebowan()
        {
            if (WybranaKlasyfikacjaOgolna==null)
            {
                return;
            }

            try
            {

                if (WybranaKlasyfikacjaOgolna != null &&
                    WybranaKlasyfikacjaSzczegolowa == null &&
                    WybraneUrzadzenie == null)
                {
                    ListaZapotrzebowan = zapotrzebowanieEwidencja.Where(z => z.IDKlasyfikacjaOgolna == WybranaKlasyfikacjaOgolna.IDKlasyfikacjaOgolna);
                }
                else if (WybranaKlasyfikacjaOgolna != null &&
                    WybranaKlasyfikacjaSzczegolowa != null &&
                    WybraneUrzadzenie == null)
                {
                    ListaZapotrzebowan = zapotrzebowanieEwidencja
                                                .Where(z => z.IDKlasyfikacjaOgolna == WybranaKlasyfikacjaOgolna.IDKlasyfikacjaOgolna)
                                                .Where(z => z.IDKlasyfikacjaSzczegolowa == WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa);
                }
                else
                {
                    ListaZapotrzebowan = zapotrzebowanieEwidencja
                                                .Where(z => z.IDKlasyfikacjaOgolna == WybranaKlasyfikacjaOgolna.IDKlasyfikacjaOgolna)
                                                .Where(z => z.IDKlasyfikacjaSzczegolowa == WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa)
                                                .Where(z => z.IDUrzadzenia == WybraneUrzadzenie.IDUrzadzenia);
                }

                ListaZapotrzebowan = ListaZapotrzebowan.OrderByDescending(s => s.SumaOfKoszt);
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK(ex.Message);
            }
            ZapotrzebowanieTytul = $"Zapotrzebowania dla \r" + 
                                    $"{WybranaKlasyfikacjaOgolna.Nazwa} - " +
                                    $"{(WybranaKlasyfikacjaSzczegolowa!=null? WybranaKlasyfikacjaSzczegolowa.Nazwa : String.Empty)} - " +
                                    $"{(WybraneUrzadzenie!=null? WybraneUrzadzenie.Nazwa : String.Empty)}";

            SumaZapotrzebowan = ListaZapotrzebowan.Sum(s => s.SumaOfKoszt).GetValueOrDefault();
            //WybranaKlasyfikacjaSzczegolowa = null;
            //WybraneUrzadzenie = null;
        }

    }
}
