using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class Dyspozycje_tblProdukcjaZlecenieAdapter : tblProdukcjaZlecenie
    {
        private readonly Dyspozycje dyspozycje;

        public Dyspozycje_tblProdukcjaZlecenieAdapter(Dyspozycje dyspozycje)
        {
            this.dyspozycje = dyspozycje;
        }

        public override int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaWloknin;
        public override int? IDProdukcjaRozliczenieStatus => default;
        public override int IDProdukcjaZlecenieStatus => (int)ProdukcjaZlecenieStatusEnum.Zakonczone;
        public override int? IDKontrahent => default;
        public override int? IDWykonujacy => 18;
        public override int IDZlecajacy => 18; // Piotr Spiewak
        public override decimal CenaMieszanki_zl => default; // todod
        public override DateTime? DataRozpoczecia => dyspozycje.Data;
        public override DateTime? DataRozpoczeciaFakt => dyspozycje.Data;
        public override DateTime DataUtworzenia => dyspozycje.Data;
        public override DateTime? DataZakonczenia => dyspozycje.Data; // todo
        public override DateTime? DataRozliczenia => default; //todo
        public override DateTime? DataZakonczeniaFakt => dyspozycje.Data;
        public override string KodKreskowy => default;
        public override string NazwaZlecenia => dyspozycje.NrZlecenia;
        public override string NrDokumentu => default;
        public override int? NrZlecenia => dyspozycje.Id;
        public override decimal Zaawansowanie => 1;

        public tblProdukcjaZlecenie Generuj()
        {
            return new tblProdukcjaZlecenie
            {

                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin,
                IDProdukcjaRozliczenieStatus = default,
                IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Zakonczone,
                IDKontrahent = default,
                IDWykonujacy = default,
                IDZlecajacy = 18, // Piotr Spiewak
                CenaMieszanki_zl = default, // todo
                DataRozpoczecia = dyspozycje.Data,
                DataRozpoczeciaFakt = dyspozycje.Data,
                DataUtworzenia = dyspozycje.Data,
                DataZakonczenia = dyspozycje.Data, // todo
                DataRozliczenia = default, //todo
                DataZakonczeniaFakt = dyspozycje.Data,
                KodKreskowy = dyspozycje.Id+dyspozycje.Data.ToString("ddMMyyyy"),
                NazwaZlecenia = dyspozycje.NrZlecenia,
                NrDokumentu = default,
                NrZlecenia = dyspozycje.Id,
                
                Zaawansowanie = 1,
                IDMsAccess = dyspozycje.Id,
                
            };
        }
    }
}
