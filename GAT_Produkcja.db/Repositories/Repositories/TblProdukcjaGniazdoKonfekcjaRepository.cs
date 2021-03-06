﻿using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoKonfekcjaRepository : IGenericRepository<tblProdukcjaGniazdoKonfekcja>
    {
    }

    public class TblProdukcjaGniazdoKonfekcjaRepository : GenericRepository<tblProdukcjaGniazdoKonfekcja, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoKonfekcjaRepository
    {
        public TblProdukcjaGniazdoKonfekcjaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
