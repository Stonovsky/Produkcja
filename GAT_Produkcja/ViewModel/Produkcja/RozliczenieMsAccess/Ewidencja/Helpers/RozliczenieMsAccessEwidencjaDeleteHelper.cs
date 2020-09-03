using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers
{
    public class RozliczenieMsAccessEwidencjaDeleteHelper : IRozliczenieMsAccessEwidencjaDeleteHelper
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private IEnumerable<tblProdukcjaRozliczenie_PW> listaRolekKonfekcjiDoUsuniecia;
        #endregion

        #region CTOR
        public RozliczenieMsAccessEwidencjaDeleteHelper(IUnitOfWork unitOfWork,
                                                        IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
        }
        #endregion

        public async Task UsunRozliczenieAsync(tblProdukcjaRozliczenie_PWPodsumowanie rozliczenie)
        {
            listaRolekKonfekcjiDoUsuniecia = await unitOfWork.tblProdukcjaRozliczenie_PW.WhereAsync(e => e.IDProdukcjaRozliczenie_Naglowek == rozliczenie.IDProdukcjaRozliczenie_Naglowek);

            if (listaRolekKonfekcjiDoUsuniecia is null
                || !listaRolekKonfekcjiDoUsuniecia.Any())
                throw new ArgumentException("Brak rolek do usunięcia.");

            await ZmienRekordyWBazieMsAccessNaNierozliczone(listaRolekKonfekcjiDoUsuniecia);
            await UsunRozliczenieZTabelSql(rozliczenie);
            await unitOfWork.SaveAsync();
        }

        private async Task UsunRozliczenieZTabelSql(tblProdukcjaRozliczenie_PWPodsumowanie rozliczenie)
        {
            //PW
            unitOfWork.tblProdukcjaRozliczenie_PW.RemoveRange(listaRolekKonfekcjiDoUsuniecia);

            //RW
            var listaRWdoUsuniecia = await unitOfWork.tblProdukcjaRozliczenie_RW.WhereAsync(e => e.IDProdukcjaRozliczenie_Naglowek == rozliczenie.IDProdukcjaRozliczenie_Naglowek);
            unitOfWork.tblProdukcjaRozliczenie_RW.RemoveRange(listaRWdoUsuniecia);

            //PW podsumowanie
            var listaPW_Podsumowanie_doUsuniecia = await unitOfWork.tblProdukcjaRozliczenie_PWPodsumowanie.WhereAsync(e => e.IDProdukcjaRozliczenie_Naglowek == rozliczenie.IDProdukcjaRozliczenie_Naglowek);
            unitOfWork.tblProdukcjaRozliczenie_PWPodsumowanie.RemoveRange(listaPW_Podsumowanie_doUsuniecia);
        }

        private async Task ZmienRekordyWBazieMsAccessNaNierozliczone(IEnumerable<tblProdukcjaRozliczenie_PW> listaRolekDoUsuniecia)
        {
            await unitOfWorkMsAccess.Konfekcja.UpdateRangeNieZaksiegowano(listaRolekDoUsuniecia);
        }
    }
}
