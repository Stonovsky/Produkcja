using GAT_Produkcja.db;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public interface IZebraZLPLabelGenerator
    {
        string GetInternalHorizontalLabel(tblProdukcjaRuchTowar towar, int iloscEtykietDoDruku);
    }
}