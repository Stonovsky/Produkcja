using DocumentFormat.OpenXml.Drawing;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek.States;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek
{
    public class GPRuchNaglowekViewModel
        : AddEditCommandGenericViewModelBase<tblProdukcjaRuchNaglowek>, IGPRuchNaglowekViewModel
    {
        #region Fields

        private readonly IGPRuchTowar_Naglowek_Helper naglowekHelper;
        private readonly IGPRuchNaglowekStateFactory naglowekStateFactory;
        private IGPRuchNaglowekState naglowekState;
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;
        private tblProdukcjaRuchNaglowek naglowekPrzeslany;
        private tblProdukcjaGniazdoProdukcyjne wybraneGniazdo;
        private tblPracownikGAT wybranyPracownik_1;

        #endregion

        #region Properties

        public IEnumerable<tblPracownikGAT> ListaPracownikow { get; set; }
        public tblPracownikGAT WybranyPracownik_1
        {
            get => wybranyPracownik_1; 
            set
            {
                wybranyPracownik_1 = value;
                Messenger.Send<tblPracownikGAT, GPRuchTowarPWViewModel>(WybranyPracownik_1);
            }
        }
        public tblPracownikGAT WybranyPracownik_2 { get; set; }

        public IEnumerable<tblProdukcjaGniazdoProdukcyjne> ListaGniazdProdukcyjnych { get; set; }
        public tblProdukcjaGniazdoProdukcyjne WybraneGniazdo
        {
            get => wybraneGniazdo;
            set
            {
                wybraneGniazdo = value;
                naglowekState = naglowekStateFactory.GetState(this);
                Messenger.Send(wybraneGniazdo);
                UpdateProperties();
                AktywujPrzyciskWyboruZlecenia();
            }
        }


        public tblProdukcjaZlecenieTowar ZlecenieTowar { get; private set; }


        public IGPRuchTowarRWViewModel RuchTowarRWViewModel { get; }
        public IGPRuchTowarPWViewModel RuchTowarPWViewModel { get; }

        public bool CzyZlecProdMaBycWidoczne => naglowekState.CzyZlecProdMaBycWidoczne;
        public bool CzyZlecCieciaMaBycWidoczne => naglowekState.CzyZlecCieciaMaBycWidoczne;
        public bool RwEnabled => naglowekState.RwEnabled;
        public override bool IsChanged => base.IsChanged || naglowekState.IsChanged;
        public override bool IsValid => base.IsValid && naglowekState.IsValid;
        public override IGenericRepository<tblProdukcjaRuchNaglowek> Repository => UnitOfWork.tblProdukcjaRuchNaglowek;
        public bool AutoSave { get; set; } = true;
        public bool CzyDodajZlecenieButtonAktywny { get; set; }
        #endregion

        #region Commands
        public RelayCommand DodajZlecenieCieciaCommand { get; set; }
        public RelayCommand DodajZlecenieProdukcyjneCommand { get; set; }
        public RelayCommand AddEditOnPWCommand { get; set; }
        #endregion

        #region CTOR
        public GPRuchNaglowekViewModel(IViewModelService viewModelService,
                                       IGPRuchTowarRWViewModel ruchTowarRWViewModel,
                                       IGPRuchTowarPWViewModel ruchTowarPWViewModel,
                                       IGPRuchTowar_Naglowek_Helper naglowekHelper,
                                       IGPRuchNaglowekStateFactory naglowekStateFactory)
            : base(viewModelService)
        {
            this.naglowekHelper = naglowekHelper;
            this.naglowekStateFactory = naglowekStateFactory;

            RuchTowarRWViewModel = ruchTowarRWViewModel;
            RuchTowarPWViewModel = ruchTowarPWViewModel;

            naglowekState = this.naglowekStateFactory.GetState(this);

            DodajZlecenieCieciaCommand = new RelayCommand(DodajZlecenieCieciaCommandExecute);
            DodajZlecenieProdukcyjneCommand = new RelayCommand(DodajZlecenieProdukcyjneCommandExecute);
            AddEditOnPWCommand = new RelayCommand(AddEditOnPWCommandExecute, AddEditOnPWCommandCanExecute);


            IsChanged_False();
        }

        private void AddEditOnPWCommandExecute()
        {
            RuchTowarPWViewModel.AddCommand.Execute(null);
        }

        private bool AddEditOnPWCommandCanExecute()
        {
            return RuchTowarPWViewModel.AddCommand.CanExecute(null);
        }

        protected override void RegisterMessengers()
        {
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdoProdukcyjne);
            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowarAsync);

            //Na potrzeby AutoSave - message wysylany jest z PW
            Messenger.Register<GPSaveMessage>(this, GdyPrzeslanoSaveMessage);
            base.RegisterMessengers();
        }


        #endregion

        #region UpdateProperties
        private void UpdateProperties()
        {
            RaisePropertyChanged(nameof(RwEnabled));
            RaisePropertyChanged(nameof(CzyZlecCieciaMaBycWidoczne));
            RaisePropertyChanged(nameof(CzyZlecProdMaBycWidoczne));
        }

        #endregion

        #region CommandsExecute

        private void DodajZlecenieProdukcyjneCommandExecute()
        {
            ViewService.Show<ZlecenieProdukcyjneEwidencjaViewModel>();
            Messenger.Send<tblProdukcjaGniazdoProdukcyjne, ZlecenieProdukcyjneEwidencjaViewModel>(WybraneGniazdo);
            Messenger.Send<ListViewModelStatesEnum, ZlecenieProdukcyjneEwidencjaViewModel>(ListViewModelStatesEnum.Select);
        }

        private void DodajZlecenieCieciaCommandExecute()
        {
            ViewService.Show<ZlecenieCieciaEwidencjaViewModel>();
        }

        #endregion

        #region MessengersSent

        private void GdyPrzeslanoGniazdoProdukcyjne(tblProdukcjaGniazdoProdukcyjne obj)
        {
            gniazdoProdukcyjne = obj;
        }

        private async void GdyPrzeslanoZlecenieTowarAsync(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            ZlecenieTowar = obj;

            var rolkiPwDlaZleceniaTowaru = await UnitOfWork.tblProdukcjaRuchTowar.WhereAsync(t => t.IDProdukcjaZlecenieTowar == ZlecenieTowar.IDProdukcjaZlecenieTowar
                                                                                               && t.IDRuchStatus == (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW);

            //var naglowek = await UnitOfWork.tblProdukcjaRuchNaglowek.SingleOrDefaultAsync(n => n.IDProdukcjaZlecenieTowar == ZlecenieTowar.IDProdukcjaZlecenieTowar);
            //{
            //    GenerujNazweTowaruDlaZlecenia(obj);

            //    VMEntity.IDProdukcjaZlecenieTowar = obj.IDProdukcjaZlecenieTowar;
            //}
            //else
            //{
            //    VMEntity.IDProdukcjaZlecenieTowar = obj.IDProdukcjaZlecenieTowar;

            //    WyslijWiadomoscDoNaglowka(rolkiPwDlaZleceniaTowaru);
            //    //WyslijWiadomoscDoNaglowka(naglowek);
            //    await LoadAsync();
            //}

            GenerujNazweTowaruDlaZlecenia(obj);

            VMEntity.IDProdukcjaZlecenieTowar = obj.IDProdukcjaZlecenieTowar;
            if (rolkiPwDlaZleceniaTowaru != null && rolkiPwDlaZleceniaTowaru.Count() > 0)
                WyslijWiadomoscDoNaglowka(rolkiPwDlaZleceniaTowaru);
            //WyslijWiadomoscDoNaglowka(naglowek);
            await LoadAsync();

            PrzypiszWlasciweGniazdoProdukcyjne();
        }

        private void PrzypiszWlasciweGniazdoProdukcyjne()
        {
            VMEntity.IDProdukcjaGniazdoProdukcyjne = gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne;
            WybraneGniazdo = ListaGniazdProdukcyjnych.SingleOrDefault(g => g.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
        }

        /// <summary>
        /// Wysyla wiadomosc do biezacej klasy celem podczytania wszystkich danych istniejacych w bazie dla danego towaru ze zlecenia
        /// </summary>
        /// <param name="rolkiPwDlaZlecenia"></param>
        private void WyslijWiadomoscDoNaglowka(IEnumerable<tblProdukcjaRuchTowar> rolkiPwDlaZlecenia)
        {
            Messenger.Send(rolkiPwDlaZlecenia.First().tblProdukcjaRuchNaglowek);
        }

        private void WyslijWiadomoscDoNaglowka(tblProdukcjaRuchNaglowek naglowek)
        {
            Messenger.Send(naglowek);
        }
        private void GenerujNazweTowaruDlaZlecenia(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;
            ZlecenieTowar.TowarNazwa = new NazwaTowaruGenerator().GenerujNazweTowaru(obj);
        }


        #endregion

        #region LoadCommand
        public override Func<tblProdukcjaRuchNaglowek, int> GetIdFromEntityWhenSentByMessenger => (entitySent) =>
        {
            if (entitySent != null)
                return entitySent.IDProdukcjaRuchNaglowek;
            else
                return 0;
        };

        public override Func<Task> LoadAdditionally => async () =>
        {
            ListaPracownikow = await UnitOfWork.tblPracownikGAT.WhereAsync(e => e.CzyPracuje == true
                                                                          && (e.ID_Dostep == (int)TypDostepuEnum.Operator
                                                                           || e.ID_Dostep == (int)TypDostepuEnum.Kierownik));

            ListaGniazdProdukcyjnych = await UnitOfWork.tblProdukcjaGniazdoProdukcyjne.GetAllAsync();
            WybierzGniazdo();
            WybranyPracownik_1 = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT);

            await GdyPrzeslanoTowarMessage();
            await ZaladujZalezneViewModele();

            AktywujPrzyciskWyboruZlecenia();

        };

        private async Task GdyPrzeslanoTowarMessage()
        {
            if (elementSent is null) return;

            PrzypiszPracownikow();
            await PobierzZlecenieTowar();
            GenerujNazweTowaruDlaZlecenia(ZlecenieTowar);
        }

        private void PrzypiszPracownikow()
        {
            WybranyPracownik_1 = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == elementSent.IDPracownikGAT);
            WybranyPracownik_2 = ListaPracownikow.SingleOrDefault(p => p.ID_PracownikGAT == elementSent.IDPracownikGAT1);
        }

        private async Task PobierzZlecenieTowar()
        {
            if (elementSent.IDProdukcjaZlecenieTowar != null)
                ZlecenieTowar = await UnitOfWork.tblProdukcjaZlecenieTowar.GetByIdAsync(elementSent.IDProdukcjaZlecenieTowar.Value);

            WybierzGniazdo();
        }

        private void AktywujPrzyciskWyboruZlecenia()
        {
            if (WybraneGniazdo != null)
                CzyDodajZlecenieButtonAktywny = true;
            else
                CzyDodajZlecenieButtonAktywny = false;
        }

        private async Task ZaladujZalezneViewModele()
        {
            await RuchTowarRWViewModel.LoadAsync(VMEntity?.IDProdukcjaZlecenieTowar);
            await RuchTowarPWViewModel.LoadAsync(VMEntity?.IDProdukcjaZlecenieTowar);
        }

        private void WybierzGniazdo()
        {
            if (gniazdoProdukcyjne != null && gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne != 0)
                WybraneGniazdo = ListaGniazdProdukcyjnych
                                                    .SingleOrDefault(g => g.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
            else if (VMEntity?.IDProdukcjaGniazdoProdukcyjne != 0)
                WybraneGniazdo = ListaGniazdProdukcyjnych.SingleOrDefault(g => g.IDProdukcjaGniazdoProdukcyjne == VMEntity.IDProdukcjaGniazdoProdukcyjne);
        }

        #endregion

        #region SaveCommand

        /// <summary>
        /// Na potzeby zapisu AutoSave, wiadomość przesyłana jest z <see cref="GPRuchTowarPWViewModel"/>
        /// </summary>
        /// <param name="obj">Message jako znacznik bez wewnetrznej implementacji</param>
        private async void GdyPrzeslanoSaveMessage(GPSaveMessage obj)
        {
            if (AutoSave == false) return;
            if (!VMEntity.IsValid || !RuchTowarRWViewModel.IsValid || !RuchTowarPWViewModel.IsValid)
            {
                DialogService.ShowError_BtnOK("Aby zapisać pozycje proszę o wypełnienie niezbędnych danych zaznaczonych na czerwono.");
                return;
            }

            await SaveAsync();
        }

        protected override void CloseWindowAfterSaving()
        {
            if (!AutoSave)
                base.CloseWindowAfterSaving();
        }


        protected override bool SaveCommandCanExecute()
        {
            if (gniazdoProdukcyjne is null) return false;

            return VMEntity.IsValid
                && naglowekState.IsValid
                && (IsChanged || naglowekState.IsChanged);
        }


        protected override Func<int> GetVMEntityId => () => VMEntity.IDProdukcjaRuchNaglowek;

        protected override Func<Task> UpdateEntityBeforeSaveAction => async () =>
        {
            VMEntity.IDMagazyn = (int)MagazynyEnum.ProdukcjaGeowlokniny_PRGW;
            VMEntity.IDProdukcjaZlecenieCiecia = ZlecenieTowar?.IDProdukcjaZlecenieCiecia;
            VMEntity.IDProdukcjaZlecenieProdukcyjne = ZlecenieTowar?.IDProdukcjaZlecenie;
            VMEntity.DataDodania = DateTime.Now;
            VMEntity.IDProdukcjaGniazdoProdukcyjne = gniazdoProdukcyjne?.IDProdukcjaGniazdoProdukcyjne ?? 0;
        };
        protected override Func<Task> SaveAdditional => async () => await naglowekState.SaveAsync();


        protected override void ShowDialogAfterSaving()
        {
        }

        #endregion


        #region CloseWindowCommand
        protected override void CloseWindowCommandExecute(CancelEventArgs args)
        {
            base.CloseWindowCommandExecute(args);
        }
        #endregion
    }
}
