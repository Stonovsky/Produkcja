using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.BaseClass
{
    public interface IZlecenieProdukcyjneTowarBase
    {
        string Title { get; }
        Task LoadAsync(int? id);
        Task SaveAsync(int? id);
        ObservableCollection<tblProdukcjaZlecenieTowar> ListOfVMEntities { get; set; }

    }
}
