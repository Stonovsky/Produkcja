﻿using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowienieHandloweRepository:IGenericRepository<tblZamowienieHandlowe>
    {
    }

    public class TblZamowienieHandloweRepository : GenericRepository<tblZamowienieHandlowe,GAT_ProdukcjaModel>, ITblZamowienieHandloweRepository
    {
        public TblZamowienieHandloweRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
