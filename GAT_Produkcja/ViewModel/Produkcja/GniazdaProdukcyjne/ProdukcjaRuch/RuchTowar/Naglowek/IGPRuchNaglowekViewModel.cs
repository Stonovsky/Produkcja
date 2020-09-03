using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek
{
    public interface IGPRuchNaglowekViewModel
    {
        bool CzyZlecCieciaMaBycWidoczne { get; }
        bool CzyZlecProdMaBycWidoczne { get; }
        bool IsChanged { get; }
        bool IsValid { get; }
        RelayCommand DodajZlecenieCieciaCommand { get; set; }
        RelayCommand DodajZlecenieProdukcyjneCommand { get; set; }
        IEnumerable<tblProdukcjaGniazdoProdukcyjne> ListaGniazdProdukcyjnych { get; set; }
        IEnumerable<tblPracownikGAT> ListaPracownikow { get; set; }
        IGenericRepository<tblProdukcjaRuchNaglowek> Repository { get; }
        IGPRuchTowarPWViewModel RuchTowarPWViewModel { get; }
        IGPRuchTowarRWViewModel RuchTowarRWViewModel { get; }
        bool RwEnabled { get; }
        tblProdukcjaGniazdoProdukcyjne WybraneGniazdo { get; set; }
        tblPracownikGAT WybranyPracownik_1 { get; set; }
        tblProdukcjaZlecenieTowar ZlecenieTowar { get; }
        tblProdukcjaRuchNaglowek VMEntity { get; }
    }
}