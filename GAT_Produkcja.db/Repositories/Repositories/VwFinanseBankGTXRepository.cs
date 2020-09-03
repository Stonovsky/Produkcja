﻿using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFinanseBankGTXRepository:IGenericRepository<vwFinanseBankGTX>
    {
    }

    public class VwFinanseBankGTXRepository : GenericRepository<vwFinanseBankGTX, GAT_ProdukcjaModel>, IVwFinanseBankGTXRepository
    {
        public VwFinanseBankGTXRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
