using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
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
    //TODO zmienic klase bazowa lub w ogole ja usunac! i zastapic interfacem
    public abstract class GPRuchNaglowekStateBase : IGPRuchNaglowekState
    {
        protected readonly IGPRuchNaglowekViewModel naglowekVM;
        #region Properties

        public abstract bool RwEnabled { get;  }
        public abstract bool CzyZlecProdMaBycWidoczne { get; }
        public abstract bool CzyZlecCieciaMaBycWidoczne { get; }
        public abstract bool IsChanged { get; }
        public abstract bool IsValid { get; }

        #endregion


        public GPRuchNaglowekStateBase(IGPRuchNaglowekViewModel naglowekVM)
        {
            this.naglowekVM = naglowekVM;

        }



        public abstract Task SaveAsync();
    }
}
