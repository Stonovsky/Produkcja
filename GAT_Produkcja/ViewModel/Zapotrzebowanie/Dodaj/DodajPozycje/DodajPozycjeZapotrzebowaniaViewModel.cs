using GalaSoft.MvvmLight;
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
using PropertyChanged;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj.DodajPozycje
{
    [AddINotifyPropertyChangedInterface]

    public class DodajPozycjeZapotrzebowaniaViewModel : ViewModelBase
    {
        #region Properties
        private tblZapotrzebowaniePozycje zapotrzebowaniePozycje;
        private tblZapotrzebowanie zapotrzebowanie;
        private string tytul;
        private IEnumerable<tblJm> listaJednostekMiar;

        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;

        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand AnulujCommand { get; set; }



        public IEnumerable<tblJm> ListaJednostekMiar
        {
            get { return listaJednostekMiar; }
            set { listaJednostekMiar = value; RaisePropertyChanged(); }
        }

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        public tblZapotrzebowaniePozycje ZapotrzebowaniePozycje
        {
            get { return zapotrzebowaniePozycje; }
            set
            {
                zapotrzebowaniePozycje = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CTOR
        public DodajPozycjeZapotrzebowaniaViewModel(IUnitOfWork unitOfWork, IViewService viewService, IDialogService dialogService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

            ZapotrzebowaniePozycje = new tblZapotrzebowaniePozycje();

            Task.Run(async () => ListaJednostekMiar = await unitOfWork.tblJm.GetAllAsync());

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            AnulujCommand = new RelayCommand(AnulujCommandExecute);

            messenger.Register<tblZapotrzebowanie>(this, GdyPrzeslanoZapotrzebowanie);
            messenger.Register<tblZapotrzebowaniePozycje>(this, GdyPrzeslanoZapotrzebowaniePozycje);

            Tytul = "Dodaj pozycję kosztową";
        }
        #endregion

        private async void GdyPrzeslanoZapotrzebowaniePozycje(tblZapotrzebowaniePozycje obj)
        {
            if (obj != null)
            {
                await PobierzZapotrzebowanie(obj);
                await PobierzPozycjeZapotrzebowania(obj);
            }
        }

        private async Task PobierzPozycjeZapotrzebowania(tblZapotrzebowaniePozycje obj)
        {
            if (obj.IDZapotrzebowaniePozycja != 0)
                ZapotrzebowaniePozycje = await unitOfWork.tblZapotrzebowaniePozycje.GetByIdAsync(obj.IDZapotrzebowaniePozycja);
            else
                ZapotrzebowaniePozycje = obj;
        }

        private async Task PobierzZapotrzebowanie(tblZapotrzebowaniePozycje obj)
        {
            if (obj.IDZapotrzebowanie != null && obj.IDZapotrzebowanie != 0)
            {
                zapotrzebowanie = await unitOfWork.tblZapotrzebowanie.GetByIdAsync(obj.IDZapotrzebowanie.Value);
                Tytul = "Pozycja kosztowa dla zapotrzebowania: " + zapotrzebowanie?.Opis;
            }
        }

        private void AnulujCommandExecute()
        {
            unitOfWork.Dispose();
            viewService.Close<DodajPozycjeZapotrzebowaniaViewModel>();
        }

        #region UsunCommand
        private bool UsunCommandCanExecute()
        {
            if (ZapotrzebowaniePozycje.IDZapotrzebowaniePozycja != 0)
            {
                return true;
            }
            return false;
        }

        private void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć pozycję zapotrzebowania?"))
            {
                //unitOfWork.tblZapotrzebowaniePozycje.Remove(ZapotrzebowaniePozycje);
                //await unitOfWork.SaveAsync();
                messenger.Send(ZapotrzebowaniePozycje, "Usun");
            }
        }

        #endregion

        private bool ZapiszCommandCanExecute()
        {
            PrzeliczKoszt();
            if (ZapotrzebowaniePozycje.IsValid)
            {
                return true;
            }
            return false;
        }

        private void PrzeliczKoszt()
        {
            if (ZapotrzebowaniePozycje.Ilosc > 0 && ZapotrzebowaniePozycje.Cena > 0)
            {
                ZapotrzebowaniePozycje.Koszt = (decimal)ZapotrzebowaniePozycje.Ilosc * ZapotrzebowaniePozycje.Cena;
                RaisePropertyChanged(nameof(ZapotrzebowaniePozycje));
            }
        }

        private void ZapiszCommandExecute()
        {
            if (ZapotrzebowaniePozycje.IDZapotrzebowaniePozycja == 0)
            {
                if (zapotrzebowaniePozycje.IDJm != null)
                    zapotrzebowaniePozycje.Jm = ListaJednostekMiar
                                                    .SingleOrDefault(j => j.IDJm == zapotrzebowaniePozycje.IDJm).Jm;

                messenger.Send(ZapotrzebowaniePozycje, "Dodaj");
            }
            else
            {
                messenger.Send(ZapotrzebowaniePozycje, "Edytuj");
            }
            viewService.Close<DodajPozycjeZapotrzebowaniaViewModel>();
        }

        private void GdyPrzeslanoZapotrzebowanie(tblZapotrzebowanie obj)
        {
            if (obj != null)
            {
                zapotrzebowanie = obj;
            }
        }


    }
}
