using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.Utilities.FilesManipulations
{
    public class FilesManipulation : IFilesManipulation
    {
        const string TARGET_PATH = @"\\192.168.34.57\gat\GAT\10. BAZA DANYCH\Pliki";
        private string[] sciezkiPlikow;

        public IEnumerable<tblPliki> GenerujKolekcjePlikowIKopiujNaSerwer(int? nrZapotrzebowania, string newFileName = null)
        {

            PobierzListeSciezekPlikow();

            if (sciezkiPlikow.Count()==0 || sciezkiPlikow==null)
            {
                return null;
            }

            List<tblPliki> kolekcjaPlikow = new List<tblPliki>();

            foreach (var file in sciezkiPlikow)
            {
                string nazwaPliku = Path.GetFileName(file);
                nazwaPliku = nrZapotrzebowania + "_ZP_" + nazwaPliku;

                string destinantionFileName = Path.Combine(TARGET_PATH, Path.GetFileName(file));
                File.Copy(file, destinantionFileName);

                var plikModel = new tblPliki();
                plikModel.NazwaPliku = Path.GetFileName(file);
                plikModel.SciezkaPliku = destinantionFileName;

                kolekcjaPlikow.Add(plikModel);
            }

            return kolekcjaPlikow;
        }

        private void PobierzListeSciezekPlikow()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            if (fileDialog.FileName == string.Empty)
            {
                return ;
            }

            sciezkiPlikow = fileDialog.FileNames;

        }
    }
}
