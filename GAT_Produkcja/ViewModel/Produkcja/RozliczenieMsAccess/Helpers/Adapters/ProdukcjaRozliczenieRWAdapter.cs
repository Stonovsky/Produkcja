using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Adapters
{
    public class ProdukcjaRozliczenieRWAdapter
    {
        private readonly NormyZuzycia pozycja;
        private readonly vwMagazynRuchGTX surowiecZSubiekt;
        private readonly int idSurowcaMsAccess;

        public ProdukcjaRozliczenieRWAdapter(NormyZuzycia pozycja, 
                                             vwMagazynRuchGTX surowiecZSubiekt,
                                             int idSurowcaMsAccess)
        {
            this.pozycja = pozycja;
            this.surowiecZSubiekt = surowiecZSubiekt;
            this.idSurowcaMsAccess = idSurowcaMsAccess;
        }


        public tblProdukcjaRozliczenie_RW Konwertuj()
        {
            return new tblProdukcjaRozliczenie_RW
            {
                IDNormaZuzyciaMsAccess = pozycja.Id,
                IDSurowiecMsAccess = idSurowcaMsAccess,
                IDSurowiecSubiekt = surowiecZSubiekt.IdTowar,
                CenaJednostkowa = surowiecZSubiekt.Cena,
                SymbolTowaruSubiekt = surowiecZSubiekt.TowarSymbol,
                NazwaTowaruSubiekt = surowiecZSubiekt.TowarNazwa,
                NazwaSurowcaMsAccess = pozycja.Surowiec,
                NrZlecenia = pozycja.Zlecenie,
                IDZlecenie = pozycja.ZlecenieID,
                Udzial = pozycja.Ilosc,
                DataDodania = DateTime.Now,
                IDJm = (int)JmEnum.kg,
                Jm = "kg"
            };
        }
    }
}
