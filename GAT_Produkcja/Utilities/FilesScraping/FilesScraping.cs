using GAT_Produkcja.db;
using GAT_Produkcja.Utilities.ExcelParsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.FilesScraping
{
    public class BadaniaGeowloknin
    {
        const string directoryPath = @"\\192.168.34.57\gat\EMG PLAST\Archiwum laboratorium\01. Raporty - wyniki pomiarów";
        private List<tblWynikiBadanGeowloknin> wynikiBadan;

        public BadaniaGeowloknin()
        {

        }
        public void PrzeszukajFoldery()
        {
            string[] directories;
            string[] subDirectories;
            wynikiBadan = new List<tblWynikiBadanGeowloknin>();

            directories = Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                if (directory.Contains("Włóknin"))
                {
                    subDirectories = Directory.GetDirectories(directory);

                    if (subDirectories.Count()>0)
                    {
                        foreach (var subDirectory in subDirectories)
                        {
                            ProcessAllFilesInDirectory(subDirectory);
                        }
                    }
                    else
                    {
                        ProcessAllFilesInDirectory(directory);
                    }

                }
            }
        }

        private void ProcessAllFilesInDirectory(string directory)
        {
            string[] files;
            files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if (file.Contains(".xls") || file.Contains(".xlsx"))
                {
                    FileInfo info = new FileInfo(file);
                    tblWynikiBadanGeowloknin wynikiBadania = new tblWynikiBadanGeowloknin();


                    wynikiBadania = new Excel_WynikiBadan(file).PobierzWynikiBadanZPlikuExcel();
                    wynikiBadania.DataUtworzeniaPliku = info.CreationTime;
                    wynikiBadania.NazwaPliku = info.Name;
                    wynikiBadania.DataModyfikacjiPliku = info.LastWriteTime;

                    wynikiBadan.Add(wynikiBadania);

                }
            }
        }
    }
}
