using ControlzEx.Standard;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Vml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BarCodeGenerator;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.ScaleService;
using GAT_Produkcja.Utilities.ScaleService.Exceptions;
using GAT_Produkcja.Utilities.ZebraPrinter.Exceptions;
using GAT_Produkcja.Utilities.ZebraPrinter.S4M;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan.Exceptions;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Ewidencja;
using Microsoft.Office.Interop.Excel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj
{
    [AddINotifyPropertyChangedInterface]

    public class GPRuchTowarDodajViewModel : SaveCommandGenericViewModelBase
    {
        #region Fields
        private tblTowarGeowlokninaParametryGramatura wybranaGramatura = new tblTowarGeowlokninaParametryGramatura();
        private decimal towarRW_Szerokosc;
        private decimal towarRW_Dlugosc;
        private tblTowarGeowlokninaParametrySurowiec wybranySurowiec = new tblTowarGeowlokninaParametrySurowiec();
        private tblProdukcjaZlecenie zlecenieProdukcyjne;
        private tblProdukcjaRuchTowar towarRuch = new tblProdukcjaRuchTowar();
        private tblProdukcjaZlecenieCiecia zlecenieCiecia;
        private tblRuchStatus statusRuchu = new tblRuchStatus();
        private DodajUsunEdytujEnum dodajEdytujStatusEnum;
        private decimal gramaturaSrednia;
        private string wybranaStronaRolki;
        private string pelnyNrRolki;
        private readonly IGPRuchTowar_RolkaHelper rolkaHelper;
        private readonly IScaleReader scaleReader;
        private readonly IWeryfikacjaGramaturyGeowlokninHelper weryfikacjaGramaturyHelper;
        private readonly IZebraS4MService zebraS4MService;
        #endregion

        public override bool IsChanged => !RuchTowar.Compare(TowarRuchOrg);
        public override bool IsValid => RuchTowar.IsValid;


        #region Properties
        public tblProdukcjaRuchTowar RuchTowar
        {
            get => towarRuch;
            set
            {
                towarRuch = value;
                PrzypiszSurowiec();
                PrzypiszGramature();
            }
        }

        public DodajUsunEdytujEnum DodajEdytujStatusEnum
        {
            get => dodajEdytujStatusEnum;
            private set
            {
                dodajEdytujStatusEnum = value;
                RuchTowar.KodKreskowy = GenerujKodKreskowy();
            }
        }

        private tblProdukcjaRuchTowar rolkaRW;

        public tblRuchStatus StatusRuchu
        {
            get => statusRuchu; set
            {
                statusRuchu = value;
                Tytul = GenerujTytul(statusRuchu.IDRuchStatus);
            }
        }
        public IEnumerable<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; }

        public tblTowarGeowlokninaParametryGramatura WybranaGramatura
        {
            get => wybranaGramatura;
            set
            {
                wybranaGramatura = value;
                if (wybranaGramatura != null)
                {
                    RuchTowar.IDGramatura = wybranaGramatura.IDTowarGeowlokninaParametryGramatura;
                    RuchTowar.Gramatura = wybranaGramatura.Gramatura.GetValueOrDefault();
                    RuchTowar.tblTowarGeowlokninaParametryGramatura = WybranaGramatura;
                    Messenger.Send(new tblTowarGeowlokninaParametryGramatura
                    {
                        Gramatura = RuchTowar.Gramatura,
                        IDTowarGeowlokninaParametryGramatura = RuchTowar.IDGramatura
                    });
                }
            }
        }

        public IEnumerable<tblTowarGeowlokninaParametrySurowiec> ListaRodzajowSurowca { get; set; }
        public tblTowarGeowlokninaParametrySurowiec WybranySurowiec
        {
            get => wybranySurowiec;
            set
            {
                wybranySurowiec = value;
                if (wybranySurowiec != null)
                {
                    RuchTowar.IDTowarGeowlokninaParametrySurowiec = wybranySurowiec.IDTowarGeowlokninaParametrySurowiec;
                    RuchTowar.tblTowarGeowlokninaParametrySurowiec = wybranySurowiec;
                }
            }
        }
        public decimal GramaturaSrednia
        {
            get
            {
                return gramaturaSrednia;
            }

            set
            {
                gramaturaSrednia = value;
            }
        }
        public int? NrZlecenia { get; set; }

        public tblProdukcjaRuchTowar TowarRuchOrg { get; set; } = new tblProdukcjaRuchTowar();

        public string Tytul { get; set; }
        public int IloscEtykietDoDruku { get; set; } = 1;
        public string NazwaDrukarki { get; set; }

        public string Info { get; set; }

        public List<string> StronyRolki { get; set; } = new List<string> { "L", "P" };
        public string WybranaStronaRolki
        {
            get => wybranaStronaRolki; 
            set
            {
                wybranaStronaRolki = value;
                RuchTowar.NrRolkiPelny = pelnyNrRolki + wybranaStronaRolki;
            }
        }
        public bool CzyStronaRolkiWidoczna { get; set; }
        #endregion

        #region Commands
        public RelayCommand ScaleReadCommand { get; set; }
        public IGPRuchTowarBadaniaViewModel BadaniaViewModel { get; set; }
        public RelayCommand ZmianaWagiCommand { get; set; }
        public RelayCommand PrintCECommand { get; set; }
        public RelayCommand PrintCEUVCommand { get; set; }
        public RelayCommand PrintInternalLabelCommand { get; set; }
        public string SaveButtonToolTip { get; private set; }
        #endregion

        #region CTOR
        public GPRuchTowarDodajViewModel(IViewModelService viewModelService,
                                         IGPRuchTowar_RolkaHelper rolkaHelper,
                                         IScaleLP7510Reader scaleReader,
                                         IWeryfikacjaGramaturyGeowlokninHelper weryfikacjaGramaturyHelper,
                                         IGPRuchTowarBadaniaViewModel badaniaViewModel,
                                         IZebraS4MService zebraS4MService)
            : base(viewModelService)
        {
            this.rolkaHelper = rolkaHelper;
            this.scaleReader = scaleReader;
            this.weryfikacjaGramaturyHelper = weryfikacjaGramaturyHelper;
            BadaniaViewModel = badaniaViewModel;
            this.zebraS4MService = zebraS4MService;

            #region Scale
            ScaleReadCommand = new RelayCommand(ScaleReadCommandExecute);
            ZmianaWagiCommand = new RelayCommand(ZmianaWagiCommandExecute);
            #endregion

            #region Printer
            PrintCECommand = new RelayCommand(PrintCECommandExecute, PrintCECommandCanExecute);
            PrintCEUVCommand = new RelayCommand(PrintCEUVCommandExecute, PrintCEUVCommandCanExecute);
            PrintInternalLabelCommand = new RelayCommand(PrintInternalLabelCommandExecute, PrintInternalLabelCommandCanExecute);
            #endregion

            Messenger.Register<tblProdukcjaZlecenie>(this, GdyPrzeslanoZlecenieProdukcyjne); //NaPotrzebyRW
            Messenger.Register<tblProdukcjaZlecenieCiecia>(this, GdyPrzeslanoZlecenieCiecia);
            Messenger.Register<DodajEdytujGPRuchTowarMessage>(this, GdyPrzeslanoDodajEdytujMessage);
        }

        #region Print

        private async void PrintInternalLabelCommandExecute()
        {
            await PrintLabelAsync(RuchTowar, IloscEtykietDoDruku, TypEtykietEnum.Internal);
        }

        private bool PrintInternalLabelCommandCanExecute()
        {
            return SaveCommandCanExecute()
                && zebraS4MService.CanPrint();
        }

        private async void PrintCEUVCommandExecute()
        {
            await PrintLabelAsync(RuchTowar, IloscEtykietDoDruku, TypEtykietEnum.CEzUV);
        }

        private bool PrintCEUVCommandCanExecute()
        {
            return SaveCommandCanExecute()
                && RuchTowar.IDTowarGeowlokninaParametrySurowiec == (int)TowarGeowlokninaSurowiecEnum.PP
                && zebraS4MService.CanPrint();
        }

        private bool PrintCECommandCanExecute()
        {
            return SaveCommandCanExecute()
                && zebraS4MService.CanPrint();
        }

        private async void PrintCECommandExecute()
        {
            await PrintLabelAsync(RuchTowar, IloscEtykietDoDruku, TypEtykietEnum.CE);
        }

        private async Task PrintLabelAsync(tblProdukcjaRuchTowar towar, int iloscEtukiet, TypEtykietEnum typEtykietEnum)
        {
            try
            {
                if (typEtykietEnum == TypEtykietEnum.Internal)
                {
                    await zebraS4MService.PrintInternalLabelAsync(towar, iloscEtukiet);
                }
                else if (typEtykietEnum == TypEtykietEnum.CEzUV)
                {
                    await zebraS4MService.PrintCELabelAsync(towar, iloscEtukiet, true);
                }
                else
                {
                    await zebraS4MService.PrintCELabelAsync(towar, iloscEtukiet, false);
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }

        #endregion


        private void ZmianaWagiCommandExecute()
        {
            PrzeliczGramatureSrednia();
            SprawdzGramature();
        }


        private async void ScaleReadCommandExecute()
        {
            try
            {
                RuchTowar.Waga_kg = await scaleReader.GetWeight();
                //SprawdzGramature();
            }
            catch (Exception ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }
        }
        private void PrzeliczIloscM2()
        {
            RuchTowar.Ilosc_m2 = RuchTowar.Dlugosc_m * RuchTowar.Szerokosc_m;
        }
        private void PrzeliczGramatureSrednia()
        {
            if (RuchTowar.Ilosc_m2 == 0) return;
            GramaturaSrednia = RuchTowar.Waga_kg / RuchTowar.Ilosc_m2 * 1000;
        }

        private void SprawdzGramature()
        {
            if (GramaturaSrednia <= 0) return;
            try
            {
                if (weryfikacjaGramaturyHelper.CzyGramaturaZgodna(RuchTowar)) return;

                var gramaturaNowa = weryfikacjaGramaturyHelper.PobierzWlasciwaGramature(gramaturaSrednia, RuchTowar);
                if (DialogService.ShowQuestion_BoolResult("Zgodnie z parametrami rolki należy zmienić gramaturę. Czy wykonać?"))
                {
                    RuchTowar.IDGramatura = gramaturaNowa.IDTowarGeowlokninaParametryGramatura;
                    PrzypiszGramature();
                }
            }
            catch (GeowlokninaGramaturaException ex)
            {
                DialogService.ShowInfo_BtnOK(ex.Message);
            }
        }

        private void GdyPrzeslanoDodajEdytujMessage(DodajEdytujGPRuchTowarMessage obj)
        {
            if (obj is null || obj.RuchStatus is null || obj.RuchTowar is null) return;

            StatusRuchu = obj.RuchStatus;
            RuchTowar = obj.RuchTowar;
            DodajEdytujStatusEnum = obj.DodajUsunEdytujEnum;
            rolkaRW = obj.RolkaRW;

            UzupelnijDaneTowaru();
            DodajTytul();
            UkryjComboboxNaFormularzu();
        }

        private void UkryjComboboxNaFormularzu()
        {
            if (RuchTowar.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                CzyStronaRolkiWidoczna = true;
            else
                CzyStronaRolkiWidoczna = false;
        }

        private void DodajTytul()
        {
            if (DodajEdytujStatusEnum == DodajUsunEdytujEnum.Dodaj)
                if (StatusRuchu.IDRuchStatus == (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
                    Tytul = "Dodaj rolkę RW";
                else
                    Tytul = "Dodaj rolkę PW";


            if (DodajEdytujStatusEnum == DodajUsunEdytujEnum.Edytuj)
                if (StatusRuchu.IDRuchStatus == (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
                    Tytul = "Edytuj rolkę RW";
                else
                    Tytul = "Edytuj rolkę PW";
        }

        private void UzupelnijDaneTowaru()
        {
            RuchTowar.DataDodania = DateTime.Now;
            RuchTowar.KodKreskowy = GenerujKodKreskowy();
            pelnyNrRolki = RuchTowar.NrRolkiPelny;

            if (RuchTowar.IDProdukcjaGniazdoProdukcyjne != (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                RuchTowar.CzyKalandrowana = true;
        }

        private void GdyPrzeslanoZlecenieCiecia(tblProdukcjaZlecenieCiecia obj)
        {
            if (obj is null)
                return;

            zlecenieCiecia = obj;
            
            NrZlecenia = zlecenieCiecia.NrZleceniaCiecia;
        }

        private void GdyPrzeslanoZlecenieProdukcyjne(tblProdukcjaZlecenie obj)
        {
            zlecenieProdukcyjne = obj;
        }
        #endregion

        #region Messengers
        private string GenerujKodKreskowy()
        {
            if (StatusRuchu.IDRuchStatus == (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW)
                if (DodajEdytujStatusEnum == DodajUsunEdytujEnum.Dodaj)
                    return BarCodeGenerator.GetUniqueId();

            return RuchTowar.KodKreskowy;
        }

        private string GenerujTytul(int iDRuchStatus)
        {
            if (iDRuchStatus == (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
                return "Dodaj towar do rozchodu (RW)";

            return "Dodaj towar do przyhcodu (PW)";
        }

        private void PrzypiszGramature()
        {
            if (RuchTowar == null || RuchTowar.IDGramatura == 0)
                return;
            if (ListaGramatur == null || ListaGramatur.Count() == 0)
                return;

            WybranaGramatura = ListaGramatur.SingleOrDefault(g => g.IDTowarGeowlokninaParametryGramatura == RuchTowar.IDGramatura);
        }
        private void PrzypiszSurowiec()
        {
            if (RuchTowar is null || RuchTowar.IDTowarGeowlokninaParametrySurowiec == 0)
                return;

            if (ListaRodzajowSurowca == null || ListaRodzajowSurowca.Count() == 0)
                return;

            WybranySurowiec = ListaRodzajowSurowca.SingleOrDefault(g => g.IDTowarGeowlokninaParametrySurowiec == RuchTowar.IDTowarGeowlokninaParametrySurowiec);
        }


        #endregion

        #region Load
        protected override async void LoadCommandExecute()
        {
            ListaGramatur = await UnitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync();
            ListaRodzajowSurowca = await UnitOfWork.tblTowarGeowlokninaParametrySurowiec.GetAllAsync();

            GenerujWybraneElementy(RuchTowar);
            RuchTowar.KodKreskowy = GenerujKodKreskowy();

            try
            {
                await BadaniaViewModel.LoadAsync(RuchTowar?.IDProdukcjaRuchTowar);
                await weryfikacjaGramaturyHelper.LoadAsync();
                await zebraS4MService.LoadAsync();
                await scaleReader.LoadAsync();
            }
            catch (ScaleException ex)
            {
                Info = ex.Message;
            }
            catch (ZebraException ex)
            {
                if (Info == null)
                    Info = ex.Message;
                else
                    Info += $"; {ex.Message}";
            }
            catch (ArgumentOutOfRangeException ex)
            {
                DialogService.ShowError_BtnOK(ex.Message);
            }

            await PobierzNoweNrDlaRolki();

        }

        private async  Task PobierzNoweNrDlaRolki()
        {
            RuchTowar.NrRolki = await rolkaHelper.PobierzKolejnyNrRolkiAsync(RuchTowar.IDProdukcjaGniazdoProdukcyjne.GetValueOrDefault());
            RuchTowar.NrRolkiPelny = await rolkaHelper.PobierzKolejnyPelnyNrRolkiAsync(RuchTowar);
        }

        private void GenerujWybraneElementy(tblProdukcjaRuchTowar produkcjaRuchTowar)
        {
            WybranySurowiec = ListaRodzajowSurowca.SingleOrDefault(s => s.IDTowarGeowlokninaParametrySurowiec == RuchTowar.IDTowarGeowlokninaParametrySurowiec);
            WybranaGramatura = ListaGramatur.SingleOrDefault(g => g.IDTowarGeowlokninaParametryGramatura == RuchTowar.IDGramatura);

        }

        #endregion

        #region Delete
        protected override void DeleteCommandExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Save
        protected override void SaveCommandExecute()
        {
            RuchTowar.tblTowarGeowlokninaParametryGramatura = WybranaGramatura;
            RuchTowar.tblTowarGeowlokninaParametrySurowiec = WybranySurowiec;
            RuchTowar.DataDodania = DateTime.Now;
            RuchTowar.tblProdukcjaRuchTowarBadania.Add(BadaniaViewModel.Save(RuchTowar.IDProdukcjaRuchTowar));

            Messenger.Send(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajEdytujStatusEnum,
                RuchStatus = new tblRuchStatus { IDRuchStatus = StatusRuchu.IDRuchStatus },
                RuchTowar = RuchTowar
            });

            ViewService.Close(this.GetType().Name);
        }


        protected override bool SaveCommandCanExecute()
        {
            PrzeliczIloscM2();
            return RuchTowar.IsValid && IsChanged && CzyParametryRolkiZgodneZRolkaRW();
        }

        private bool CzyParametryRolkiZgodneZRolkaRW()
        {
            if(rolkaRW is null)
            {
                SaveButtonToolTip = "Zapisz pozycję.\r\n[CTRL + S]";
                return true;
            }

            if(rolkaRW.Dlugosc_m>=RuchTowar.Dlugosc_m 
                && rolkaRW.Szerokosc_m>=RuchTowar.Szerokosc_m)
            {
                SaveButtonToolTip = "Zapisz pozycję.\r\n[CTRL + S]";
                return true;
            }

            SaveButtonToolTip = "Szerokość lub długość większe od parametrów rolki RW.\r\nPopraw parametry aby zapisać.";
            return false;
        }

        #endregion

        public override void IsChanged_False()
        {
            TowarRuchOrg = RuchTowar.DeepClone();
        }
    }
}
