using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.EppFile
{
    public interface IEppFileGenerator
    {
        Task GenerujPlikEPP(StatusRuchuTowarowEnum statusRuchu, tblProdukcjaRozliczenie_Naglowek naglowek, IEnumerable<IProdukcjaRozliczenie> listaPozycji, string sciezkaPliku, string uwagiDokumentu = "");
    }
}