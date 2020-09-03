using GAT_Produkcja.db;
using System.Collections.Generic;

namespace GAT_Produkcja.Utilities.FilesManipulations
{
    public interface IFilesManipulation
    {
        IEnumerable<tblPliki> GenerujKolekcjePlikowIKopiujNaSerwer(int? nrZapotrzebowania,string newFileName = null);

    }
}