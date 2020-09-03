using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States
{
    public interface IGPRuchNaglowekState
    {
        /// <summary>
        /// Flaga do ukrywania RW na formularzu - ukrywamy tylko dla linii wloknin
        /// </summary>
        bool RwEnabled { get; }

        /// <summary>
        /// Flaga do pokazywania zlecenia ciecia na formularzu naglowka
        /// </summary>
        bool CzyZlecCieciaMaBycWidoczne { get; }
        
        /// <summary>
        /// Flaga do pokazywania zlecenia produkcyjnego na formularzu naglowka
        /// </summary>
        bool CzyZlecProdMaBycWidoczne { get; }
        bool IsChanged { get; }
        bool IsValid { get; }

        Task SaveAsync();

    }
}
