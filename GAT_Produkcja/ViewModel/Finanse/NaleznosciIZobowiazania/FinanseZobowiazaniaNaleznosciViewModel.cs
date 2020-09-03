using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse
{
    public class FinanseZobowiazaniaNaleznosciViewModel : ListCommandViewModelBase
    {
        #region Fields
        private IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazanAGG;
        private IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazanGTX;
        private IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazanGTX2;
        private IEnumerable<IFinanseNaleznosciZobowiazania> listaZobowiazanINaleznosciAll;
        #endregion

        #region Porperties
        public IEnumerable<IFinanseNaleznosciZobowiazania> ListaNaleznosci { get; set; }
        public IEnumerable<IFinanseNaleznosciZobowiazania> ListaZobowiazan { get; set; }


        public IEnumerable<tblFirma> ListaFirm { get; set; }
        public tblFirma WybranaFirma { get; set; }
        public DateTime DataWymagalnosci { get; set; } = DateTime.Now.Date;

        public string Kontrahent { get; set; }

        public string Tytul { get; set; }

        public decimal NaleznosciSuma { get; set; }
        public decimal ZobowiazaniaSuma { get; set; }
        #endregion

        #region Command
        public RelayCommand SzukajCommand { get; set; }
        #endregion

        public FinanseZobowiazaniaNaleznosciViewModel(IViewModelService viewModelService) 
            : base(viewModelService)
        {

            SzukajCommand = new RelayCommand(SzukajCommandExecute);
            Tytul = "Należności i zobowiązania";
        }

        private void SzukajCommandExecute()
        {
            var listaZobowiazanINaleznosciFiltrowana = listaZobowiazanINaleznosciAll
                    .Where(d => d.TerminPlatnosci <= DataWymagalnosci);
            
            if (WybranaFirma != null)
            {
                listaZobowiazanINaleznosciFiltrowana = listaZobowiazanINaleznosciAll
                    .Where(f => f.IdFirma == WybranaFirma.IDFirma);
            }
            if(Kontrahent != null)
            {
                listaZobowiazanINaleznosciFiltrowana = listaZobowiazanINaleznosciAll
                        .Where(e=>e.Kontrahent.ToLower().Contains(Kontrahent.ToLower()));

            }

            SeparujListeNaleznosciIZobowiazan(listaZobowiazanINaleznosciFiltrowana);

            Podsumuj();
        }

        private void Podsumuj()
        {
            NaleznosciSuma = ListaNaleznosci.Sum(s => s.Naleznosc.GetValueOrDefault());
            ZobowiazaniaSuma = ListaZobowiazan.Sum(s => s.Zobowiazanie.GetValueOrDefault());
        }

        protected override async void LoadCommandExecute()
        {
            ListaFirm = await UnitOfWork.tblFirma.GetAllAsync();
            ListaFirm = ListaFirm.Where(f => f.Nazwa.ToLower().Contains("gtex")
                                          || f.Nazwa.ToLower().Contains("geosynthetics"));

            listaNaleznosciIZobowiazanAGG = await UnitOfWork.vwFinanseNalZobAGG.GetAllAsync();
            listaNaleznosciIZobowiazanAGG.ToList().ForEach(f => f.IdFirma = (int)FirmaEnum.AGG);
            listaNaleznosciIZobowiazanGTX = await UnitOfWork.vwFinanseNalZobGTX.GetAllAsync();
            listaNaleznosciIZobowiazanGTX.ToList().ForEach(f => f.IdFirma = (int)FirmaEnum.GTEX);
            listaNaleznosciIZobowiazanGTX2 = await UnitOfWork.vwFinanseNalZobGTX2.GetAllAsync();
            listaNaleznosciIZobowiazanGTX2.ToList().ForEach(f => f.IdFirma = (int)FirmaEnum.GTEX2);

            listaZobowiazanINaleznosciAll = listaNaleznosciIZobowiazanAGG
                                                .Union(listaNaleznosciIZobowiazanGTX)
                                                .Union(listaNaleznosciIZobowiazanGTX2);                                                ;

            var l = listaZobowiazanINaleznosciAll.Count();
            SeparujListeNaleznosciIZobowiazan(listaZobowiazanINaleznosciAll);

            Podsumuj();
        }
        /// <summary>
        /// Rozdziela liste ogolna na <see cref=" ListaNaleznosci"/> oraz <see cref="ListaZobowiazan"/>
        /// </summary>
        private void SeparujListeNaleznosciIZobowiazan(IEnumerable<IFinanseNaleznosciZobowiazania> listaNalzenosciIZobowiazan)
        {
            ListaNaleznosci = listaNalzenosciIZobowiazan
                .Where(n => n.Naleznosc > 0)
                .Where(z => z.TerminPlatnosci <= DataWymagalnosci);

            ListaZobowiazan = listaNalzenosciIZobowiazan
                .Where(n => n.Zobowiazanie > 0)
                .Where(z => z.TerminPlatnosci <= DataWymagalnosci);
        }
    }
}
