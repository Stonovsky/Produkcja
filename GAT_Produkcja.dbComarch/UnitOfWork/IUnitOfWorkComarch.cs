using GAT_Produkcja.dbComarch.Repositories;

namespace GAT_Produkcja.dbComarch.UnitOfWork
{
    public interface IUnitOfWorkComarch
    {
        ISurowiecRepository Surowiec { get; set; }
    }
}