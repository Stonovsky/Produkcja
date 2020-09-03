using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj;
using System.Linq;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.db.Enums;
using PropertyChanged;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.ViewModel.MainMenu.Messages;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Services;

namespace GAT_Produkcja.ViewModel.MainMenu.Zapotrzebowanie
{
    [AddINotifyPropertyChangedInterface]
    public class MainMenuZapotrzebowanieViewModel : ListMethodViewModelBase, IMainMenuZapotrzebowanieViewModel
    {
        private const string AdresMailowy_DyrektorProdukcji = "dyrektor.produkcji@gtex.pl";

        #region Properties
        private IEnumerable<vwZapotrzebowanieEwidencja> listaZapotrzebowan;
        private readonly IOutlookMailSender outlookMailSender;

        public RelayCommand PokazZapotrzebowanieCommand { get; set; }
        public RelayCommand DodajZapotrzebowanieCommand { get; set; }
        public RelayCommand StatusAkceptacjaCommand { get; set; }
        public RelayCommand StatusBrakAkceptacjiCommand { get; set; }
        public IEnumerable<vwZapotrzebowanieEwidencja> ListaZapotrzebowan
        {
            get { return listaZapotrzebowan; }
            set { listaZapotrzebowan = value; RaisePropertyChanged(); }
        }


        private vwZapotrzebowanieEwidencja wybraneZapotrzebowanie;

        public vwZapotrzebowanieEwidencja WybraneZapotrzebowanie
        {
            get { return wybraneZapotrzebowanie; }
            set { wybraneZapotrzebowanie = value; RaisePropertyChanged(); }
        }

        public string Tytul { get; set; }
        public bool IsButtonActive { get; set; } = true;
        #endregion


        #region CTOR
        public MainMenuZapotrzebowanieViewModel(IViewModelService viewModelService,
                                                IOutlookMailSender outlookMailSender)
            : base(viewModelService)
        {
            this.outlookMailSender = outlookMailSender;

            PokazZapotrzebowanieCommand = new RelayCommand(PokazZapotrzebowanieCommandExecute, PokazZapotrzebowanieCommandCanExecute);
            DodajZapotrzebowanieCommand = new RelayCommand(DodajZapotrzebowanieCommandExecute);
            StatusAkceptacjaCommand = new RelayCommand(StatusAkceptacjaCommandExecute, StatusAkceptacjaCommandCanExecute);
            StatusBrakAkceptacjiCommand = new RelayCommand(StatusBrakAkceptacjiCommandExecute, StatusBrakAkceptacjiCommandCanExecute);

            Messenger.Register<OdswiezMainMenuMessage>(this, Odswiez);
            Messenger.Register<tblZapotrzebowanie>(this, Odswiez);

            ListaZapotrzebowan = new List<vwZapotrzebowanieEwidencja>();
            Tytul = "Zapotrzebowania oczekujące i zweryfikowane";

        }
        public override async Task LoadAsync(int? id)
        {
            IsButtonActive = false;
            await PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync();
            IsButtonActive = true;
        }

        private bool PokazZapotrzebowanieCommandCanExecute()
        {
            return WybraneZapotrzebowanie != null;
        }



        #region Akceptacja/BrakAkceptacji
        private bool StatusBrakAkceptacjiCommandCanExecute()
        {
            if (WybraneZapotrzebowanie == null) return false;

            return WybraneZapotrzebowanie.IDZapotrzebowanieStatus != (int?)StatusZapotrzebowaniaEnum.BrakAkceptacji;

        }
        private async void StatusBrakAkceptacjiCommandExecute()
        {
            await ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum.BrakAkceptacji);
        }

        private bool StatusAkceptacjaCommandCanExecute()
        {
            if (WybraneZapotrzebowanie == null) return false;

            return WybraneZapotrzebowanie.IDZapotrzebowanieStatus != (int?)StatusZapotrzebowaniaEnum.Akceptacja;
        }

