using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using GAT_Produkcja.dbMsAccess.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess
{
    public static class StaticStartUp
    {
        public static void Initiall()
        {
            FluentMapper.Initialize(cfig =>
            {
                cfig.AddMap(new NormyZuzyciaMapper());
                cfig.AddMap(new ArtykulyMapper());
                cfig.AddMap(new ProdukcjaMapper());
                cfig.AddMap(new KalanderMapper());
                cfig.AddMap(new KonfekcjaMapper());
                cfig.AddMap(new SurowiecMapper());
                cfig.ForDommel();
            });


        }

    }
}
