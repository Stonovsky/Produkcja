using DocumentFormat.OpenXml.Presentation;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Decorator;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Models;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW
{
    public class GPRuchTowarPWViewModel
        : ListAddEditDeleteMethodGenericViewModelBase<tblProdukcjaRuchTowar>, IGPRuchTowarPWViewModel
    {
        #region Fields

        private readonly IGPRuchTowar_PW_Helper pwHelper;
        private readonly IGPRuchTowar_PW_PodsumowanieStrategyFactory podsumowanieStrategyFactory;
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        private tblProdukcjaZlecenie zlecenieProdukcyjne;
        private tblProdukcjaRuchTowar rolkaRW;
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;
        private IGPRuchTowar_PW_PodsumowanieHelper podsumowanie;
        private bool isLoadAsyncFinished;
        private tblPracownikGAT pracownik;


        #endregion

        #region Properties

        public override ObservableCollection<tblProdukcjaRuchTowar> ListOfVMEntities
        {
            get => base.ListOfVMEntities;
            set
            {
                base.ListOfVMEntities = value;
                GenerujPodsumowanie();
            }
        }

        public tblProdukcjaGniazdoProdukcyjne GniazdoProdukcyjne
        {
            get => gniazdoProdukcyjne;
            set
            {
                gniazdoProdukcyjne = value;
                //BazowaRolkaPW.IDProdukcjaGniazdoProdukcyjne = gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne;
                podsumowanie = podsumowanieStrategyFactory.PobierzPodsumowanieStrategy((GniazdaProdukcyjneEnum)gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
            }
        }
        public tblProdukcjaZlecenieTowar ZlecenieTowar { get => zlecenieTowar; set => zlecenieTowar = value; }
        public tblProdukcjaRuchTowar RolkaRW { get => rolkaRW; set => rolkaRW = value; }

        public IGPRuchTowar_PW_Helper PwHelper => pwHelper;


        #region RolkaBazowa
        public tblProdukcjaRuchTowar BazowaRolkaPW { get; set; } = new tblProdukcjaRuchTowar();

        public IEnumerable<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; } = new List<tblTowarGeowlokninaParametryGramatura>();
        public tblTowarGeowlokninaParametryGramatura WybranaGramatura { get; set; }
        public IEnumerable<tblTowarGeowlokninaParametrySurowiec> ListaSurowcow { get; set; } = new List<tblTowarGeowlokninaParametrySurowiec>();
        public tblTowarGeowlokninaParametrySurowiec WybranySurowiec { get; set; }
        public decimal Szerokosc_m { get; set; }
        public decimal Dlugosc_m { get; set; }
        public int IloscRolek { get; set; }

        #endregion


        public override string Title => "PW";
        public RolkaPWDecorator RolkaPWDecorator { get; set; }
        public PWChildObjectHelper PWChildObjectHelper { get; set; }
        public bool IloscSztWidoczne { get; set; }
        public override IGenericRepository<tblProdukcjaRuchTowar> Repository => UnitOfWork.tblProdukcjaRuchTowar;

        #region Podsumowanie
        public PodsumowaniePWModel PodsumowaniePW_Kwalifikowane { get; set; } = new PodsumowaniePWModel();
        public PodsumowaniePWModel PodsumowaniePW_NieKwalifikowane { get; set; } = new PodsumowaniePWModel();
        public PodsumowaniePWModel PodsumowaniePW_Pozostalo { get; set; } = new PodsumowaniePWModel();
        #endregion

        #endregion

        #region Commands
        public RelayCommand PobierzKolejnyNumerPaletyCommand { get; set; }
        #endregion

        #region CTOR
        public GPRuchTowarPWViewModel(IViewModelService viewModelService,
                                      IGPRuchTowar_PW_Helper pwHelper,
                                      IGPRuchTowar_PW_PodsumowanieStrategyFactory podsumowanieStrategyFactory)
            : base(viewModelService)
        {
            this.pwHelper = pwHelper;
            this.podsumowanieStrategyFactory = podsumowanieStrategyFactory;

            podsumowanie = podsumowanieStrategyFactory.PobierzPodsumowanieStrategy(GniazdaProdukcyjneEnum.LiniaWloknin);

            PobierzKolejnyNumerPaletyCommand = new RelayCommand(PobierzKolejnyNumerPaletyCommandExecute);

            RolkaPWDecorator = new RolkaPWDecorator();
            PWChildObjectHelper = new PWChildObjectHelper();
        }


        protected override void MessengerRegistration()
        {
            base.MessengerRegistration();

            // msg ZleceniaTowaru z ktorego przypisywane sa zlecenia ciecia i zlecenia produkcyjne do pol + przypisanie z RW
            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowar);
            // msg z gniazda 
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdo);
            // msg z formularza dodaj
            Messenger.Register<DodajEdytujGPRuchTowarMessage>(this, GdyPrzeslano_DodajEdytujGPRuchTowarMessage);
            // rolka z RW
            Messenger.Register<RwProdukcjaRuchTowaruMessage>(this, GdyPrzeslanoTowarZRW);

            Messenger.Register<RozchodCalkowityRolkiRWMessage>(this, GdyPrzeslanoRozchodCalkowityRolki);

            Messenger.Register<tblPracownikGAT>(this, GdyPrzeslanoOperatora);
        }

        private void GdyPrzeslanoOperatora(tblPracownikGAT obj)
        {
            pracownik = obj;
            AddCommandToolTip = "Dodaj rolkę PW";
        }
        #endregion


        #region Messengers

        #region GdyPrzeslanoZlecenieTowar
        /// <summary>
        /// Metoda przypisujaca zlecenia i uruchamiajaca generowanie RolkiBazowej poniewaz ten delegat bedzie wywolywany najpozniej 
        /// - w momencie dodania zlecenia w naglowku
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoZlecenieTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            PrzypiszZlecenia(obj);
            GenerujRolkeBazowaPW();
            GenerujPozycjeWybrane();
        }
        private void PrzypiszZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            this.ZlecenieTowar = zlecenieTowar;
            zlecenieProdukcyjne = zlecenieTowar.tblProdukcjaZlecenie;
        }

        private void GenerujPozycjeWybrane()
        {
            if (ListaGramatur is null) return;
            if (ListaSurowcow is null) return;

            WybranaGramatura = ListaGramatur
                                .SingleOrDefault(s => s.IDTowarGeowlokninaParametryGramatura == ZlecenieTowar.IDTowarGeowlokninaParametryGramatura);
            WybranySurowiec = ListaSurowcow
                                .SingleOrDefault(s => s.IDTowarGeowlokninaParametrySurowiec == ZlecenieTowar.IDTowarGeowlokninaParametrySurowiec);

            IloscRolek = ZlecenieTowar?.Ilosc_szt ?? 0;
        }


        #endregion

        #region PrzeslanoGniazdo
        /// <summary>
        /// Gdy przeslano gniazdo - nastepuje przypisanie do Propertisa
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoGniazdo(tblProdukcjaGniazdoProdukcyjne obj)
        {
            GniazdoProdukcyjne = obj;

            UstalWidocznoscIlosciSzt();
        }

        private void UstalWidocznoscIlosciSzt()
        {
            if (GniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji)
                IloscSztWidoczne = true;
            else
                IloscSztWidoczne = false;
        }
        #endregion

        #region GdyPrzeslanoRolkeDoDodania

        /// <summary>
        /// Dodawanie rolki PW do <see cref="ListOfVMEntities"/> wraz z aktualizacja podsumowania oraz zaawansowania rozchodu rolki
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslano_DodajEdytujGPRuchTowarMessage(DodajEdytujGPRuchTowarMessage obj)
        {
            if (obj == null || obj.RuchStatus is null || obj.RuchTowar is null) return;
            if (obj.RuchStatus.IDRuchStatus == 0) return;
            if (obj.RuchStatus.IDRuchStatus != (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW) return;

            DodajElementDoListy(obj);
            GenerujPodsumowanie();
            WyslijZaawansowanieRozchoduRolkiRWDoPW();
        }

        #region Dodawanie elementow do listy
        /// <summary>
        /// Metoda sprawdza czy ilosc m2 dodawanej rolki wraz z juz wczesniej dodanymi rolkami do listy nie jest wieksza od m2 rolki RW
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void DodajElementDoListy(DodajEdytujGPRuchTowarMessage obj)
        {
            if (obj.RuchStatus.IDRuchStatus != (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW) return;
            if (obj.DodajUsunEdytujEnum != DodajUsunEdytujEnum.Dodaj) return;
            if (!PwHelper.KonfekcjaHelper.CzyIloscM2ZgodnaZRolkaRW(RolkaRW, obj.RuchTowar, ListOfVMEntities)) return;
            if (!PwHelper.KonfekcjaHelper.CzyIloscM2ZgodnaZeZleceniem(ZlecenieTowar, obj.RuchTowar, ListOfVMEntities)) return;

            DodajRolkePwDoListy(obj);

            WyslijWiadomoscOWylaczeniuPrzyciskuDodajNaRW();
            WyslijWiadomoscDoNaglowkaOZapisie();
        }

        /// <summary>
        /// Wysyla wiadomosc w celu wylaczenia przycisku Dodaj na RW
        /// </summary>
        private void WyslijWiadomoscOWylaczeniuPrzyciskuDodajNaRW()
        {
            if (ListOfVMEntities.Any())
                Messenger.Send(new GPAddOnRWButtonMessage { IsEnabled = false });
        }

        private void DodajRolkePwDoListy(DodajEdytujGPRuchTowarMessage obj)
        {
            var nowyRuchTowar = obj.RuchTowar.DeepClone();
            DodajNumeryPorzadkowe(nowyRuchTowar);
            ListOfVMEntities.Add(nowyRuchTowar);
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
                                        (ListOfVMEntities.OrderByDescending(e => e.LP));
        }

        /// <summary>
        /// Wysyla wiadomosc do naglowka o zapisie na potrzeby AutoSave
        /// </summary>
        private void WyslijWiadomoscDoNaglowkaOZapisie()
        {
            Messenger.Send(new GPSaveMessage());
        }

        /// <summary>
        /// Dodaje nry porzadkowe do Listy pozycji PW
        /// </summary>
        /// <param name="nowyRuchTowar"></param>
        /// <returns></returns>
        private void DodajNumeryPorzadkowe(tblProdukcjaRuchTowar nowyRuchTowar)
        {
            nowyRuchTowar.LP = ListOfVMEntities.Count() + 1;
        }
        #endregion

        /// <summary>
        /// Metoda generuje zaawansowanie rozchodu rolki RW i wysyla wiadomosc do RwVM
        /// </summary>
        private void WyslijZaawansowanieRozchoduRolkiRWDoPW()
        {
            var rozchodRolkiRw = PwHelper.ZaawansowanieHelper.PobierzRozchodRolkiRw(RolkaRW, ListOfVMEntities);
            if (rozchodRolkiRw > 0)
                Messenger.Send(new ZaawansowanieRozchoduRolkiRwMessage { Rozchod = rozchodRolkiRw });
        }
        #endregion

        #region GdyPrzeslanoRolkeZRW

        /// <summary>
        /// msg wyslany tylko wowczas, gdy rolka zozstala dodana do RW i spelnia kryteria zlecenia produkcyjnego lub zlecenia ciecia
        /// rolkaRW zostaje przypisana do pola, uzupelnienei <see cref="BazowaRolkaPW"/>
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoTowarZRW(RwProdukcjaRuchTowaruMessage obj)
        {
            if (obj is null || obj.RwTowar is null) return;

            RolkaRW = obj.RwTowar;

            //await Task.Delay(300);

            //if (isLoadAsyncFinished)
            GenerujRolkeBazowaPW();

        }
        #endregion

        #region GdyPrzeslanoRozchodCalkowityRolki

        /// <summary>
        /// Metoda wylaczajaca AddButton
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoRozchodCalkowityRolki(RozchodCalkowityRolkiRWMessage obj)
        {
            WylaczAddButton();
        }

        /// <summary>
        /// Wylacza mozliwosc dodawania nowych rolek na PW <see cref="AddCommandCanExecute"/> = false
        /// </summary>
        private void WylaczAddButton()
        {
            RolkaRW = null;
        }

        #endregion

        #endregion

        #region Delegaty
        public override Func<tblProdukcjaRuchTowar, int> GetElementId => (e) => e.IDProdukcjaRuchTowar;
        #endregion

        #region Uzupelnianie danych Rolki Bazowej PW
        private void GenerujRolkeBazowaPW()
        {
            PobierzDaneZRolkiRwDlaRolkiBazowej();
            PobierzDaneZeZleceniaDlaRolkiBazowej();
            PrzypiszParametryOgolneDoRolkiBazowej();
        }


        #region Uzupelnianie danych ze Zlecenia

        /// <summary>
        /// Pobiera dane ze zlecenia dla <see cref="BazowaRolkaPW"/>, ktora nastepnie wysylana jest do formularza dodawania
        /// </summary>
        private void PobierzDaneZeZleceniaDlaRolkiBazowej()
        {
            if (ZlecenieTowar is null) return;

            BazowaRolkaPW = PwHelper.RolkaBazowaHelper.PobierzDaneZeZlecenia(BazowaRolkaPW, ZlecenieTowar);
            DodajZlecenieBazowe();
        }

        private void DodajZlecenieBazowe()
        {
            if (gniazdoProdukcyjne is null) return;

            if (gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin
                && BazowaRolkaPW != null)
            {
                BazowaRolkaPW.IDZleceniePodstawowe = BazowaRolkaPW.IDProdukcjaZlecenieProdukcyjne;
                BazowaRolkaPW.NrZleceniaPodstawowego = BazowaRolkaPW.NrZlecenia;
            }
        }

        #endregion

        #region Uzupelnieni danych z RW
        /// <summary>
        /// Przypisuje do <see cref="BazowaRolkaPW"/> dane pochodzace z rolki przslanej z formularza RW
        /// </summary>
        /// <returns></returns>
        private void PobierzDaneZRolkiRwDlaRolkiBazowej()
        {
            if (RolkaRW is null) return;
            BazowaRolkaPW = PwHelper.RolkaBazowaHelper.PobierzDaneZRolkiRw(BazowaRolkaPW, RolkaRW, GniazdoProdukcyjne);
        }

        #endregion

        #region Uzupelnienie danych pozostalych

        private void PrzypiszParametryOgolneDoRolkiBazowej()
        {
            if (BazowaRolkaPW is null) return;

            BazowaRolkaPW.LP = ListOfVMEntities.Count() + 1;
            BazowaRolkaPW.IDProdukcjaGniazdoProdukcyjne = gniazdoProdukcyjne?.IDProdukcjaGniazdoProdukcyjne;

            if (gniazdoProdukcyjne is null) return;

            if (gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                BazowaRolkaPW.IDZleceniePodstawowe = BazowaRolkaPW.IDProdukcjaZlecenieProdukcyjne;
                BazowaRolkaPW.NrZleceniaPodstawowego = BazowaRolkaPW.NrZlecenia;
            }
        }

        #endregion

        #endregion

        #region Palety
        private async void PobierzKolejnyNumerPaletyCommandExecute()
        {
            await PobierzKolejnyNrPaletyAsync();
        }
        private async Task PobierzKolejnyNrPaletyAsync()
        {
            DateTime tydzien = DateTime.Now.Date.AddDays(-7);
            if (gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji)
                BazowaRolkaPW.NrPalety = await UnitOfWork.tblProdukcjaRuchTowar.GetNewNumberAsync(r => r.NrPalety > 0
                                                                                                    && r.DataDodania >= tydzien
                                                                                                     , r => r.NrPalety);
            else
                BazowaRolkaPW.NrPalety = 0;
        }
        #endregion

        #region Podsumowanie
        private void GenerujPodsumowanie()
        {
            podsumowanie.Init(ListOfVMEntities, ZlecenieTowar);
            PodsumowaniePW_Kwalifikowane = podsumowanie.PodsumowanieRolekKwalifikowanych();
            PodsumowaniePW_NieKwalifikowane = podsumowanie.PodsumowanieRolekNieKwalifikowanych();
            PodsumowaniePW_Pozostalo = podsumowanie.PodsumowaniePozostalo();
        }
        #endregion

        #region Add

        protected override bool AddCommandCanExecute()
        {
            if (pracownik is null)
            {
                AddCommandToolTip = "Brak przypisanego operatora";
                return false;
            }

            if (GniazdoProdukcyjne == null)
                return true;

            if (GniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                AddCommandToolTip = "Dodaj rolke PW";
                return true;
            }

            if (RolkaRW is null)
            {
                AddCommandToolTip = "Dodaj rolkę do RW";
                return false;
            }

            AddCommandToolTip = "Dodaj rolkę do PW";
            return true;

        }

        public override Action ShowAddEditWindow => () =>
        {
            ViewService.ShowDialog<GPRuchTowarDodajViewModel>(
                ()=> Wyslij_DodajEdytujGPRuchTowarMessage(DodajUsunEdytujEnum.Dodaj)
                );
            
        };


        protected override void EditCommandExecute()
        {
            ViewService.Show<GPRuchTowarDodajViewModel>();
            Wyslij_DodajEdytujGPRuchTowarMessage(DodajUsunEdytujEnum.Edytuj);
        }


        private void Wyslij_DodajEdytujGPRuchTowarMessage(DodajUsunEdytujEnum dodajUsunEdytujEnum)
        {
            tblProdukcjaRuchTowar towar = GenerujTowarDoWyslania(dodajUsunEdytujEnum);

            Messenger.Send<DodajEdytujGPRuchTowarMessage, GPRuchTowarDodajViewModel>(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                DodajUsunEdytujEnum = dodajUsunEdytujEnum,
                RuchTowar = towar,
                ZlecenieProdukcyjne = zlecenieProdukcyjne,
                RolkaRW = RolkaRW,
            });
        }

        private tblProdukcjaRuchTowar GenerujTowarDoWyslania(DodajUsunEdytujEnum dodajUsunEdytujEnum)
        {
            tblProdukcjaRuchTowar towar = new tblProdukcjaRuchTowar();
            if (dodajUsunEdytujEnum == DodajUsunEdytujEnum.Edytuj)
                towar = SelectedVMEntity;
            else
                towar = BazowaRolkaPW.DeepClone();
            return towar;
        }

        /// <summary>
        /// Generuje podstawowe dane towaru, ktore beda przeslane do formularza DODAJ
        /// </summary>
        /// <returns></returns>
        private tblProdukcjaRuchTowar GenerujBazeTowaru()
        {
            var nowyTowar = BazowaRolkaPW.DeepClone();
            nowyTowar.WagaOdpad_kg = 0;
            nowyTowar.Waga_kg = 0;

            return nowyTowar;
        }

        #endregion

        #region LoadAsync

        /// <summary>
        /// Laduje <see cref="ListOfVMEntities"/> oraz <see cref="ListaGramatur"/> i <see cref="ListaSurowcow"/> z bazy
        /// </summary>
        /// <param name="idZlecenieTowar">Parametr niezbedny do pobrania <see cref="ListOfVMEntities"/> z bazy do edycji</param>
        /// <returns></returns>
        public override async Task LoadAsync(int? idZlecenieTowar)
        {
            isLoadAsyncFinished = false;

            await PobierzZleceniaZBazyAsync(idZlecenieTowar);
            PobierzRolkeBazowaAsync();
            await GetListOfVMEntitiesAsync(idZlecenieTowar);
            NumerujListe();
            WyslijWiadomoscOWylaczeniuPrzyciskuDodajNaRW();
            WyslijZaawansowanieRozchoduRolkiRWDoPW();
            ListaGramatur = await UnitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync();
            ListaSurowcow = await UnitOfWork.tblTowarGeowlokninaParametrySurowiec.GetAllAsync();
            await PobierzKolejnyNrPaletyAsync();
            GenerujRolkeBazowaPW();

            isLoadAsyncFinished = true;
        }

        private void NumerujListe()
        {
            int nr = 1;
            foreach (var item in ListOfVMEntities)
            {
                item.LP = nr;
                nr++;
            }
        }

        private void PobierzRolkeBazowaAsync()
        {
            PobierzDaneZeZleceniaDlaRolkiBazowej();
        }

        private async Task PobierzZleceniaZBazyAsync(int? idZlecenieTowar)
        {
            if (idZlecenieTowar is null) return;
            ZlecenieTowar = await UnitOfWork.tblProdukcjaZlecenieTowar.GetByIdAsync(idZlecenieTowar.Value);
            zlecenieProdukcyjne = ZlecenieTowar?.tblProdukcjaZlecenie;
        }

        /// <summary>
        /// Pobiera lub generuje Liste PW
        /// </summary>
        /// <param name="idZlecenieTowar"></param>
        /// <returns></returns>
        private async Task GetListOfVMEntitiesAsync(int? idZlecenieTowar)
        {
            if (idZlecenieTowar == null || idZlecenieTowar == 0)
                ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>();
            else
                await GetListOfVMEntitesFromDBAsync(idZlecenieTowar);
        }

        /// <summary>
        /// Pobiera z bazy danych encje PW dla danego id naglowka
        /// </summary>
        /// <param name="idZlecenieTowar"> id z <see cref="tblProdukcjaRuchNaglowek"/></param>
        /// <returns></returns>
        private async Task GetListOfVMEntitesFromDBAsync(int? idZlecenieTowar)
        {
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>(
                                    await UnitOfWork.tblProdukcjaRuchTowar
                                                    .WhereAsync(r => r.IDProdukcjaZlecenieTowar == idZlecenieTowar
                                                                  && r.IDRuchStatus == (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW));
        }


        #endregion

        #region SaveAsync

        protected override Action<int?> UpdateEntityBeforeSaveAction => throw new NotImplementedException();

        public string AddCommandToolTip { get; private set; }

        public override async Task SaveAsync(int? idRuchNaglowek)
        {
            if (!idRuchNaglowek.HasValue || idRuchNaglowek == 0) return;

            await UzupelnijDaneDlaPozycjiPW(idRuchNaglowek);
            UsunWszystkieChildObiekty();
            DodajNowePozycjeDoBazy();

            await UnitOfWork.SaveAsync();

            await AktualizujZaawansowanieZlecen();
            AktualizujStatusyZlecen();

            await UnitOfWork.SaveAsync();
        }

        public virtual void UsunWszystkieChildObiekty()
        {
            PWChildObjectHelper.Remove(ListOfVMEntities, new List<string> { "tblProdukcjaRuchTowarBadania" });
        }

        #region UzupelnieniePozycjiPW

        public virtual async Task UzupelnijDaneDlaPozycjiPW(int? idRuchNaglowek)
        {
            foreach (var pozycjaPW in ListOfVMEntities)
            {
                if (pozycjaPW.IDProdukcjaRuchTowar == 0)
                    await RolkaPWDecorator.UzupelnijPozycjePW(idRuchNaglowek, pozycjaPW, this);
            }
        }
        #endregion

        private void DodajNowePozycjeDoBazy()
        {
            var listaNowychPozycji = ListOfVMEntities.Where(p => p.IDProdukcjaRuchTowar == 0);

            if (listaNowychPozycji.Any())
                UnitOfWork.tblProdukcjaRuchTowar.AddRange(listaNowychPozycji);
        }

        #region Zaawansowanie
        /// <summary>
        /// Aktualizuje zaawansowanie zarowno calych zlecen jak i kazdego towaru na zleceniu
        /// </summary>
        /// <returns></returns>
        private async Task AktualizujZaawansowanieZlecen()
        {
            await AktualizujZaawansowanieZlecenieTowar();
            await AktualizujZaawansowanieZlecenia();
        }

        /// <summary>
        /// Aktualizuje zaawansowanie zlecenia
        /// </summary>
        /// <returns></returns>
        private async Task AktualizujZaawansowanieZlecenia()
        {
            if (ZlecenieTowar is null) return;

            if (zlecenieProdukcyjne != null)
                zlecenieProdukcyjne.Zaawansowanie = await PwHelper.ZaawansowanieHelper
                                                                  .PobierzZawansowanie_ProdukcjaZlecenie(ZlecenieTowar.IDProdukcjaZlecenieTowar);
        }

        /// <summary>
        /// Aktualizuje zaawansowanie towaru ze zlecenia
        /// </summary>
        /// <returns></returns>
        private async Task AktualizujZaawansowanieZlecenieTowar()
        {
            if (ZlecenieTowar is null) return;

            ZlecenieTowar.Zaawansowanie = await PwHelper.ZaawansowanieHelper
                                                        .PobierzZaawansowanie_ProdukcjaZlecenieTowar(ZlecenieTowar);
        }
        #endregion

        #region Statusy

        /// <summary>
        /// Aktualizuje statusy zlecen produkcjnych oraz ciecia z uwagi na zaawansowanie tych zlecen
        /// </summary>
        private void AktualizujStatusyZlecen()
        {
            if (ZlecenieTowar is null) return;
            ZlecenieTowar.IDProdukcjaZlecenieStatus = (int)GenerujStatusZlecenia(ZlecenieTowar.Zaawansowanie);

            if (zlecenieProdukcyjne != null)
                zlecenieProdukcyjne.IDProdukcjaZlecenieStatus = (int)GenerujStatusZlecenia(zlecenieProdukcyjne.Zaawansowanie);
        }


        /// <summary>
        /// Zwraca status na podstawie zaawansowania
        /// </summary>
        /// <param name="zaawansowanie">zaawansowanie zlecenia lub towaru ze zlecenia</param>
        /// <returns></returns>
        private ProdukcjaZlecenieStatusEnum GenerujStatusZlecenia(decimal zaawansowanie)
        {
            if (zaawansowanie == 0)
            {
                return ProdukcjaZlecenieStatusEnum.Oczekuje;
            }
            else if (zaawansowanie > 0 && zaawansowanie < 1)
            {
                return ProdukcjaZlecenieStatusEnum.WTrakcie;
            }
            else if (zaawansowanie == 1)
            {
                return ProdukcjaZlecenieStatusEnum.Zakonczone;
            }
            else
            {
                return ProdukcjaZlecenieStatusEnum.Anulowane;
            }
        }

        #endregion

        #endregion

        #region DeleteAsync
        public Task DeleteAsync(int idRuchNaglowek)
        {
            throw new NotImplementedException();
        }
        public override Action AfterDeleteAction => ()=>GenerujPodsumowanie();
        #endregion
    }
}
