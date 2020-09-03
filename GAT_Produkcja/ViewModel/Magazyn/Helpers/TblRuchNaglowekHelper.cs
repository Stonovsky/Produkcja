using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Helpers
{
    public class TblRuchNaglowekHelper : ITblRuchNaglowekHelper
    {
        private readonly IUnitOfWork unitOfWork;

        public TblRuchNaglowekHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<NrDokumentuRuchNaglowek> NrDokumentuGenerator(StatusRuchuTowarowEnum statusRuchuTowarowEnum)
        {
            var statusRuchu = await unitOfWork.tblRuchStatus.GetByIdAsync((int)statusRuchuTowarowEnum);
            int nrDokumentu = await PobierzNowyNrDokumentu(statusRuchuTowarowEnum);

            if (nrDokumentu == 0)
                return null;

            return new NrDokumentuRuchNaglowek
            {
                NrDokumentu = nrDokumentu,
                PelnyNrDokumentu = $"{statusRuchu.Symbol.Trim()} {nrDokumentu}/{DateTime.Now.Year}"
            };
        }

        private async Task<int> PobierzNowyNrDokumentu(StatusRuchuTowarowEnum statusRuchuTowarowEnum)
        {
            int nrDokumentu = 0;
            var listaNaglowkowDlaStatusu = await unitOfWork.tblRuchNaglowek.WhereAsync(n => n.IDRuchStatus == (int)statusRuchuTowarowEnum);
            if (listaNaglowkowDlaStatusu.Count() == 0)
            {
                nrDokumentu += 1;
            }
            else
            {
                var numerDokumentuOstatni = listaNaglowkowDlaStatusu.Max(n => n.NrDokumentu);
                nrDokumentu = numerDokumentuOstatni.GetValueOrDefault() + 1;
            }
            return nrDokumentu;
        }
    }
}
