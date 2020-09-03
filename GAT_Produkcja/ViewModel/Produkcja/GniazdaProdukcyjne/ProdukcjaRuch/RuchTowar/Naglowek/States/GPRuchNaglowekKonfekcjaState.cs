using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States
{
    public class GPRuchNaglowekKonfekcjaState : GPRuchNaglowekStateBase, IGPRuchNaglowekState
    {
        public GPRuchNaglowekKonfekcjaState(IGPRuchNaglowekViewModel naglowekVM) : base(naglowekVM)
        {
        }

        public override bool RwEnabled => true;

        public override bool CzyZlecProdMaBycWidoczne => false;

        public override bool CzyZlecCieciaMaBycWidoczne => true;

        public override bool IsChanged => naglowekVM.RuchTowarRWViewModel.IsChanged
                                        || naglowekVM.RuchTowarPWViewModel.IsChanged;
                                        //|| naglowekVM.IsChanged;

        public override bool IsValid => naglowekVM.RuchTowarRWViewModel.IsValid
                                        && naglowekVM.RuchTowarPWViewModel.IsValid;
                                        //&& naglowekVM.VMEntity.IsValid;

        public override async Task SaveAsync()
        {
            await naglowekVM.RuchTowarRWViewModel.SaveAsync(naglowekVM.VMEntity.IDProdukcjaRuchNaglowek);
            await naglowekVM.RuchTowarPWViewModel.SaveAsync(naglowekVM.VMEntity.IDProdukcjaRuchNaglowek);
        }
    }
}
