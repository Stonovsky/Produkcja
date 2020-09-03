using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele
{
    [AddINotifyPropertyChangedInterface]

    public class RozliczenieMsAccessFiltr
    {
        public int? IdZlecenie { get; set; }
        public string TowarNazwa { get; set; }
        public string Przychod { get; set; }

    }
}
