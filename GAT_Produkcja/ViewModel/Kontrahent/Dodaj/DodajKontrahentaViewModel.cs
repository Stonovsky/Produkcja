using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Helpers.Kontrahent;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.WebScraper;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Kontrahent.Dodaj
{
    public class DodajKontrahentaViewModel 
        : AddEditCommandGenericViewModelBase<tblKontrahent>
    {
        #region Fields

        private readonly IPobierzDaneKontrahentaZGUS pobierzDaneKontrahentaZGUS;
        private KontrahentNipValidationHelper kontrahentNipValidationHelper;

        #endregion

        #region Properties
        public bool GUSButtonActive { get; set; } = true;
        #endregion

        #region Commands
        public RelayCommand GetDataFromGUSCommand { get; set; }

        #endregion

        public DodajKontrahentaViewModel(IViewModelService viewModelService,
                                        IPobierzDaneKontrahentaZGUS pobierzDaneKontrahentaZGUS)
            : base(viewModelService)
        {
            this.pobierzDaneKontrahentaZGUS = pobierzDaneKontrahentaZGUS;

            GetDataFromGUSCommand = new RelayCommand(GetDataFromGUSExecute, GetDataFromGUSCanExecute);
            kontrahentNipValidationHelper = new KontrahentNipValidationHelper();

        }

        #region GUS

        private async void GetDataFromGUSExecute()
        {
            GUSButtonActive = false;

            try
            {
                VMEntity = await pobierzDaneKontrahentaZGUS.PobierzAsync(VMEntity);
            }
            catch (Exception ex)
            {
                DialogService.ShowInfo_BtnOK($"Coś poszło nie tak. \r\n {ex.Message}\r\n{ex.StackTrace}");
                //Logger .Error(ex, ex.Message); //TODO Logger to log errors => in VMSB
            }

            GUSButtonActive = true;
        }


        private bool GetDataFromGUSCanExecute()
        {
            if (string.IsNullOrWhiteSpace(VMEntity.NIP)) return false;

            try
            {
                return kontrahentNipValidationHelper.CzyNipPoprawnyDoPobraniaDanychZGus(VMEntity.NIP);
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
                return false;
            }
        }

        #endregion
        public override IGenericRepository<tblKontrahent> Repository => UnitOfWork.tblKontrahent;

        public override Func<tblKontrahent, int> GetIdFromEntityWhenSentByMessenger => (k) => k.ID_Kontrahent;

        protected override Func<int> GetVMEntityId => () => VMEntity.ID_Kontrahent ;
    }
}
