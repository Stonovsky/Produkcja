﻿using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowienieHandloweTowarGeowlokninaRepository:IGenericRepository<tblZamowienieHandloweTowarGeowloknina>
    {
    }

    public class TblZamowienieHandloweTowarGeowlokninaRepository : GenericRepository<tblZamowienieHandloweTowarGeowloknina,GAT_ProdukcjaModel>, ITblZamowienieHandloweTowarGeowlokninaRepository
    {
        public TblZamowienieHandloweTowarGeowlokninaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
