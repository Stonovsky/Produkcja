using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieMagazynyHelper : IButtonActive
    {
        PodsFinans_MagazynyModel Podsumowanie { get; }
        Task<IEnumerable<PodsFinans_MagazynyModel>> PobierzPodsumowanieMagazynuDoDaty<T>(T TMagazyn, DateTime dataDo);
    }
}