using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Messages;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka
{
    [AddINotifyPropertyChangedInterface]
    public class ZlecenieProdukcyjneMieszankaViewModel : SaveDeleteMessangerViewModelBase, IZlecenieProdukcyjneMieszankaViewModel
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private bool isValid;
        private bool isChanged;
        private decimal sumarycznaIloscMieszanki;
        private tblProdukcjaZlecenieProdukcyjne_Mieszanka wybranaPozycjaMieszanki;
        private MieszankaKalkulatorCenyHelper kalkulator;
        private decimal udzialSumaryczny;
        #endregion

        #region Properties
        public tblProdukcjaZlecenie ZlcecenieProdukcyjne { get; set; } = new tblProdukcjaZlecenie();
        public ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka> ListaPozycjiMieszanki { get; set; }
        public tblProdukcjaZlecenieProdukcyjne_Mieszanka WybranaPozycjaMieszanki { get => wybranaPozycjaMieszanki; set { wybranaPozycjaMieszanki = value; ObliczUdzialyProcentowe(); } }
        public decimal SumarycznaIloscMieszanki
        {
            get => sumarycznaIloscMieszanki;
            set
            {
                sumarycznaIloscMieszanki = value;
                ObliczUdzialyProcentowe();
                GenerujPodsumowanieMieszanki();
                //messenger.Send(new ZmianaIlosciMieszankiMessage { Ilosc = SumarycznaIloscMieszanki });
            }
        }


        public decimal WartoscMieszanki { get; set; }
        public decimal KosztMieszanki_kg { get; set; }


        public override bool IsValid
        {
            get { return isValid = IsListValid(); }
            set => isValid = value;
        }
        public override bool IsChanged
        {
            get
            {
                //var test = !ListaPozycjiMieszanki.JsonCompare(ListaPozycjiMieszankiStartowa);
                return isChanged = !ListaPozycjiMieszanki.CompareWithList(ListaPozycjiMieszankiStartowa);
            }
            set => isChanged = value;
        }


        public string Uwagi { get; set; }
        #endregion

        #region Commands
        public RelayCommand OtworzEwidencjeTowarowCommand { get; set; }
        public RelayCommand DodajPozycjeMieszankiCommand { get; set; }
        public RelayCommand ZmienPozycjeMieszankiCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiDataGridCommand { get; set; }
        public RelayCommand UsunPozycjeMieszankiCommand { get; set; }
        public IList<tblProdukcjaZlecenieProdukcyjne_Mieszanka> ListaPozycjiMieszankiStartowa { get; set; }
        #endregion


        #region CTOR
        public ZlecenieProdukcyjneMieszankaViewModel(IUnitOfWork unitOfWork,
                                                     IViewService viewService,
                                                     IDialogService dialogService,
                                                     IMessenger messenger)
            : base(messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;

            OtworzEwidencjeTowarowCommand = new RelayCommand(OtworzEwidencjeTowarowCommandExecute);
            DodajPozycjeMieszankiCommand = new RelayCommand(DodajPozycjeMieszankiCommandExecute);
            ZmienPozycjeMieszankiCommand = new RelayCommand(ZmienPozycjeMieszankiCommandExecute);
            PoEdycjiKomorkiDataGridCommand = new RelayCommand(PoEdycjiKomorkiDataGridCommandExecute);
            UsunPozycjeMieszankiCommand = new RelayCommand(UsunPozycjeMieszankiCommandExecute, UsunPozycjeMieszankiCommandCanExecute);

            messenger.Register<ZmianaIlosciMieszankiMessage>(this, GdyZmienionoIlosc);
            messenger.Register<vwMagazynRuchGTX>(this, GdyWyslanoSurowiec);
            messenger.Register<NrZleceniaMessage>(this, GdyPrzeslanoNrZlecenia);


            ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>();

            kalkulator = new MieszankaKalkulatorCenyHelper();

            StworzKopieListyPozycjiMieszanki();
        }
        #endregion

        private async void GdyPrzeslanoNrZlecenia(NrZleceniaMessage obj)
        {
            await LoadAsync(obj.NrZlecenia);
            StworzKopieListyPozycjiMieszanki();
        }

        private void StworzKopieListyPozycjiMieszanki()
        {
            ListaPozycjiMieszankiStartowa = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>();
            foreach (var item in ListaPozycjiMieszanki)
            {
                var pozycja = item.DeepClone();
                ListaPozycjiMieszankiStartowa.Add(pozycja);
            }
        }

        #region Messengery
        private void GdyWyslanoSurowiec(vwMagazynRuchGTX obj)
        {
            viewService.Close<MagazynStanTowaruViewModel>();

            if (obj == null)
                throw new ArgumentNullException("Błąd: Przesłany surowiec jest pusty");

            if (CzyTowarIstniejeNaLiscieMieszanki(obj.IdTowar))
            {
                dialogService.ShowError_BtnOK("Towar już istnieje na liście mieszanki więc nie zostanie dodany.\n" +
                                              "Wybierz inny towar.");
                return;
            }

            WybranaPozycjaMieszanki = new tblProdukcjaZlecenieProdukcyjne_Mieszanka
            {
                IDTowar = obj.IdTowar,
                NazwaTowaru = obj.TowarNazwa,
                IDJm = (int)JmEnum.kg,
                JmNazwa = "kg",
                Cena_kg = obj.Cena
            };

            ObliczUdzialyProcentowe();
            ObliczCenyDlaPozycji();

            ListaPozycjiMieszanki.Add(WybranaPozycjaMieszanki);

        }

        private void ObliczUdzialyProcentowe()
        {
            if (!ListaPozycjiMieszanki.Any()) return;

            SumarycznaIloscMieszanki = ListaPozycjiMieszanki.Sum(s => s.IloscKg);
            
            if (SumarycznaIloscMieszanki == 0) return;

            ListaPozycjiMieszanki.ToList().ForEach
            (
                e => 
                { 
                    e.ZawartoscProcentowa = e.IloscKg/ SumarycznaIloscMieszanki;
                    e.IloscMieszanki_kg = SumarycznaIloscMieszanki;
                });
        }


        private bool CzyTowarIstniejeNaLiscieMieszanki(int idTowar)
        {
            return ListaPozycjiMieszanki.Any(c => c.IDTowar == idTowar);
        }

        private void GdyZmienionoIlosc(ZmianaIlosciMieszankiMessage obj)
        {
            SumarycznaIloscMieszanki = obj.Ilosc;

            foreach (var pozycja in ListaPozycjiMieszanki)
            {
                pozycja.IloscKg = pozycja.ZawartoscProcentowa * obj.Ilosc;
            }
        }

        private bool IsListValid()
        {
            if(ListaPozycjiMieszanki is null)
            {
                Uwagi = "Brak pozycji mieszanki";
                return false;
            }

            if (ListaPozycjiMieszanki.Count() == 0)
            {
                Uwagi = "Brak pozycji mieszanki";
                return false;
            }

            foreach (var pozycja in ListaPozycjiMieszanki)
            {
                if (!pozycja.IsValid)
                    return false;
            }
            if (ListaPozycjiMieszanki.Sum(s => s.ZawartoscProcentowa) != 1)
            {
                Uwagi = "Sumaryczna zawartość procentowa jest różna od 1";
                return false;
            }

            return true;
        }



        #region Commands
        private void OtworzEwidencjeTowarowCommandExecute()
        {
            viewService.Show<TowarEwidencjaViewModel>();
        }

        public async override Task LoadAsync(int? idZleceniaProdukcyjnego)
        {
            if (idZleceniaProdukcyjnego.HasValue)
            {
                //ListaPozycjiMieszanki = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == idZleceniaProdukcyjnego) as ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>;
                ListaPozycjiMieszanki = new ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>(
                    await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == idZleceniaProdukcyjnego)
                    );
                PobierzSumarycznaIloscMieszanki();
                GenerujPodsumowanieMieszanki();
                await PrzypiszPolaNieMapowane(ListaPozycjiMieszanki);
            }
            else
            {
                unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.AddRange(ListaPozycjiMieszanki);
            }
        }

        private void PobierzSumarycznaIloscMieszanki()
        {
            SumarycznaIloscMieszanki = ListaPozycjiMieszanki.Sum(s => s.IloscKg);
        }

        private async Task PrzypiszPolaNieMapowane(ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)
        {
            var towary = await unitOfWork.vwTowarGTX.GetAllAsync();
            if (towary is null || towary.Count()==0) return;

            foreach (var pozycja in listaPozycjiMieszanki)
            {
                pozycja.NazwaTowaru = towary.SingleOrDefault(s => s.IdTowar == pozycja.IDTowar).Nazwa;
                pozycja.JmNazwa = pozycja.tblJm.Jm;
            }
        }

        private async void UsunPozycjeMieszankiCommandExecute()
        {

            if (WybranaPozycjaMieszanki.IDProdukcjaZlecenieProdukcyjne == null)
            {
                ListaPozycjiMieszanki.Remove(WybranaPozycjaMieszanki);
            }
            else
            {
                var mieszanka = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.GetByIdAsync(WybranaPozycjaMieszanki.IDZlecenieProdukcyjneMieszanka);
                unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.Remove(mieszanka);
                await unitOfWork.SaveAsync();

                ListaPozycjiMieszanki.Remove(wybranaPozycjaMieszanki);
            }
        }

        private bool UsunPozycjeMieszankiCommandCanExecute()
        {
            return WybranaPozycjaMieszanki != null;
        }
        #endregion

        private void PoEdycjiKomorkiDataGridCommandExecute()
        {
            ObliczUdzialyProcentowe();
            ObliczCenyDlaPozycji();
            GenerujPodsumowanieMieszanki();
        }

        private void ZmienPozycjeMieszankiCommandExecute()
        {
            viewService.Show<MagazynStanTowaruViewModel>();
        }

        private void DodajPozycjeMieszankiCommandExecute()
        {
            WybranaPozycjaMieszanki = new tblProdukcjaZlecenieProdukcyjne_Mieszanka();
            viewService.Show<MagazynStanTowaruViewModel>();
            messenger.Send(new vwMagazynGTX { IdMagazyn = (int)MagazynyGTXEnum.Surowce_Geowloknina_SWL });
        }

        public async Task SaveAsync(int? idZleceniaProdukcyjnego)
        {

            if (idZleceniaProdukcyjnego != null
                && idZleceniaProdukcyjnego != 0)
            {
                foreach (var pozycja in ListaPozycjiMieszanki)
                {
                    pozycja.IDProdukcjaZlecenieProdukcyjne = idZleceniaProdukcyjnego;

                    if (pozycja.IDZlecenieProdukcyjneMieszanka == 0)
                        unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.Add(pozycja);
                }

                await unitOfWork.SaveAsync();
                IsChanged_False();
            }
        }

        private void ObliczCenyDlaPozycji()
        {
            try
            {
                ListaPozycjiMieszanki = kalkulator.ObliczWartoscPozycji(ListaPozycjiMieszanki) as ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>;
                ListaPozycjiMieszanki = kalkulator.DodajWartoscMieszankiDoPozycjiListy(ListaPozycjiMieszanki) as ObservableCollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka>;
            }
            catch (Exception ex)
            {
                dialogService.ShowError_BtnOK(ex.Message);
            }
        }

        private void GenerujPodsumowanieMieszanki()
        {
            try
            {
                ZlcecenieProdukcyjne.WartoscMieszanki_zl = kalkulator.ObliczWartoscMieszanki(ListaPozycjiMieszanki);
                ZlcecenieProdukcyjne.CenaMieszanki_zl = kalkulator.ObliczSredniaCeneMieszankiZaKg(ListaPozycjiMieszanki);
                ZlcecenieProdukcyjne.UdzialSurowcowWMieszance = ListaPozycjiMieszanki.Sum(s => s.ZawartoscProcentowa);

                messenger.Send<tblProdukcjaZlecenie, ZlecenieProdukcyjneNaglowekViewModel_old>(ZlcecenieProdukcyjne);
            }
            catch (Exception ex)
            {
                dialogService.ShowError_BtnOK(ex.Message);
            }
        }


        public async Task DeleteAsync(int idZleceniaProdukcyjnego)
        {
            if (idZleceniaProdukcyjnego != 0)
            {
                unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(ListaPozycjiMieszanki);
                await unitOfWork.SaveAsync();
            }
        }

        #endregion

        #region IsChange
        public void IsChanged_False()
        {
            ListaPozycjiMieszankiStartowa = ListaPozycjiMieszanki.DeepClone();
        }

        #endregion

    }
}
