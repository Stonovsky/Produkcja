using GAT_Produkcja.dbMsAccess.Repositories;

namespace GAT_Produkcja.dbMsAccess.UnitOfWork
{
    public interface IUnitOfWorkMsAccess
    {
        IArtykulyRepository Artykuly { get; set; }
        IKalanderRepository Kalander { get; set; }
        IKonfekcjaRepository Konfekcja { get; set; }
        INormyZuzyciaRepository NormyZuzycia { get; set; }
        IProdukcjaRepository Produkcja { get; set; }
        ISurowiecRepository Surowiec { get; set; }
        IDyspozycjeRepository Dyspozycje { get; set; }
    }
}