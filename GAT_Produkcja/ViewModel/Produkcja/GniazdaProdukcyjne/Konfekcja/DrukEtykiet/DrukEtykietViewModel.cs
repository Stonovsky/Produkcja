using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.ZebraPrinter;
using PropertyChanged;
using System.Windows.Input;
using GAT_Produkcja.Utilities.BarCodeGenerator;
using GalaSoft.MvvmLight.Threading;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Helpers.Geowloknina;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Konfekcja.DrukEtykiet
{
    [AddINotifyPropertyChangedInterface]

    public class DrukEtykietViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IZebraLabelPrinter zebraLabelPrinter;
        private readonly IGeowlokninaHelper geowlokninaHelper;
        private readonly IMessenger messenger;
        private readonly ZebraLabelGenerator _zebraLabelGenerator;

        #region Properties

        public IEnumerable<tblTowarGeowlokninaParametryRodzaj> ListaRodzajowSurowca { get; set; }
        public tblTowarGeowlokninaParametryRodzaj WybranyRodzajSurowca { get; set; }
        public IEnumerable<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; }
        public tblTowarGeowlokninaParametryGramatura WybranaGramatura { get; set; }
        public List<tblTowarGeowlokninaParametry> ListaParametrow { get; set; }

        public string KodKreskowy { get; set; }
        public string PrinterName { get; set; }
        public LabelModel LabelModel { get; set; }
        public tblRuchNaglowek RuchNaglowek { get; private set; }
        public tblRuchTowar RuchTowaruModel { get; set; }
        #endregion

        #region Commands
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand DrukujCommand { get; set; }
        public RelayCommand GenerujKodKreskowyCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand PoZmianieIlosciCommand { get; set; }
        public RelayCommand ResetujCommand { get; set; }
        public tblRuchTowarGeowlokninaParametry RuchTowarGeowlokninaParametryModel { get; private set; }
        #endregion


        #region CTOR

        public DrukEtykietViewModel(IDialogService dialogService,
                                    IViewService viewService,
                                    IUnitOfWork unitOfWork,
                                    IZebraLabelPrinter zebraLabelPrinter,
                                    IGeowlokninaHelper geowlokninaHelper,
                                    IMessenger messenger
                                    )
        {
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.unitOfWork = unitOfWork;
            this.zebraLabelPrinter = zebraLabelPrinter;
            this.geowlokninaHelper = geowlokninaHelper;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecuteAsync);
            DrukujCommand = new RelayCommand(DrukujCommandExecuteAsync, DrukujCommandCanExecute);
            GenerujKodKreskowyCommand = new RelayCommand(GenerujKodKreskowyCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            PoZmianieIlosciCommand = new RelayCommand(PoZmianieIlosciCommandExecute);
            ResetujCommand = new RelayCommand(ResetujCommandExecute);

            StworzNowyLabelModel();
        }

        private void StworzNowyLabelModel()
        {
            LabelModel = new LabelModel
            {
                Kalandrowana = true,
                IloscEtykietNaJednaSztuke = 1,
                Ilosc = 1,
                IloscEtykietDoDruku = 1,
                GniazdoProdukcyjne = GniazdaProdukcyjneEnum.LiniaDoKonfekcji
            };
        }

        private void ResetujCommandExecute()
        {
            StworzNowyLabelModel();
            RuchTowaruModel = new tblRuchTowar();
        }

        private void PoZmianieIlosciCommandExecute()
        {
            //LabelModel.SumarycznaIloscEtykiet = LabelModel.Ilosc * LabelModel.IloscEtykietNaJednaSztuke;
        }

        private void GenerujKodKreskowyCommandExecute()
        {
            LabelModel.KodKreskowy = BarCodeGenerator.GetUniqueId();
            RaisePropertyChanged(nameof(LabelModel));
        }

        #endregion

        #region Commands
        #region ZapiszCommand
        private bool ZapiszCommandCanExecute()
        {
            if (!LabelModel.IsValid)
                return false;

            return true;
        }
        private async void ZapiszCommandExecute()
        {
            bool czyDodajemyTowar = false;

            if (RuchTowaruModel == null ||
                RuchTowaruModel.IDRuchTowar == 0)
            {
                czyDodajemyTowar = true;

                RuchNaglowek = StworzRuchNaglowekModel();
                unitOfWork.tblRuchNaglowek.Add(RuchNaglowek);
                await unitOfWork.SaveAsync();

                RuchTowaruModel = await StworzRuchTowarModel();
                unitOfWork.tblRuchTowar.Add(RuchTowaruModel);
                await unitOfWork.SaveAsync();

                RuchTowarGeowlokninaParametryModel = StworzRuchTowarGeowlokninaParametryModel();
                unitOfWork.tblRuchTowarGeowlokninaParametry.Add(RuchTowarGeowlokninaParametryModel);
            }

            await unitOfWork.SaveAsync();
            WyswietlWiadomosc(czyDodajemyTowar);
            messenger.Send(RuchTowaruModel);
        }


        private void WyswietlWiadomosc(bool czyDodajemyTowar)
        {
            if (czyDodajemyTowar)
                dialogService.ShowInfo_BtnOK("Pozycja została dodana do bazy danych.");
            else
                dialogService.ShowInfo_BtnOK("Zaktualizowano pozycję w bazie danych.");
        }

        private tblRuchNaglowek StworzRuchNaglowekModel()
        {
            return new tblRuchNaglowek
            {
                IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,
                IDMagazynDo = (int)MagazynyEnum.WyrobGotowy_GTEX_WG,
                IDProdukcjaGniazdaProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ID_PracownikGAT=UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT,
                DataPrzyjecia = DateTime.Now,
                NrDokumentuPelny = "Konfekcja"
            };
        }
        private async Task<tblRuchTowar> StworzRuchTowarModel()
        {
            var listaTowarow = await unitOfWork.tblTowar.WhereAsync(t => t.Nazwa.Contains(WybranaGramatura.Gramatura.ToString()) &&
                                                                         t.Nazwa.Contains(WybranyRodzajSurowca.Rodzaj));
            var towar = listaTowarow.First();
            var magazyn = await unitOfWork.tblMagazyn.SingleOrDefaultAsync(m => m.Nazwa.ToLower().Contains("gotowego") &&
                                                                                 m.tblFirma.Nazwa.ToLower().Contains("EMG"));
            var jm = await unitOfWork.tblJm.SingleOrDefaultAsync(t => t.Jm == "m2");
            var dokumentTyp = await unitOfWork.tblDokumentTyp.SingleOrDefaultAsync(d => d.DokumentTypSkrot.Contains("PW"));
            var ruchTowarModel = new tblRuchTowar()
            {

                IDTowar = towar.IDTowar,
                TowarNazwa = towar.Nazwa,
                IDMagazyn = magazyn.IDMagazyn,
                IDJm = jm.IDJm,
                IDDokumentTyp = dokumentTyp.IDDokumentTyp,
                IDRuchNaglowek = RuchNaglowek.IDRuchNaglowek,
                //IDProdukcjaGniazdaProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                Uwagi = LabelModel.Uwagi,
                Ilosc = LabelModel.SzerokoscRolki * LabelModel.DlugoscNawoju * LabelModel.Ilosc,
                NrParti = LabelModel.KodKreskowy,
                IsValid = true
            };

            return ruchTowarModel;
        }
        private tblRuchTowarGeowlokninaParametry StworzRuchTowarGeowlokninaParametryModel()
        {
            decimal gramatura = (decimal)ListaGramatur
                                    .Where(g => g.IDTowarGeowlokninaParametryGramatura == LabelModel.IDTowarGeowlokninaParametryGramatura)
                                    .Select(s => s.Gramatura)
                                    .SingleOrDefault();

            return new tblRuchTowarGeowlokninaParametry
            {
                
                IDRuchTowar = RuchTowaruModel.IDRuchTowar,
                IDTowarGeowlokninaParametryGramatura = LabelModel.IDTowarGeowlokninaParametryGramatura,
                IDTowarGeowlokninaParametryRodzaj = LabelModel.IDTowarGeowlokninaParametryRodzaj,
                Dlugosc = LabelModel.DlugoscNawoju,
                Szerokosc = LabelModel.SzerokoscRolki,
                Waga = LabelModel.DlugoscNawoju * LabelModel.SzerokoscRolki * (gramatura/1000)
            };
        }

        #endregion

        private async void ZaladujWartosciPoczatkoweCommandExecuteAsync()
        {
            ListaRodzajowSurowca = await unitOfWork.tblTowarGeowlokninaParametryRodzaj.WhereAsync(t => t.Rodzaj == "ALTEX AT PP" || t.Rodzaj == "ALTEX AT PES");
            ListaGramatur = await unitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync();
            PrinterName = zebraLabelPrinter.GetPrinterName();

            SprawdzDrukarke();
        }

        private void SprawdzDrukarke()
        {
            if (PrinterName == null)
            {
                dialogService.ShowError_BtnOK("Brak podłączonej drukarki." +
                                                "\r\nPodłącz drukarkę, w przeciwnym razie wydruk będzie niemożliwy");
            }
        }

        private bool DrukujCommandCanExecute()
        {
            if (string.IsNullOrWhiteSpace(PrinterName))
                return false;

            if (!LabelModel.IsValid)
                return false;

            return true;
        }


        private async void DrukujCommandExecuteAsync()
        {
            LabelModel.RodzajSurowca = WybranyRodzajSurowca.RodzajSkrot;
            LabelModel.Gramatura = WybranaGramatura.Gramatura;
            tblTowar towar = await geowlokninaHelper.PobierzTowarZGramaturyIRodzajuSurowca(WybranaGramatura.Gramatura.GetValueOrDefault(), WybranyRodzajSurowca.RodzajSkrot);
            tblTowarGeowlokninaParametry geowlokninaParametry = await unitOfWork.tblTowarGeowlokninaParametry.GetByIdIncludeAllTablesAsync(towar.IDTowar);

            await zebraLabelPrinter.PrintLabelCE(LabelModel,geowlokninaParametry);
        }
        #endregion

    }
}
