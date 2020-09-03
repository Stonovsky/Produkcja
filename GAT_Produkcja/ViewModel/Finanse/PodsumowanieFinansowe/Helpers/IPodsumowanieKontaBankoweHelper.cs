using GAT_Produkcja.db;
using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieKontaBankoweHelper : IButtonActive
    {
        decimal Podsumowanie { get; }
        Task<IEnumerable<PodsFinans_StanyKontModel>> PobierzStanKontZDaty(DateTime data);
    }
}