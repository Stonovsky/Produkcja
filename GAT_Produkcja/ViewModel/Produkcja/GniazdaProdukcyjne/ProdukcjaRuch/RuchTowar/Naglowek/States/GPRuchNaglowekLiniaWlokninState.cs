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
    public class GPRuchNaglowekLiniaWlokninState : GPRuchNaglowekStateBase, IGPRuchNaglowekState
    {
        public GPRuchNaglowekLiniaWlokninState(IGPRuchNaglowekViewModel naglowekVM) : base(naglowekVM)
        {
        }

        public override bool RwEnabled => false;
        public override bool CzyZlecProdMaBycWidoczne => true;
        public override bool CzyZlecCieciaMaBycWidoczne => false;

        public override bool IsChanged => naglowekVM.RuchTowarPWViewModel.IsChanged;

        public override bool IsValid => naglowekVM.RuchTowarPWViewModel.IsValid;

        public override async Task SaveAsync()
        {
            await naglowekVM.RuchTowarPWViewModel.SaveAsync(naglowekVM.VMEntity.IDProdukcjaRuchNaglowek);
        }
    }
}
