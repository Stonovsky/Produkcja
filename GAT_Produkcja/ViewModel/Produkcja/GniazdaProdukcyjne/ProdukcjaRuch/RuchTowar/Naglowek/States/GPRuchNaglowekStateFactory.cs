using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States
{
    public class GPRuchNaglowekStateFactory : IGPRuchNaglowekStateFactory
    {
        public IGPRuchNaglowekState GetState(IGPRuchNaglowekViewModel ruchNaglowekViewModel)
        {
            if(ruchNaglowekViewModel is null) 
                    return new GPRuchNaglowekLiniaWlokninState(ruchNaglowekViewModel);

            if (ruchNaglowekViewModel.WybraneGniazdo is null)
                return new GPRuchNaglowekLiniaWlokninState(ruchNaglowekViewModel);

            switch (ruchNaglowekViewModel.WybraneGniazdo.IDProdukcjaGniazdoProdukcyjne)
            {
                case (int)GniazdaProdukcyjneEnum.LiniaWloknin:
                    return new GPRuchNaglowekLiniaWlokninState(ruchNaglowekViewModel);
                case (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    return new GPRuchNaglowekKalanderState(ruchNaglowekViewModel);
                case (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    return new GPRuchNaglowekKonfekcjaState(ruchNaglowekViewModel);

                default:
                    return new GPRuchNaglowekLiniaWlokninState(ruchNaglowekViewModel);
            }
        }
    }
}
