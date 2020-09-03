using GAT_Produkcja.dbMsAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.UnitOfWork
{
    public class UnitOfWorkMsAccess : IUnitOfWorkMsAccess
    {
        public IArtykulyRepository Artykuly { get; set; }
        public IProdukcjaRepository Produkcja { get; set; }
        public IKalanderRepository Kalander { get; set; }
        public IKonfekcjaRepository Konfekcja { get; set; }
        public ISurowiecRepository Surowiec { get; set; }
        public INormyZuzyciaRepository NormyZuzycia { get; set; }
        public IDyspozycjeRepository Dyspozycje { get ; set ; }

        public UnitOfWorkMsAccess()
        {
            Artykuly = new ArtykulyRepository();
            Produkcja = new ProdukcjaRepository();
            Kalander = new KalanderRepository();
            Konfekcja = new KonfekcjaRepository();
            Surowiec = new SurowiecRepository();
            NormyZuzycia = new NormyZuzyciaRepository();
            Dyspozycje = new DyspozycjeRepository();
        }
    }
}
