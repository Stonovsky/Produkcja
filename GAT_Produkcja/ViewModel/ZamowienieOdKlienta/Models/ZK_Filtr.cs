using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models
{
    [AddINotifyPropertyChangedInterface]

    public class ZK_Filtr
    {
        public string NazwaTowaru { get; set; }
        public string Status { get; set; }
        public string Grupa { get; set; }
        public DateTime? DataOd { get; set; } 
        public DateTime? DataDo { get; set; } 
        public DateTime? TerminRealizacjiOd { get; set; } 
        public DateTime? TerminRealizacjiDo { get; set; } 
    }
}
