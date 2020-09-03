using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Ewidencja
{
    public class ZapotrzebowanieEwidencjaViewModel : ViewModelBase
    {
        private const string AdresMailowy_DyrektorProdukcji = "dyrektor.produkcji@gtex.pl";
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IViewService viewService;
        private readonly IOutlookMailSender outlookMailSender;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        private ObservableCollection<vwZapotrzebowanieEwidencja> listaZapotrzebowan;
        private decimal sumaZapotrzebowan;
        private decimal sumaZapotrzebowanDlaBiezacegoMiesiaca;
        private decimal sumaZapotrzebowanZBiezacegoRoku;
        private decimal sumaWszystkichZapotrzebowanZaakceptowanych;
        private decimal sumaZapotrzebowanZaakceptowanychWBiezacymRoku;
        private decimal sumaZapotrzebowanZaakceptowanychWBiezacymMiesiacu;


        public RelayCommand PokazSzczegolyZapotrzebowaniaCommand { get; set; }
        public RelayCommand DodajZapotrzebowanieCommand { get; set; }
        public RelayCommand PobierzWartosciPoczatkoweCommand { get; set; }

        public RelayCommand StatusAkceptacjaCommand { get; set; }
        public RelayCommand StatusBrakAkceptacjiCommand { get; set; }

        public decimal SumaZapotrzebowanDlaBiezacegoMiesiaca
        {
            get { return sumaZapotrzebowanDlaBiezacegoMiesiaca; }
            set { sumaZapotrzebowanDlaBiezacegoMiesiaca = value; RaisePropertyChanged(); }
        }

        private vwZapotrzebowanieEwidencja wybraneZapotrzebowanie;

        public vwZapotrzebowanieEwidencja WybraneZapotrzebowanie
        {
            get { return wybraneZapotrzebowanie; }
            set { wybraneZapotrzebowanie = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<vwZapotrzebowanieEwidencja> ListaZapotrzebowan
        {
            get { return listaZapotrzebowan; }
            set { listaZapotrzebowan = value; RaisePropertyChanged(); }
        }


        public decimal SumaZapotrzebowan
        {
            get { return sumaZapotrzebowan; }
            set { sumaZapotrzebowan = value; RaisePropertyChanged(); }
        }


        public decimal SumaZapotrzebowanZBiezacegoRoku
        {
            get { return sumaZapotrzebowanZBiezacegoRoku; }
            set { sumaZapotrzebowanZBiezacegoRoku = value; RaisePropertyChanged(); }
        }

        public decimal SumaWszystkichZapotrzebowanZaakceptowanych
        {
            get { return sumaWszystkichZapotrzebowanZaakceptowanych; }
            set { sumaWszystkichZapotrzebowanZaakceptowanych = value; RaisePropertyChanged(); }
        }


        public decimal SumaZapotrzebowanZaakceptowanychWBiezacymRoku
        {
            get { return sumaZapotrzebowanZaakceptowanychWBiezacymRoku; }
            set { sumaZapotrzebowanZaakceptowanychWBiezacymRoku = value; RaisePropertyChanged(); }
        }


        public decimal SumaZapotrzebowanZaakceptowanychWBiezacymMiesiacu
        {
            get { return sumaZapotrzebowanZaakceptowanychWBiezacymMiesiacu; }
            set { sumaZapotrzebowanZaakceptowanychWBiezacymMiesiacu = value; RaisePropertyChanged(); }
        }



        #endregion

        #region CTOR
        public ZapotrzebowanieEwidencjaViewModel(IUnitOfWork unitOfWork,
                                                IUnitOfWorkFactory unitOfWorkFactory,
                                                IViewService viewService,
                                                IOutlookMailSender outlookMailSender,
                                                IDialogService dialogService,
                                                IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.viewService = viewService;
            this.outlookMailSender = outlookMailSender;
            this.dialogService = dialogService;
            this.messenger = messenger;

            PokazSzczegolyZapotrzebowaniaCommand = new RelayCommand(PokazSzczegolyZapotrzebowaniaCommandExecute);
            DodajZapotrzebowanieCommand = new RelayCommand(DodajZapotrzebowanieCommandExecute);
            PobierzWartosciPoczatkoweCommand = new RelayCommand(PobierzWartosciPoczatkoweCommandExecute);

            StatusAkceptacjaCommand = new RelayCommand(StatusAkceptacjaCommandExecute, StatusAkceptacjaCommandCanExecute);
            StatusBrakAkceptacjiCommand = new RelayCommand(StatusBrakAkceptacjiCommandExecute, StatusBrakAkceptacjiCommandCanExecute);

            messenger.Register<tblZapotrzebowanie>(this, Odswiez);
        }

        #region ZmianaStatusuZapotrzebowania
        private bool StatusBrakAkceptacjiCommandCanExecute()
        {
            return WybraneZapotrzebowanie.IDZapotrzebowanieStatus != (int)StatusZapotrzebowaniaEnum.BrakAkceptacji;
        }
        private async void StatusBrakAkceptacjiCommandExecute()
        {
            await ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum.BrakAkceptacji);
        }

        private bool StatusAkceptacjaCommandCanExecute()
        {
            return WybraneZapotrzebowanie.IDZapotrzebowanieStatus != (int)StatusZapotrzebowaniaEnum.Akceptacja;
        }

        private async void StatusAkceptacjaCommandExecute()
        {
            await ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum.Akceptacja);
        }


        private async Task ZmienStatusZapotrzebowania(StatusZapotrzebowaniaEnum statusZapotrzebowania)
        {
            try
            {
                var zapotrzebowanieDoZmiany = await unitOfWork.tblZapotrzebowanie.GetByIdAsync(WybraneZapotrzebowanie.IDZapotrzebowanie);

                zapotrzebowanieDoZmiany.IDZapotrzebowanieStatus = (int?)statusZapotrzebowania;
                //zapotrzebowanieDoZmiany.tblZapotrzebowanieStatus = await unitOfWork.tblZapotrzebowanieStatus.GetByIdAsync((int)statusZapotrzebowania);

                await WyslijMailaZOutlooka(zapotrzebowanieDoZmiany);

                await unitOfWork.SaveAsync();

                dialogService.ShowInfo_BtnOK($"Wiadomość: {zapotrzebowanieDoZmiany.tblZapotrzebowanieStatus?.StatusZapotrzebowania} \nzostała wysłana do adresata zapotrzebowania: {zapotrzebowanieDoZmiany.tblPracownikGAT.Email}");
                messenger.Send(zapotrzebowanieDoZmiany);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private async Task WyslijMailaZOutlooka(tblZapotrzebowanie zapotrzebowanie)
        {
            await outlookMailSender.WyslijMailZZapotrzebowaniemAsync(zapotrzebowanie,
                                                                     new List<string>
                                                                    {
                                                                        zapotrzebowanie.tblPracownikGAT.Email,
                                                                        zapotrzebowanie.PracownikOdpZaZakup.Email,
                                                                        AdresMailowy_DyrektorProdukcji
                                                                    }); ;
        }

        private async void Odswiez(tblZapotrzebowanie obj)
        {
            await PobierzWartosciPoczatkowe();
        }

        private async void PobierzWartosciPoczatkoweCommandExecute()
        {
            await PobierzWartosciPoczatkowe();
        }

        private async Task PobierzWartosciPoczatkowe()
        {
            using (var uow = unitOfWorkFactory.Create())
            {
                ListaZapotrzebowan = new ObservableCollection<vwZapotrzebowanieEwidencja>(await uow.vwZapotrzebowanieEwidencja.GetAllAsync());
            }

            GenerujPodsumowaniaZListyZapotrzebowan();
        }

        private void DodajZapotrzebowanieCommandExecute()
        {
            viewService.Show<ZapotrzebowanieDodajViewModel>();
        }

        private void PokazSzczegolyZapotrzebowaniaCommandExecute()
        {
            viewService.Show<ZapotrzebowanieDodajViewModel>();
            messenger.Send(WybraneZapotrzebowanie);
        }

        #endregion

        #region Methods

        private void GenerujPodsumowaniaZListyZapotrzebowan()
        {
            SumaZapotrzebowan = ListaZapotrzebowan.Sum(s => s.SumaOfKoszt).GetValueOrDefault();
            SumaZapotrzebowanDlaBiezacegoMiesiaca = ListaZapotrzebowan.Where(d => d.DataZgloszenia.Value.Month == DateTime.Now.Month)
                                                                        .Where(d => d.DataZgloszenia.Value.Year == DateTime.Now.Year)
                                                                        .Sum(s => s.SumaOfKoszt)
                                                                        .GetValueOrDefault();
            SumaZapotrzebowanZBiezacegoRoku = ListaZapotrzebowan.Where(d => d.DataZgloszenia.Value.Year == DateTime.Now.Year)
                                                                .Sum(s => s.SumaOfKoszt)
                                                                .GetValueOrDefault();
            SumaWszystkichZapotrzebowanZaakceptowanych = ListaZapotrzebowan.Where(a => a.StatusZapotrzebowania == "Akceptacja").Sum(s => s.SumaOfKoszt).GetValueOrDefault();

            SumaZapotrzebowanZaakceptowanychWBiezacymRoku = ListaZapotrzebowan.Where(d => d.DataZgloszenia.Value.Year == DateTime.Now.Year)
                                                                                .Where(a => a.StatusZapotrzebowania == "Akceptacja")
                                                                                .Sum(s => s.SumaOfKoszt)
                                                                                .GetValueOrDefault();
            SumaZapotrzebowanZaakceptowanychWBiezacymMiesiacu = ListaZapotrzebowan
                .Where(d => d.DataZgloszenia.Value.Month == DateTime.Now.Month)
                .Where(d => d.DataZgloszenia.Value.Year == DateTime.Now.Year)
                .Where(a => a.StatusZapotrzebowania == "Akceptacja")
                .Sum(s => s.SumaOfKoszt)
                .GetValueOrDefault();
        }
        #endregion
    }
}
