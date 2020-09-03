using GAT_Produkcja.db;
using GAT_Produkcja.Helpers.Interfaces;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public interface IPodsumowanieZamowieniaOdKlientowHelper : IButtonActive
    {
        PodsFinans_ZamowienieOdKlientowModel Podsumowanie { get; }

        Task<List<PodsFinans_ZamowienieOdKlientowModel>> PodsumujZamowieniaOdKlientow(DateTime dataOd, DateTime dataDo);

    }
}