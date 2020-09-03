using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers
{
    public class GPRuchTowar_Naglowek_Helper : IGPRuchTowar_Naglowek_Helper
    {
        public IMagazynRuchNaglowekSaveHelper MagazynRuchNaglowekSaveHelper { get; }
        public IMagazynRuchTowarSaveHelper MagazynRuchTowarSaveHelper { get; }

        public GPRuchTowar_Naglowek_Helper(IMagazynRuchNaglowekSaveHelper magazynRuchNaglowekSaveHelper,
                                           IMagazynRuchTowarSaveHelper magazynRuchTowarSaveHelper
                                            )
        {
            MagazynRuchNaglowekSaveHelper = magazynRuchNaglowekSaveHelper;
            MagazynRuchTowarSaveHelper = magazynRuchTowarSaveHelper;
        }

    }
}
