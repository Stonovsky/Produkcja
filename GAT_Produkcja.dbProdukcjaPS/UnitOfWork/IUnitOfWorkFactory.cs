using GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork;
using System;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        void Dispose();
    }
}