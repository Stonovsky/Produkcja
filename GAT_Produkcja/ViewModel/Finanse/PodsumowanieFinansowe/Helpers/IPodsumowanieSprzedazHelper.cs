using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieSprzedazHelper : IButtonActive
    {
        PodsFinans_SprzedazAGGModel Podsumowanie { get; }

        Task<IEnumerable<PodsFinans_SprzedazAGGModel>> PobierzSprzedazAGGWDatach(DateTime dataOd, DateTime dataDo);
    }
}