using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.Sprzedaz.Ewidencja.Models
{
    [AddINotifyPropertyChangedInterface]

    public class PodsumowanieSprzedazyModel
    {
        public decimal Ilosc { get; set; }
        public decimal Netto { get; set; }
        public decimal Zysk { get; set; }
        public decimal Marza { get; set; }
    }
}
