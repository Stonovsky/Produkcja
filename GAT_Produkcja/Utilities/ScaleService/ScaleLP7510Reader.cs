using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Utilities.ScaleService.Exceptions;
using GAT_Produkcja.ViewModel.Konfiguracja.KonfiguracjaUrzadzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService
{
    public class ScaleLP7510Reader : ScaleReaderBase, IScaleLP7510Reader
    {
        private readonly IUnitOfWork unitOfWork;
        private tblKonfiguracjaUrzadzen konfiguracjaUrzadzen;
        #region Properties
        protected override string MessageToScale => "R";

        #endregion

        #region CTOR
        public ScaleLP7510Reader(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public override Task<decimal> GetWeight()
        {
            return base.GetWeight();
        }

        public override Task<string> GetWeightInString()
        {
            return base.GetWeightInString();
        }

        public async Task LoadAsync()
        {
            konfiguracjaUrzadzen = await unitOfWork.tblKonfiguracjaUrzadzen.SingleOrDefaultAsync(w => w.NazwaKomputera == Environment.MachineName);
            if (konfiguracjaUrzadzen is null) throw new ScaleException("Brak skonfigurowanej wagi");

            ComPortName = konfiguracjaUrzadzen?.WagaComPort;
        }

        protected override string MessageParser(string scaleMessage)
        {
            ScaleModel.WeightStatus = scaleMessage.Substring(0, 2);
            ScaleModel.WeightMode = scaleMessage.Substring(3, 2);
            ScaleModel.PositiveNegativeSymbol = scaleMessage.Substring(6, 1);
            ScaleModel.Weight = scaleMessage.Substring(7, 7);
            ScaleModel.Unit = scaleMessage.Substring(14, 2);

            return ScaleModel.Weight.Trim();
        }

    }
}
