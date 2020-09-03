using DocumentFormat.OpenXml.Presentation;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.Printers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Konfiguracja.KonfiguracjaUrzadzen
{
    public class KonfiguracjaUrzadzenViewModel : AddEditCommandGenericViewModelBase<tblKonfiguracjaUrzadzen>
    {
        #region Fileds

        private readonly IPrinterService printerService;
        private string selectedPrinter;
        private string scaleCOMPort;
        private string selectedScaleCom;

        #endregion

        #region Properties
        public IEnumerable<string> Printers { get; set; }
        public string SelectedPrinter
        {
            get => selectedPrinter;
            set
            {
                selectedPrinter = value;
                try
                {

                    VMEntity.DrukarkaNazwa = SelectedPrinter;
                    VMEntity.DrukarkaIP = printerService.GetPrinterIP(SelectedPrinter);
                    RaisePropertyChanged(nameof(VMEntity));
                }
                catch (Exception ex)
                {
                    DialogService.ShowError_BtnOK(ex.Message);
                }

            }
        }
        public IEnumerable<string> ComPorts { get; set; }
        public string SelectedScaleCom
        {
            get => selectedScaleCom;
            set
            {
                selectedScaleCom = value;
                VMEntity.WagaComPort = selectedScaleCom;

            }
        }
        #endregion

        #region CTOR

        public KonfiguracjaUrzadzenViewModel(IViewModelService viewModelService,
                                             IPrinterService printerService)
            : base(viewModelService)
        {
            this.printerService = printerService;

            Title = "Konfiguracja urządzeń";
        }

        #endregion

        public override IGenericRepository<tblKonfiguracjaUrzadzen> Repository => UnitOfWork.tblKonfiguracjaUrzadzen;

        public override Func<tblKonfiguracjaUrzadzen, int> GetIdFromEntityWhenSentByMessenger => (e) => e.IDKonfiguracjaUrzadzen;

        protected override Func<int> GetVMEntityId => () => VMEntity.IDKonfiguracjaUrzadzen;

        #region LoadCommand

        protected override async void LoadCommandExecute()
        {
            Printers = await printerService.GetPrintersAsync();
            ComPorts = SerialPort.GetPortNames();

            VMEntity = await UnitOfWork.tblKonfiguracjaUrzadzen.SingleOrDefaultAsync(e => e.NazwaKomputera == Environment.MachineName);

            if (VMEntity is null)
                VMEntity = new tblKonfiguracjaUrzadzen();
            else
            {
                SelectedPrinter = VMEntity.DrukarkaNazwa;
                SelectedScaleCom = VMEntity.WagaComPort;
            }
        }

        #endregion

        #region SaveCommand

        protected override Func<Task> UpdateEntityBeforeSaveAction => async () =>
        {
            VMEntity.NazwaKomputera = Environment.MachineName;
            VMEntity.IDOperator = UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT ?? 7;
            VMEntity.DataDodania = DateTime.Now;
        };
        protected override async Task SaveAsync()
        {
           await  base.SaveAsync();

            UzytkownikZalogowany.KonfiguracjaUrzadzen = VMEntity;
        }
        #endregion
    }
}
