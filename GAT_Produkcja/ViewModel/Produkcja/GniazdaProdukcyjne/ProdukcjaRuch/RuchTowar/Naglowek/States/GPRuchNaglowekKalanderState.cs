using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States
{
    [AddINotifyPropertyChangedInterface]

    public class GPRuchNaglowekKalanderState : GPRuchNaglowekStateBase, IGPRuchNaglowekState
    {
        public GPRuchNaglowekKalanderState(IGPRuchNaglowekViewModel naglowekVM) : base(naglowekVM)
        {
        }

        public override bool RwEnabled => true;

        public override bool CzyZlecProdMaBycWidoczne => true;

        public override bool CzyZlecCieciaMaBycWidoczne => false;

        public override bool IsChanged => naglowekVM.RuchTowarRWViewModel.IsChanged
                                        || naglowekVM.RuchTowarPWViewModel.IsChanged;
                                        //|| naglowekVM.IsChanged;

        public override bool IsValid => naglowekVM.RuchTowarRWViewModel.IsValid
                                        && naglowekVM.RuchTowarPWViewModel.IsValid;
                                        //&& naglowekVM.IsValid;

        public override async Task SaveAsync()
        {
            await naglowekVM.RuchTowarRWViewModel.SaveAsync(naglowekVM.VMEntity.IDProdukcjaRuchNaglowek);
            await naglowekVM.RuchTowarPWViewModel.SaveAsync(naglowekVM.VMEntity.IDProdukcjaRuchNaglowek);
        }
    }
}
