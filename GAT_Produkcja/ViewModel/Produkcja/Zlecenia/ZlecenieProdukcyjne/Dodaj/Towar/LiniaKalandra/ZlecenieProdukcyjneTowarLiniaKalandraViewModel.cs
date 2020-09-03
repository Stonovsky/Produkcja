using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaKalandra
{
    public class ZlecenieProdukcyjneTowarLiniaKalandraViewModel : ZlecenieProdukcyjneTowarBase, IZlecenieProdukcyjneTowarLiniaKalandraViewModel
    {
        public override string Title => "Linia kalandra";

        protected override GniazdaProdukcyjneEnum gniazdoProdukcyjne => GniazdaProdukcyjneEnum.LiniaDoKalandowania;

        public ZlecenieProdukcyjneTowarLiniaKalandraViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
        }

    }
}