        private async void StatusAkceptacjaCommandExecute()
        {
            await ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum.Akceptacja);
        }

        private async Task ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum statusEnum)
        {
            var zapotrzebowanieZaakceptowane = await ZmienStatus(WybraneZapotrzebowanie, statusEnum);
            try
            {
                await WyslijMailaZOutlooka(zapotrzebowanieZaakceptowane);

                await UnitOfWork.SaveAsync();
                Messenger.Send(new OdswiezMainMenuMessage());
            }
            catch (Exception ex)
            {
                DialogService.ShowInfo_BtnOK(ex.Message);
            }
        }

        private async Task<tblZapotrzebowanie> ZmienStatus(vwZapotrzebowanieEwidencja zapotrzebowanie, StatusZapotrzebowaniaEnum statusEnum)
        {
            var zapotrzebowanieDoZmiany = await UnitOfWork.tblZapotrzebowanie.GetByIdAsync(zapotrzebowanie.IDZapotrzebowanie);

            if (zapotrzebowanieDoZmiany.IDZapotrzebowanieStatus == (int?)statusEnum)
                return zapotrzebowanieDoZmiany;

            zapotrzebowanieDoZmiany.IDZapotrzebowanieStatus = (int?)statusEnum;
            var status = await UnitOfWork.tblZapotrzebowanieStatus.SingleOrDefaultAsync(s => s.IDZapotrzebowanieStatus == (int?)statusEnum);
            zapotrzebowanieDoZmiany.tblZapotrzebowanieStatus = status;

            return zapotrzebowanieDoZmiany;
        }
        private async Task WyslijMailaZOutlooka(tblZapotrzebowanie zapotrzebowanie)
        {
            await outlookMailSender.WyslijMailZZapotrzebowaniemAsync(zapotrzebowanie, new List<string>
                                                                            {
                                                                                zapotrzebowanie.tblPracownikGAT.Email,
                                                                                zapotrzebowanie.PracownikOdpZaZakup.Email,
                                                                                AdresMailowy_DyrektorProdukcji
                                                                            }); ;
        }
        #endregion


        private async void Odswiez(tblZapotrzebowanie obj)
        {
            if (obj != null)
                await PobierzZapotrzebowaniaZweryfikowaneINieZaakceptowaneAsync();
        }
        private async void Odswiez(OdswiezMainMenuMessage obj)
        {
            await PobierzZapotrzebowaniaZweryfikowaneINieZaakceptowaneAsync();
        }

        private void DodajZapotrzebowanieCommandExecute()
        {
            ViewService.Show<ZapotrzebowanieDodajViewModel>();
        }

        private void PokazZapotrzebowanieCommandExecute()
        {
            if (WybraneZapotrzebowanie != null)
            {
                ViewService.Show<ZapotrzebowanieDodajViewModel>();
                Messenger.Send(WybraneZapotrzebowanie);
            }
        }
        #endregion

        private async Task PobierzZapotrzebowanieZbiezacegoMiesiaca()
        {
            try
            {
                ListaZapotrzebowan = await UnitOfWork.vwZapotrzebowanieEwidencja.PobierzZapotrzebowanieZbiezacegoMiesiacaAsync();
                ListaZapotrzebowan = ListaZapotrzebowan.OrderByDescending(l => l.Nr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task PobierzZapotrzebowaniaZweryfikowaneINieZaakceptowaneAsync()
        {
            try
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    ListaZapotrzebowan = await uow.vwZapotrzebowanieEwidencja.PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync();
                }
                ListaZapotrzebowan = ListaZapotrzebowan.OrderByDescending(l => l.Nr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync()
        {
            ListaZapotrzebowan = await UnitOfWork.vwZapotrzebowanieEwidencja.PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync();
            ListaZapotrzebowan = ListaZapotrzebowan.OrderByDescending(l => l.Nr);
        }

    }
}
