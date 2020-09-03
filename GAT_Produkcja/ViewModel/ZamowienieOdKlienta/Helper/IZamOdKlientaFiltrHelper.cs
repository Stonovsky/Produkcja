using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper
{
    public interface IZamOdKlientaFiltrHelper
    {
        Task<IEnumerable<vwZamOdKlientaAGG>> FiltrujAsync(ZK_Filtr filtr);
    }
}
