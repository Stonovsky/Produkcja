using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.MainMenu.Models
{
    [AddINotifyPropertyChangedInterface]

    public class KursyWalutModel
    {
        public decimal EUR { get; set; }
        public decimal USD { get; set; }
        public decimal RUB { get; set; }
    }
}
