using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.Utilities.ExcelReportGenerator;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe
{
    public class PodsumowanieFinansoweViewModel : ListCommandViewModelBase
    {
        #region Fields

        private readonly IPodsumowanieFinansoweHelper podsumowanieHelper;
        private readonly IXlsServiceBuilder xlsServiceBuilder;
        private readonly IXlsService xlsService;

        #endregion

        #region Properties
        public DateTime DataOd { get; set; } = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        public DateTime DataDo { get; set; } = DateTime.Now.Date;
        public List<PodsFinans_ZamowienieOdKlientowModel> ZamowieniaOdKlientowPodsumowanieLista { get; set; }
        public PodsFinans_ZamowienieOdKlientowModel ZamowieniaOdKlientowPodsumowanie { get; set; }
        public List<PodsFinans_ProdukcjaModel> ProdukcjaPodsumowanieLista { get; set; }
        public PodsFinans_ProdukcjaModel ProdukcjaPodsumowanie { get; set; }
        public IEnumerable<PodsFinans_SprzedazAGGModel> SprzedazAGGPodsumowanieLista { get; set; }
        public PodsFinans_SprzedazAGGModel SprzedazAGGPodsumowanie { get; set; }
        public IEnumerable<PodsFinans_MagazynyModel> MagazynAGGPodsumowanieLista { get; set; }
        public IEnumerable<PodsFinans_MagazynyModel> MagazynGTXPodsumowanieLista { get; set; }
        public IEnumerable<PodsFinans_MagazynyModel> MagazynGTX2PodsumowanieLista { get; set; }
        public PodsFinans_MagazynyModel MagazynyPodsumowanie { get;  set; }
        public IEnumerable<PodsFinans_NaleznosciIZobowiazaniaModel> NaleznosciIZobowiazaniaPodsumowanieLista { get; set; }
        public PodsFinans_NaleznosciIZobowiazaniaModel NaleznosciIZobowiazaniaPodsumowanie { get; set; }
        public IEnumerable<PodsFinans_StanyKontModel> StanyKontPodsumowanieLista { get; set; }
        public decimal StanyKontPodsumowanie { get; set; }


        public decimal Bilans { get; set; }

        public string Tytul { get; set; }
        #endregion

        #region Commands
        public RelayCommand SzukajCommand { get; set; }
        public RelayCommand GenerujRaportXlsCommand { get; set; }
        #endregion

        public PodsumowanieFinansoweViewModel(IViewModelService viewModelService,
                                            IPodsumowanieFinansoweHelper podsumowanieHelper,
                                            IXlsServiceBuilder xlsServiceBuilder) : base(viewModelService)
        {
            this.podsumowanieHelper = podsumowanieHelper;
            this.xlsServiceBuilder = xlsServiceBuilder;

            SzukajCommand = new RelayCommand(SzukajCommandExecute, SzukajCommandCanExecute);
            GenerujRaportXlsCommand = new RelayCommand(GenerujRaportXlsCommandExecute);


            Tytul = "Podsumowanie w przedziale dat";
        }

        #region GenerujRaportXlsCommand

        private void GenerujRaportXlsCommandExecute()
        {
            try
            {
                string wsName = $"Raport_{DataDo.ToString("dd/MM/yyyy")}";
                xlsServiceBuilder
                    .CreateWorkbook()
                    .CreateWorksheet(wsName)
                    .AddListToSheet(new List<decimal> { Bilans }, "Bilans", wsName)
                    .AddListToSheet(ZamowieniaOdKlientowPodsumowanieLista, "Zamówienia od klientów", wsName)
                    .AddListToSheet(ProdukcjaPodsumowanieLista, "Produkcja", wsName)
                    .AddListToSheet(SprzedazAGGPodsumowanieLista, "Sprzedaż AGG", wsName)
                    .AddListToSheet(MagazynAGGPodsumowanieLista, "Magazyn AGG", wsName)
                    .AddListToSheet(MagazynGTXPodsumowanieLista, "Magazyn GTX", wsName)
                    .AddListToSheet(MagazynGTX2PodsumowanieLista, "Magazyn GTX2", wsName)
                    .AddListToSheet(NaleznosciIZobowiazaniaPodsumowanieLista, "Należności i zobowiązania", wsName, new List<string> { "NaleznosciDoDaty", "ZobowiazaniaDoDaty" })
                    .AddListToSheet(StanyKontPodsumowanieLista, "Stany Kont", wsName)
                    .Build();
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }

        #endregion

        #region SzukajCommand

        private async void SzukajCommandExecute()
        {
            await PodsumujWszystko();
        }

        private bool SzukajCommandCanExecute()
        {
            return podsumowanieHelper.PodsumowanieZamowieniaOdKlientowHelper.IsButtonActive
                && podsumowanieHelper.PodsumowanieSprzedazHelper.IsButtonActive
                && podsumowanieHelper.PodsumowanieProdukcjaHelper.IsButtonActive
                && podsumowanieHelper.PodsumowanieMagazynyHelper.IsButtonActive
                && podsumowanieHelper.PodsumowanieNaleznosciIZobowiazaniaHelper.IsButtonActive
                && podsumowanieHelper.PodsumowanieKontBankowychHelper.IsButtonActive
                ;
        }

        #endregion


        protected override async void LoadCommandExecute()
        {
            await podsumowanieHelper.PodsumowanieProdukcjaHelper.LoadAsync();
            await PodsumujWszystko();
        }

        private async Task PodsumujWszystko()
        {
            await PodsumowanieZlecenOdKlientow();
            await PodsumowanieProdukcji();
            await PodsumowanieSprzedazy();
            await PodsumowanieMagazynow();
            await PodsumowanieNaleznosciIZobowiazan();
            await PodsumowanieStanowKont();

            ObliczBilans();
        }

        private async Task PodsumowanieStanowKont()
        {
            StanyKontPodsumowanieLista = await podsumowanieHelper.PodsumowanieKontBankowychHelper.PobierzStanKontZDaty(DataDo);
            StanyKontPodsumowanie = podsumowanieHelper.PodsumowanieKontBankowychHelper.Podsumowanie;
        }

        private void ObliczBilans()
        {
            Bilans = 
                NaleznosciIZobowiazaniaPodsumowanie.NaleznosciAll - NaleznosciIZobowiazaniaPodsumowanie.ZobowiazaniaAll 
                + MagazynyPodsumowanie.Wartosc
                + StanyKontPodsumowanie;
        }

        #region ZamowieniaOdKlientow

        private async Task PodsumowanieZlecenOdKlientow()
        {
            //var listaZamowienOdKlientow = await UnitOfWork.vwZamOdKlientaAGG.WhereAsync(z => z.DataWyst >= DataOd
            //                                                                              && z.DataWyst <= DataDo);
            ZamowieniaOdKlientowPodsumowanieLista = await podsumowanieHelper
                                                        .PodsumowanieZamowieniaOdKlientowHelper
                                                            .PodsumujZamowieniaOdKlientow(DataOd, DataDo);
            ZamowieniaOdKlientowPodsumowanie = podsumowanieHelper.PodsumowanieZamowieniaOdKlientowHelper.Podsumowanie;
        }

        #endregion

        #region Produkcja
        private async Task PodsumowanieProdukcji()
        {
            ProdukcjaPodsumowanieLista = await podsumowanieHelper.PodsumowanieProdukcjaHelper
                                                            .PobierzPodsumowanieProdukcjiWDatach(DataOd, DataDo);
            ProdukcjaPodsumowanie = podsumowanieHelper.PodsumowanieProdukcjaHelper.Podsumowanie;
        }
        #endregion

        #region Sprzedaz
        private async Task PodsumowanieSprzedazy()
        {
            SprzedazAGGPodsumowanieLista = await podsumowanieHelper.PodsumowanieSprzedazHelper
                                                             .PobierzSprzedazAGGWDatach(DataOd, DataDo);
            SprzedazAGGPodsumowanie = podsumowanieHelper.PodsumowanieSprzedazHelper.Podsumowanie;
        }
        #endregion

        #region Magazyny
        private async Task PodsumowanieMagazynow()
        {
            MagazynAGGPodsumowanieLista = await podsumowanieHelper.PodsumowanieMagazynyHelper.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchAGG(), DataDo);
            MagazynGTXPodsumowanieLista = await podsumowanieHelper.PodsumowanieMagazynyHelper.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX(), DataDo);
            MagazynGTX2PodsumowanieLista = await podsumowanieHelper.PodsumowanieMagazynyHelper.PobierzPodsumowanieMagazynuDoDaty(new vwMagazynRuchGTX2(), DataDo);

            MagazynyPodsumowanie = podsumowanieHelper.PodsumowanieMagazynyHelper.Podsumowanie;
        }
        #endregion

        #region NaleznosciIZobowiazania
        private async Task PodsumowanieNaleznosciIZobowiazan()
        {
            NaleznosciIZobowiazaniaPodsumowanieLista = await podsumowanieHelper.PodsumowanieNaleznosciIZobowiazaniaHelper.PobierzPodsumowanieNalzenosciIZobowiazan(DataDo);
            NaleznosciIZobowiazaniaPodsumowanie = podsumowanieHelper.PodsumowanieNaleznosciIZobowiazaniaHelper.Podsumowanie;
        }
        #endregion
    }
}
