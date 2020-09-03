using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWork ctx { get; private set; }

        public IUnitOfWork Create()
        {
            ctx= new UnitOfWork(new GAT_ProdukcjaModel());
            return ctx;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
