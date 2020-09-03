using GAT_Produkcja.dbComarch.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch.Helpers
{
    public class ComarchReposiotriesHelper
    {
        private readonly IUnitOfWorkComarch unitOfWorkComarch;

        public ComarchReposiotriesHelper(IUnitOfWorkComarch unitOfWorkComarch)
        {
            this.unitOfWorkComarch = unitOfWorkComarch;
        }

    }
}
