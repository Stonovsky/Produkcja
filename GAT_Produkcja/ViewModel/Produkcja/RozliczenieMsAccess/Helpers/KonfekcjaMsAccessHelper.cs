using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class KonfekcjaMsAccessHelper : IKonfekcjaHelper
    {
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        #region CTOR
        public KonfekcjaMsAccessHelper(IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
        }
        #endregion
        public async Task<IEnumerable<IProdukcjaRuchTowar>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia)
        {
            if (idZlecenia == 0)
                throw new ArgumentException("Zerowe idZlecenia przesłane do metody");

            var lista = await unitOfWorkMsAccess.Konfekcja.GetByZlecenieIdAsync(idZlecenia);
            lista = lista.Where(k => k.CzyZaksiegowano == false)
                         .Where(k => k.Przychody == "Linia")
                         .Where(k => k.NrWz != "0")
                         .ToList();
            return lista.Select(k => new KonfekcjaAdapter(k)); 
        }

        public async Task<IEnumerable<IProdukcjaRuchTowar>> PobierzKonfekcjeDoRozliczenia()
        {
            var listaKonfekcji = await unitOfWorkMsAccess.Konfekcja.GetUnaccountedAsync().ConfigureAwait(false);

            if (listaKonfekcji is null)
                throw new ArgumentException("Brak wyprodukowanych rolek dla danych parametrów.");

            listaKonfekcji = listaKonfekcji.Where(l => l.NrWz != "0").OrderByDescending(x => x.Data);


            return listaKonfekcji.Select(konf => new KonfekcjaAdapter(konf));
        }

        public Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> PobierzCeneMieszankiDlaZleceniaProdukcji(int idZlecenieProdukcyjne)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<tblProdukcjaRuchTowar>> IKonfekcjaHelper.PobierzKonfekcjeDoRozliczenia()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<tblProdukcjaRuchTowar>> IKonfekcjaHelper.PobierzListeKonfekcjiDlaZlecenia(int idZlecenia)
        {
            throw new NotImplementedException();
        }
    }
}
