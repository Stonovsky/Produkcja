using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.ZebraPrinter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public class ZebraS4MService : IZebraS4MService
    {
        private readonly IZebraZPLCELabelGenerator zebraZPLCELabel;
        private readonly IZebraZLPLabelGenerator zebraZLPLabel;
        private readonly IZebraS4MPrinter zebraS4MPrinter;
        private readonly IUnitOfWork unitOfWork;
        private string printerIP;
        private tblKonfiguracjaUrzadzen konfiguracja;

        #region CTOR

        public ZebraS4MService(IZebraZPLCELabelGenerator zebraZPLCELabel,
                               IZebraZLPLabelGenerator zebraZLPLabel,
                               IZebraS4MPrinter zebraS4MPrinter,
                               IUnitOfWork unitOfWork)
        {
            this.zebraZPLCELabel = zebraZPLCELabel;
            this.zebraZLPLabel = zebraZLPLabel;
            this.zebraS4MPrinter = zebraS4MPrinter;
            this.unitOfWork = unitOfWork;

        }

        #endregion

        public async Task LoadAsync()
        {
            //TODO do zmiany na UzytkownikZalogowany.KonfiguracjaUrzadzen;

            konfiguracja = await unitOfWork.tblKonfiguracjaUrzadzen.SingleOrDefaultAsync(p => p.NazwaKomputera == Environment.MachineName);

            if (konfiguracja is null) throw new ZebraException("Brak skonfigurowanej drukarki ZEBRA");
        }

        public void PrintInternalLabel(tblProdukcjaRuchTowar towar, int iloscEtykiet)
        {
            var zlpMessage = zebraZLPLabel.GetInternalHorizontalLabel(towar, iloscEtykiet);
            zebraS4MPrinter.SendZplOverTcp(printerIP, zlpMessage);
        }

        public async Task PrintInternalLabelAsync(tblProdukcjaRuchTowar towar, int iloscEtykiet)
        {
            await Task.Run(() => PrintInternalLabel(towar, iloscEtykiet));
        }

        public async Task PrintCELabelAsync(tblProdukcjaRuchTowar towar, int iloscEtykiet, bool czyUV)
        {
            var zlpMessage = await zebraZPLCELabel.GetLabelCE(towar, iloscEtykiet, czyUV);
            await Task.Run(() => zebraS4MPrinter.SendZplOverTcp(printerIP, zlpMessage));
        }

        public bool CanPrint()
        {
            //printerIP = UzytkownikZalogowany.KonfiguracjaUrzadzen.DrukarkaIP;
            if (konfiguracja is null) return false;

            return !string.IsNullOrEmpty(konfiguracja.DrukarkaIP);
        }
    }
}
