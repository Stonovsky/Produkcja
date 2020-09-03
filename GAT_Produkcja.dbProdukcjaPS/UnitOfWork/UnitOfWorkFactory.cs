using GAT_PRodukcja.dbProdukcjaPS;
using GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWork ctx { get; private set; }

        public IUnitOfWork Create()
        {
            ctx= new UnitOfWork(new ProdukcjaPSDbContext());
            return ctx;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
