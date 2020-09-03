using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Models
{
    [AddINotifyPropertyChangedInterface]

    public class PodsumowaniePWModel
    {
        public int IloscSzt { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public decimal Waga_kg { get; set; }
        public decimal Odpad_kg { get; set; }

    }
}
