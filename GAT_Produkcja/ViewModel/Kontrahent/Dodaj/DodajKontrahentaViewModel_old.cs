using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Utilities.WebScraper;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.db.Helpers.Kontrahent;
using PropertyChanged;
using NLog;

namespace GAT_Produkcja.ViewModel.Kontrahent.Dodaj
{
    [AddINotifyPropertyChangedInterface]

    public class DodajKontrahentaViewModel_old : ViewModelBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Properties
        private tblKontrahent kontrahent;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IPobierzDaneKontrahentaZGUS pobierzDaneKontrahentaZGUS;
        private readonly IMessenger messenger;
        private readonly KontrahentNipValidationHelper kontrahentNipValidationHelper;
        private tblKontrahent kontrahentPrzeslany;
        private bool pobierzDaneZGUSSButtonActive;
        public bool PobierzDaneZGusButtonActivate { get; set; }
        public tblKontrahent Kontrahent
        {
            get { return kontrahent; }
            set { kontrahent = value; RaisePropertyChanged(); }
        }

        private string tytul;

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }



        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        #endregion
        public RelayCommand PobierzDaneZGUSCommand { get; set; }
        public bool CzyWbazieIstniejeKontrahentOPodanymNrNIP { get; set; }
        #region CTOR
        public DodajKontrahentaViewModel_old(IUnitOfWork unitOfWork,
                                        IDialogService dialogService,
                                        IViewService viewService,
                                        IPobierzDaneKontrahentaZGUS pobierzDaneKontrahentaZGUS,
                                        IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.pobierzDaneKontrahentaZGUS = pobierzDaneKontrahentaZGUS;
            this.messenger = messenger;

            kontrahentNipValidationHelper = new KontrahentNipValidationHelper();

            Kontrahent = new tblKontrahent();
            //Kontrahent.MetaSetUp();

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            PobierzDaneZGUSCommand = new RelayCommand(PobierzDaneZGUSCommandExecute, PobierzDaneZGUSCommandCanExecute);


            messenger.Register<tblKontrahent>(this, GdyPrzeslanoKontrahenta);
            PobierzDaneZGusButtonActivate = true;
        }
        #endregion

        private bool PobierzDaneZGUSCommandCanExecute()
        {
            if (string.IsNullOrWhiteSpace(Kontrahent.NIP))
            {
                return false;
            }

            try
            {

                return kontrahentNipValidationHelper.CzyNipPoprawnyDoPobraniaDanychZGus(Kontrahent.NIP);
            }
            catch (Exception ex)
            {
                dialogService.ShowError_BtnOK(ex.Message);
                return false;
            }
        }

        private async void PobierzDaneZGUSCommandExecute()
        {
            PobierzDaneZGusButtonActivate = false;

            try
            {
                Kontrahent = await pobierzDaneKontrahentaZGUS.PobierzAsync(Kontrahent);
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK($"Coś poszło nie tak. \r\n {ex.Message}\r\n{ex.StackTrace}");
                logger.Error(ex, ex.Message);
            }

            PobierzDaneZGusButtonActivate = true;
        }

        #region UsunCommand
        private bool UsunCommandCanExecute()
        {
            if (Kontrahent.ID_Kontrahent == 0)
                return false;
            //jeżeli edytowano nazwe lub nip to nie można usunąć kontrahenta
            if (kontrahentPrzeslany != null)
            {
                if (Kontrahent.Nazwa != kontrahentPrzeslany.Nazwa &&
                    Kontrahent.NIP != kontrahentPrzeslany.NIP)
                {
                    return false;
                }
            }
            return true;
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć kontrahenta?"))
            {
                try
                {
                    unitOfWork.tblKontrahent.Remove(Kontrahent);
                    await unitOfWork.SaveAsync();
                    messenger.Send<string, EwidencjaKontrahentowViewModel_old>("Odswiez");
                    viewService.Close<DodajKontrahentaViewModel_old>();
                }
                catch (Exception e)
                {
                    dialogService.ShowInfo_BtnOK(e.Message);
                }
            }
        }
        #endregion

        #region ZapiszCommand
        private bool ZapiszCommandCanExecute()
        {
            if (Kontrahent.IsValid)
                return true;

            return false;
        }

        private async void ZapiszCommandExecute()
        {
            //jezeli jest kontrahent nowy

            if (Kontrahent.ID_Kontrahent == 0)
            {

                await CzyIstniejeKontrahentOPodanymNIPie();

                if (CzyWbazieIstniejeKontrahentOPodanymNrNIP)
                {
                    if (dialogService.ShowQuestion_BoolResult("Kontrahent o podanym NIP'ie już istnieje w systemie." +
                        "                                 \r\n Czy pomimo to zapisać Kontrahenta?") == false)
                        return;
                }

                try
                {
                    unitOfWork.tblKontrahent.Add(Kontrahent);
                    await unitOfWork.SaveAsync();

                    dialogService.ShowInfo_BtnOK("Kontrahent został zapisany");
                    messenger.Send<string, EwidencjaKontrahentowViewModel_old>("Odswiez");
                    viewService.Close<DodajKontrahentaViewModel_old>();
                }
                catch (Exception e)
                {

                }
            }
            else // jezeli modyfikujemy kontrahenta
            {
                await unitOfWork.SaveAsync();

                dialogService.ShowInfo_BtnOK("Dane Kontrahenta zostały zaktualizowane");
                messenger.Send<string, EwidencjaKontrahentowViewModel_old>("Odswiez");
                viewService.Close<DodajKontrahentaViewModel_old>();
            }
        }
        #endregion

        private async Task CzyIstniejeKontrahentOPodanymNIPie()
        {
            var liczbaKontrahentowZTymSamymNIPem = await unitOfWork.tblKontrahent.WyszukajKontrahentaPoNIP(Kontrahent.NIP);
            if (liczbaKontrahentowZTymSamymNIPem.Count() == 0)
            {
                CzyWbazieIstniejeKontrahentOPodanymNrNIP = false;
            }
            else
            {
                CzyWbazieIstniejeKontrahentOPodanymNrNIP = true;
            }
        }

        private async void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            if (obj != null)
            {
                kontrahentPrzeslany = obj;
                Kontrahent = await unitOfWork.tblKontrahent.GetByIdAsync(obj.ID_Kontrahent);
            }
        }
    }
}
