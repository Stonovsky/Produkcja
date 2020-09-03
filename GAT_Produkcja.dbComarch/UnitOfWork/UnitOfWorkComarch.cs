using GAT_Produkcja.dbComarch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch.UnitOfWork

{
    public class UnitOfWorkComarch : IUnitOfWorkComarch
    {
        public ISurowiecRepository Surowiec { get; set; }

        public UnitOfWorkComarch()
        {
            Surowiec = new SurowiecRepository();
        }
    }
}
