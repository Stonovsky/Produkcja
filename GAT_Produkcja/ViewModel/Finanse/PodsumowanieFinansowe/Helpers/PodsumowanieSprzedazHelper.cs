using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieSprzedazHelper : IPodsumowanieSprzedazHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private PodsFinans_SprzedazAGGModel podsumowanie;
        private List<PodsFinans_SprzedazAGGModel> sprzedazWDatach;

        public PodsFinans_SprzedazAGGModel Podsumowanie
        {
            get => new PodsFinans_SprzedazAGGModel
            {
                Ilosc_m2 = sprzedazWDatach.Sum(s => s.Ilosc_m2),
                Netto = sprzedazWDatach.Sum(s => s.Netto),
                Zysk = sprzedazWDatach.Sum(s => s.Zysk),
            }
                ; set => podsumowanie = value;
        }
        public bool IsButtonActive { get; set; } = true;

        public PodsumowanieSprzedazHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PodsFinans_SprzedazAGGModel>> PobierzSprzedazAGGWDatach(DateTime dataOd, DateTime dataDo)
        {
            IsButtonActive = false;

            var sprzedaz = await unitOfWork.vwZestSprzedazyAGG.WhereAsync(s => s.DataSprzedazy >= dataOd && s.DataSprzedazy <= dataDo);

            sprzedazWDatach = new List<PodsFinans_SprzedazAGGModel>();

            var podsumowanie = new PodsFinans_SprzedazAGGModel
            {
                Nazwa = "Sprzedaż AGG",
                Ilosc_m2 = sprzedaz.Sum(s => s.Ilosc),
                Netto = sprzedaz.Sum(s => s.Netto),
                Zysk = sprzedaz.Sum(s => s.Zysk),
            };

            sprzedazWDatach.Add(podsumowanie);

            IsButtonActive = true;

            return sprzedazWDatach;
        }
    }
}
