using GalaSoft.MvvmLight;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeowloknina
{
    [AddINotifyPropertyChangedInterface]

    public class TowarGeowlokninaViewModel : ViewModelBase, ITowarGeowlokninaViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        private tblZamowienieHandlowe zamowienieHandlowe;
        #region Properties

        public ObservableCollection<tblZamowienieHandloweTowarGeowloknina> ListaPozycjiGeowloknin { get; set; }
        public tblZamowienieHandloweTowarGeowloknina WybranaPozycjaGeowlokniny { get; set; }
        public IEnumerable<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; }
        public tblTowarGeowlokninaParametryGramatura WybranaGramatura { get; set; }
        public IEnumerable<tblTowarGeowlokninaParametryRodzaj> ListaRodzajowTowarow { get; set; }
        public tblTowarGeowlokninaParametryRodzaj WybranyRodzajTowaru { get; set; }


        public RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiCommand { get; set; }
        public RelayCommand OnAddingNewRowCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public bool IsValid { get; private set; }
        #endregion

        #region CTOR
        public TowarGeowlokninaViewModel(IUnitOfWork unitOfWork, IDialogService dialogService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.messenger = messenger;
            ZaladujPodczasUruchomieniaCommand = new RelayCommand(ZaladujPodczasUruchomieniaCommandExecute);
            PoEdycjiKomorkiCommand = new RelayCommand(PoEdycjiKomorkiCommandExecute);
            OnAddingNewRowCommand = new RelayCommand(OnAddingNewRowCommandExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);

            ListaPozycjiGeowloknin = new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>();
            WybranaPozycjaGeowlokniny = new tblZamowienieHandloweTowarGeowloknina();

            messenger.Register<tblZamowienieHandlowe>(this, "PrzeslaneZamowienie", GdyPrzeslanoZamowinieHandlowe);
            messenger.Register<tblZamowienieHandlowe>(this, "ZapiszTowar", GdyPrzeslanoZapisz);

            IsValid = true;
        }

        private bool UsunCommandCanExecute()
        {
            if (WybranaPozycjaGeowlokniny == null)
            {
                return false;
            }

            return true;
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć zaznaczoną pozycję?") == false)
                return;

            if (WybranaPozycjaGeowlokniny.IDZamowienieHandloweTowarGeowloknina != 0)
            {
                unitOfWork.tblZamowienieHandloweTowarGeowloknina.Remove(WybranaPozycjaGeowlokniny);
                await unitOfWork.SaveAsync();
            }
            ListaPozycjiGeowloknin.Remove(WybranaPozycjaGeowlokniny);
        }


        private async void GdyPrzeslanoZapisz(tblZamowienieHandlowe obj)
        {
            zamowienieHandlowe = obj;

            if (ListaPozycjiGeowloknin.Count() == 0)
                return;

            foreach (var towar in ListaPozycjiGeowloknin)
            {
                towar.IDZamowienieHandlowe = zamowienieHandlowe.IDZamowienieHandlowe;

                if (towar.IDZamowienieHandloweTowarGeowloknina == 0)
                {
                    unitOfWork.tblZamowienieHandloweTowarGeowloknina.Add(towar);
                }
            }
            await unitOfWork.SaveAsync();
        }

        private void OnAddingNewRowCommandExecute()
        {
        }

        private void PoEdycjiKomorkiCommandExecute()
        {
            WybranaPozycjaGeowlokniny.IloscSumaryczna = WybranaPozycjaGeowlokniny.SzerokoscRolki * WybranaPozycjaGeowlokniny.DlugoscNawoju * WybranaPozycjaGeowlokniny.IloscRolek;
            ValidujModel();

            if (WybranaPozycjaGeowlokniny.IsValid)
                GenerujNazwePelna();
        }

        private void GenerujNazwePelna()
        {
            WybranaPozycjaGeowlokniny.NazwaPelna = String.Format($"{0} {1} ({2}x{3}m), Ilość rolek: {4}szt., Ilość: {5}",
                                                                    WybranaPozycjaGeowlokniny.tblTowarGeowlokninaParametryRodzaj.Rodzaj,
                                                                    WybranaPozycjaGeowlokniny.tblTowarGeowlokninaParametryGramatura.Gramatura,
                                                                    WybranaPozycjaGeowlokniny.SzerokoscRolki,
                                                                    WybranaPozycjaGeowlokniny.DlugoscNawoju,
                                                                    WybranaPozycjaGeowlokniny.IloscRolek,
                                                                    WybranaPozycjaGeowlokniny.IloscSumaryczna
                                                                    );
        }

        private void ValidujModel()
        {
            if (ListaPozycjiGeowloknin.Count() == 0)
            {
                IsValid = true;
                return;
            }

            foreach (var towar in ListaPozycjiGeowloknin)
            {
                if (!towar.IsValid)
                {
                    IsValid = false;
                }
            }
            IsValid = true;
        }

        private async void ZaladujPodczasUruchomieniaCommandExecute()
        {
            ListaGramatur = await unitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync().ConfigureAwait(false);
            ListaRodzajowTowarow = await unitOfWork.tblTowarGeowlokninaParametryRodzaj.GetAllAsync().ConfigureAwait(false);
        }

        private async void GdyPrzeslanoZamowinieHandlowe(tblZamowienieHandlowe obj)
        {
            zamowienieHandlowe = obj;

            ListaPozycjiGeowloknin = new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>(
                                            await unitOfWork.tblZamowienieHandloweTowarGeowloknina
                                                    .WhereAsync(t => t.IDZamowienieHandlowe == zamowienieHandlowe.IDZamowienieHandlowe));
        }
        #endregion
    }

}
