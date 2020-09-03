using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAT_Produkcja.Services
{
    public class OpenSaveDialogService : IOpenSaveDialogService
    {

        public string SaveFile()
        {
            SaveFileDialog saveDialogBox = new SaveFileDialog();
            saveDialogBox.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveDialogBox.DefaultExt = "xlsx";
            saveDialogBox.AddExtension = true;

            if (saveDialogBox.ShowDialog() == DialogResult.OK)
                return saveDialogBox.FileName;

            return null;
        }

        public string OpenFile()
        {
            var openDialogBox = new OpenFileDialog();
            if (openDialogBox.ShowDialog() == DialogResult.OK)
                return openDialogBox.FileName;

            return null;
        }
    }
}
