using System;

namespace GAT_Produkcja.db.Repositories.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        void Dispose();
    }
}