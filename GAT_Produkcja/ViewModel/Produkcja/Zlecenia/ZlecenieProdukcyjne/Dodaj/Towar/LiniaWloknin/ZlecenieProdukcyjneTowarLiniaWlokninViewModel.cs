using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaWloknin
{
    public class ZlecenieProdukcyjneTowarLiniaWlokninViewModel : ZlecenieProdukcyjneTowarBase, IZlecenieProdukcyjneTowarLiniaWlokninViewModel
    {
        public override string Title => "Linia włóknin";
        protected override GniazdaProdukcyjneEnum gniazdoProdukcyjne => GniazdaProdukcyjneEnum.LiniaWloknin;
        public ZlecenieProdukcyjneTowarLiniaWlokninViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
        }

    }
}
