using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja
{
    public class MagazynEwidencjaSubiektUCViewModel : ListMethodViewModelBase, IMagazynEwidencjaSubiektUCViewModel
    {

        #region Fields
        private IEnumerable<vwMagazynAGG> listaMagazynowAGG;
        private IEnumerable<vwMagazynGTX> listaMagazynowGTX;
        private IEnumerable<vwMagazynGTX2> listaMagazynowGTX2;
        private IEnumerable<IMagazynRuchSubiekt> listaTowarowNaMagazynieAGG;
        private IEnumerable<IMagazynRuchSubiekt> listaTowarowNaMagazynieGTX;
        private string wybranaFirma;
        private IMagazynSubiekt wybranyMagazyn;
        private IEnumerable<IMagazynRuchSubiekt> listaTowarowNaMagazynachWObuFirmach;
        private IEnumerable<vwMagazynRuchGTX2> listaTowarowNaMagazynieGTX2;


        #endregion

        #region Properties
        public IEnumerable<IMagazynRuchSubiekt> ListaTowarowNaMagazynie { get; set; }
        public IEnumerable<IMagazynSubiekt> ListaMagazynow { get; set; }
        public IMagazynSubiekt WybranyMagazyn
        {
            get => wybranyMagazyn;
            set
            {
                wybranyMagazyn = value;
            }
        }

        private void PodsumujTowary()
        {
            Wartosc = ListaTowarowNaMagazynie.Sum(s => s.Wartosc);
            Ilosc = ListaTowarowNaMagazynie.Sum(s => s.Pozostalo);
        }


        public List<string> ListaFirm { get; set; } = new List<string> { "AG Geosynthetics", "GTEX" };
        public string WybranaFirma
        {
            get => wybranaFirma;
            set
            {
                wybranaFirma = value;
                if (WybranaFirma == "AG Geosynthetics")
                    ListaMagazynow = listaMagazynowAGG;
                else if (WybranaFirma == "GTEX")
                    ListaMagazynow = listaMagazynowGTX;
                else if (WybranaFirma == "GTEX2")
                    ListaMagazynow = listaMagazynowGTX2;
                else
                    ListaMagazynow = null;
            }
        }
        public string NazwaTowaru { get; set; }
        public decimal Wartosc { get; set; }
        public decimal Ilosc { get; set; }
        #endregion

        #region Commands
        public RelayCommand SzukajCommand { get; set; }
        #endregion

        public MagazynEwidencjaSubiektUCViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            SzukajCommand = new RelayCommand(SzukajCommandExecute);

        }

        private void SzukajCommandExecute()
        {
            ListaTowarowNaMagazynie = listaTowarowNaMagazynachWObuFirmach;

            if (WybranaFirma != null)
                ListaTowarowNaMagazynie = ListaTowarowNaMagazynie.Where(t => t.Firma == WybranaFirma).ToList();

            if (NazwaTowaru != null)
                ListaTowarowNaMagazynie = ListaTowarowNaMagazynie.Where(t => t.TowarNazwa.ToLower().Contains(NazwaTowaru.ToLower())).ToList();

            if (WybranyMagazyn != null)
                DekorujListeTowarowNaMagazynie();

            PodsumujTowary();
        }
        private void DekorujListeTowarowNaMagazynie()
        {
            var magAGG = WybranyMagazyn as vwMagazynAGG;
            if (magAGG != null)
                ListaTowarowNaMagazynie = new GrupowanieTowarowDecorator(ListaTowarowNaMagazynie
                                                                    .Where(t => t.IdMagazyn == WybranyMagazyn.IdMagazyn)
                                                                    .ToList()
                                                                    )
                                                .Grupuj();
            else
                ListaTowarowNaMagazynie = new GrupowanieTowarowDecorator (ListaTowarowNaMagazynie
                                                                    .Where(t => t.IdMagazyn == WybranyMagazyn.IdMagazyn)
                                                                    .ToList()
                                                                    )
                                                .Grupuj();
        }

        public override async Task LoadAsync(int? id)
        {
            listaMagazynowAGG = await UnitOfWork.vwMagazynAGG.GetAllAsync();
            listaMagazynowGTX = await UnitOfWork.vwMagazynGTX.GetAllAsync();
            listaMagazynowGTX2 = await UnitOfWork.vwMagazynGTX2.GetAllAsync();

            listaTowarowNaMagazynieAGG = await UnitOfWork.vwMagazynRuchAGG.WhereAsync(m => m.Wartosc != 0);
            listaTowarowNaMagazynieAGG.ToList().ForEach(f => f.Firma = "AG Geosynthetics");

            listaTowarowNaMagazynieGTX = await UnitOfWork.vwMagazynRuchGTX.WhereAsync(m => m.Wartosc != 0);
            listaTowarowNaMagazynieGTX.ToList().ForEach(f => f.Firma = "GTEX");

            listaTowarowNaMagazynieGTX2 = await UnitOfWork.vwMagazynRuchGTX2.WhereAsync(m => m.Wartosc != 0);
            listaTowarowNaMagazynieGTX2.ToList().ForEach(f => f.Firma = "GTEX2");

            listaTowarowNaMagazynachWObuFirmach = listaTowarowNaMagazynieAGG
                                                                .Union(listaTowarowNaMagazynieGTX)
                                                                .Union(listaTowarowNaMagazynieGTX2)
                                                                ;
            ListaFirm = new List<string> { "AG Geosynthetics", "GTEX", "GTEX2" };
        }
    }
}
