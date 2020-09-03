using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models
{
    [AddINotifyPropertyChangedInterface]

    public class ZK_Podsumowanie
    {
        public decimal Ilosc { get; set; }
        public decimal Wartosc { get; set; }
    }
}
