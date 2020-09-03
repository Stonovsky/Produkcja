using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieNaleznosciIZobowiazaniaHelper : IButtonActive
    {
        PodsFinans_NaleznosciIZobowiazaniaModel Podsumowanie { get; }

        Task<IEnumerable<PodsFinans_NaleznosciIZobowiazaniaModel>> PobierzPodsumowanieNalzenosciIZobowiazan(DateTime dataDo);
    }
}