using GAT_Produkcja.db;
using GAT_Produkcja.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku
{
    public class BadaniaGeowloknin : IBadaniaGeowloknin
    {
        const string directoryPath = @"\\192.168.34.57\gat\EMG PLAST\Archiwum laboratorium\01. Raporty - wyniki pomiarów";

        private readonly IBadaniaGeowlokninRepository repository;
        private readonly IBadaniaGeowlokninWynikiZPliku plikZBadaniem;
        private readonly IDialogService dialogService;
        private List<tblWynikiBadanGeowloknin> wynikiBadan;
        private ObservableCollection<tblWynikiBadanGeowloknin> listaBadanZBazy;
        private int licznikPlikowDoDodania;
        private int licznikPlikowDoOdswiezenia;
        private List<string> listaBadanDoDodania;
        private List<string> listaBadanDoOdswiezenia;

        #region CTOR
        public BadaniaGeowloknin(IBadaniaGeowlokninRepository repository,
                                 IBadaniaGeowlokninWynikiZPliku plikZBadaniem,
                                 IDialogService dialogService)
        {
            this.repository = repository;
            this.plikZBadaniem = plikZBadaniem;
            this.dialogService = dialogService;
        }
        #endregion
        public async Task DodajBadaniaZPlikowExcel()
        {
            await StworzListeBadanDoDodaniaIOdswiezenia();
            
            if (licznikPlikowDoDodania > 0)
            {
                DodajNoweBadaniaZPlikowExcel();
                await repository.DodajBadaniaDoBazyAsync(wynikiBadan);
                dialogService.ShowInfo_BtnOK($"Zakutalizowano zestawienie o nowe pozycje w liczbie: {licznikPlikowDoDodania}");
            }
            else if (licznikPlikowDoOdswiezenia>0)
            {
                await UaktualnijBadaniaZPlikowExcel();
            }
            else
            {
                dialogService.ShowInfo_BtnOK("Zestawienie aktualne, brak plików do zaktualizowania");
            }
        }

        private List<string> PobierzWszystkiePlikiExcelZBadaniami()
        {
            string[] directories;
            string[] subDirectories;
            string[] files;
            List<string> listaPlikow = new List<string>();


            directories = Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                if (directory.Contains("Włóknin"))
                {
                    subDirectories = Directory.GetDirectories(directory);

                    if (subDirectories.Count() > 0)
                    {
                        foreach (var subDirectory in subDirectories)
                        {
                            files = Directory.GetFiles(subDirectory);
                            foreach (var file in files)
                            {
                                if (file.Contains(".xls") || file.Contains(".xlsx") )
                                {
                                    listaPlikow.Add(file);
                                }
                            }
                        }
                    }
                    else
                    {
                        files = Directory.GetFiles(directory);
                        foreach (var file in files)
                        {
                            if (file.Contains(".xls") || file.Contains(".xlsx"))
                            {
                                listaPlikow.Add(file);
                            }
                        }
                    }
                }
            }
            return listaPlikow;

        }
        private async Task StworzListeBadanDoDodaniaIOdswiezenia()
        {
            listaBadanDoDodania = new List<string>();
            listaBadanDoOdswiezenia = new List<string>();

            licznikPlikowDoDodania = 0;
            licznikPlikowDoOdswiezenia = 0;

            var listaWszystkichPlikowZBadaniami = await Task.Run(()=>PobierzWszystkiePlikiExcelZBadaniami());

            await PobierzListeBadanZBazyIUstawBrakStatusu();

            foreach (var plik in listaWszystkichPlikowZBadaniami)
            {
                FileInfo fileInfo = new FileInfo(plik);

                #region test
                //var plikZBazy = listaBadanZBazy.Where(l => l.NazwaPliku == fileInfo.Name).SingleOrDefault();
                //var dataModyfikacji = plikZBazy.DataModyfikacjiPliku;
                //var dataUtworzenia = plikZBazy.DataUtworzeniaPliku;

                //dataModyfikacji = fileInfo.LastWriteTime;
                //dataUtworzenia = fileInfo.CreationTime; 
                #endregion
                if (listaBadanZBazy
                                .Where(f => f.NazwaPliku== fileInfo.Name)
                                .Count() == 0)
                                //.Where(f => f.SciezkaPliku.Replace(@"\",string.Empty)== fileInfo.FullName.Replace(@"\",string.Empty))
                {

                    listaBadanDoDodania.Add(plik);
                    licznikPlikowDoDodania++;
                }
                else if (listaBadanZBazy
                                    .Where(c=>c.NazwaPliku == fileInfo.Name)
                                    .Where(c=>c.DataModyfikacjiPliku!=fileInfo.LastWriteTime)
                                    .Count()>0)
                                    //.Where(c => c.SciezkaPliku.Replace(@"\", string.Empty) == fileInfo.FullName.Replace(@"\", string.Empty))

                {
                    listaBadanDoOdswiezenia.Add(plik);
                    licznikPlikowDoOdswiezenia++;
                }
                //if (listaBadanZBazy.Where(c => c.NazwaPliku == fileInfo.Name)
            }
        }

        private async Task PobierzListeBadanZBazyIUstawBrakStatusu()
        {
            listaBadanZBazy = await repository.PobierzListeBadan();
            foreach (var badanie in listaBadanZBazy)
            {
                badanie.StatusBadania = "";
            }
        }

        private void DodajNoweBadaniaZPlikowExcel()
        {
            foreach (var plik in listaBadanDoDodania)
            {

                var wynik = new tblWynikiBadanGeowloknin();
                FileInfo fileInfo = new FileInfo(plik);

                Task.Run(()=>wynik = plikZBadaniem.PobierzWynikiBadanZPlikuExcel(plik));

                    wynik.DataUtworzeniaPliku = fileInfo.CreationTime;
                    wynik.NazwaPliku = fileInfo.Name;
                    wynik.DataModyfikacjiPliku = fileInfo.LastWriteTime;
                    wynik.StatusBadania = "Dodano";

                    wynikiBadan.Add(wynik);
            }
        }
        private async Task UaktualnijBadaniaZPlikowExcel()
        {
            foreach (var plik in listaBadanDoOdswiezenia)
            {
                var wynik = await repository.PobierzWynikiBadaniaZBazyAsync(plik);
                    
                FileInfo fileInfo = new FileInfo(plik);

                await Task.Run(() => wynik = plikZBadaniem.PobierzWynikiBadanZPlikuExcel(plik));

                    wynik.DataUtworzeniaPliku = fileInfo.CreationTime;
                    wynik.NazwaPliku = fileInfo.Name;
                    wynik.DataModyfikacjiPliku = fileInfo.LastWriteTime;
                    wynik.StatusBadania = "Uaktualniono";

                await repository.UaktualnijWynikiBadanWBazieAsync();
            }
        }

    }
}
