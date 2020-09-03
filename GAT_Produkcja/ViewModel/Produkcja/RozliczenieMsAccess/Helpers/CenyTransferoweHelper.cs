using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class CenyTransferoweHelper : ICenyTransferoweHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaCenTransferowychGTEX;
        #region CTOR
        public CenyTransferoweHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public async Task LoadAsync()
        {
            listaCenTransferowychGTEX = await unitOfWork.tblProdukcjaRozliczenie_CenyTransferowe.WhereAsync(c => c.CzyAktualna == true);
        }

        public decimal PobierzCeneTransferowa(string nazwaTowaruSubiekt)
        {
            if (listaCenTransferowychGTEX is null) throw new ArgumentException("Brak pobranych cen transferowych z bazy GTEX");

            if (nazwaTowaruSubiekt is null ||
                nazwaTowaruSubiekt == string.Empty)
                throw new ArgumentException("Brak nazwy towaru dla pobrania ceny z bazy Subiekt GTEX");

            foreach (var pozycja in listaCenTransferowychGTEX)
            {
                if (nazwaTowaruSubiekt.Contains(pozycja.TowarNazwa))
                    return pozycja.CenaTransferowa;
            }
            return 0;
        }
        public decimal PobierzCeneHurtowa(string nazwaTowaruSubiekt)

        {
            if (listaCenTransferowychGTEX is null) throw new ArgumentException("Brak pobranych cen transferowych z bazy GTEX");

            if (nazwaTowaruSubiekt is null ||
                nazwaTowaruSubiekt == string.Empty)
                throw new ArgumentException("Brak nazwy towaru dla pobrania ceny z bazy Subiekt GTEX");

            foreach (var pozycja in listaCenTransferowychGTEX)
            {
                if (nazwaTowaruSubiekt.Contains(pozycja.TowarNazwa))
                    return pozycja.CenaHurtowa;
            }
            return 0;
        }
    }
}
