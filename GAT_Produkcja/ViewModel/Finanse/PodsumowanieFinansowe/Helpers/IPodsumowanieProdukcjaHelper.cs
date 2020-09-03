using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieProdukcjaHelper : IButtonActive
    {
        PodsFinans_ProdukcjaModel Podsumowanie { get; }

        Task<List<PodsFinans_ProdukcjaModel>> PobierzPodsumowanieProdukcjiWDatach(DateTime dataOd, DateTime dataDo);
        Task LoadAsync();
    }
}