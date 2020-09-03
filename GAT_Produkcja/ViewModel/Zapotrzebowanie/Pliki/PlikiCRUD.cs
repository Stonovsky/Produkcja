using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki
{
    public class PlikiCRUD : IPlikiCRUD
    {
        const string TARGET_PATH = @"\\192.168.34.57\gtex\10. PRODUKCJA\Program\Files";
        private string[] sciezkiPlikow;
        private List<tblPliki> kolekcjaPlikowDoDodaniaDoBazy;
        private tblZapotrzebowanie zapotrzebowanie;
        private vwFVKosztowezSubiektGT rozliczenie;

        #region CTOR
        public PlikiCRUD()
        {

        }
        #endregion


        public List<tblPliki> PobierzListePlikowDoDodania(object obj)
        {
            if (obj as tblZapotrzebowanie!=null)
            {
                this.zapotrzebowanie = obj as tblZapotrzebowanie;
                GenerujKolekcjePlikowIKopiujNaSerwer();

                return kolekcjaPlikowDoDodaniaDoBazy;
            }
            else
            {
                this.rozliczenie = obj as vwFVKosztowezSubiektGT;
                GenerujKolekcjePlikowIKopiujNaSerwer();

                return kolekcjaPlikowDoDodaniaDoBazy;
            }

        }


        public void UsunPlikZSerwera (IEnumerable<tblPliki> pliki)
        {
            if (pliki.Count()==0)
            {
                return;
            }

            foreach (var plik in pliki)
            {
                File.Delete(plik.SciezkaPliku);
            }
        }

        private List<tblPliki> GenerujKolekcjePlikowIKopiujNaSerwer()
        {
            PobierzListeSciezekPlikow();

            if (sciezkiPlikow == null || sciezkiPlikow.Count() == 0)
            {
                return null;
            }

            kolekcjaPlikowDoDodaniaDoBazy = new List<tblPliki>();

            foreach (var plik in sciezkiPlikow)
            {

                string sciezkaPlikuZSerwera = GenerujSciezkePlikuNaSerwerze(plik);

                tblPliki plikModel = PrzygotujModelPlikuDoDodania(plik, sciezkaPlikuZSerwera);

                kolekcjaPlikowDoDodaniaDoBazy.Add(plikModel);
            }

            return kolekcjaPlikowDoDodaniaDoBazy;
        }
        public void KopiujPlikNaSerwer(tblPliki plik)
        {
            File.Copy(plik.SciezkaLokalnaPliku, plik.SciezkaPliku,true);

        }
        private string GenerujSciezkePlikuNaSerwerze(string plik)
        {
            if (zapotrzebowanie!=null)
            {
                string nazwaPliku = Path.GetFileName(plik);
                nazwaPliku = zapotrzebowanie.Nr + "_ZP_" + nazwaPliku;

                string destinantionFileName = Path.Combine(TARGET_PATH, nazwaPliku);

                return destinantionFileName;
            }
            else
            {
                string nazwaPliku = Path.GetFileName(plik);
                nazwaPliku = rozliczenie.NrZP + "_FV_" + nazwaPliku;

                string destinantionFileName = Path.Combine(TARGET_PATH, nazwaPliku);

                return destinantionFileName;
            }
        }
        private tblPliki PrzygotujModelPlikuDoDodania(string sciezkaLokalnaPliku, string sciezkaPlikuSerwer)
        {
            var plikModel = new tblPliki();
            plikModel.NazwaPliku = Path.GetFileName(sciezkaPlikuSerwer);
            plikModel.SciezkaLokalnaPliku = sciezkaLokalnaPliku;
            plikModel.SciezkaPliku = sciezkaPlikuSerwer;

            plikModel.NrZP = PobierzNumerZapotrzebowania();

            return plikModel;
        }
        private string PobierzNumerZapotrzebowania()
        {
            if (zapotrzebowanie!=null)
            {
                return zapotrzebowanie.Nr.ToString();
            }
            else
            {
                return rozliczenie.NrZP;
            }

        }

        private void PobierzListeSciezekPlikow()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            if (fileDialog.FileName == string.Empty)
            {
                return;
            }

            sciezkiPlikow = fileDialog.FileNames;
        }
    }
}
