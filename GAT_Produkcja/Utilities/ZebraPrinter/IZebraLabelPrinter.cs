using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter
{
    public interface IZebraLabelPrinter
    {
        Task PrintAsync(LabelModel labelModel);

        string GetPrinterName();
        Task PrintLabelCE(LabelModel labelModel, tblTowarGeowlokninaParametry geowlokninaParametry);

    }
}