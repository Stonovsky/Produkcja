using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Extensions;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Decorators;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie rolki RW do konfekcji
    /// </summary>
    public class GPRuchTowarRWViewModel
        : ListAddEditDeleteMethodGenericViewModelBase<tblProdukcjaRuchTowar>, IGPRuchTowarRWViewModel
    {
        #region Fields
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;
        private tblProdukcjaZlecenieTowar towarZeZelceniaProdukcyjnego;
        private tblProdukcjaZlecenie zlecenieProdukcyjne;
        private bool czyRozchodowano;
        private string kodKreskowy;
        private readonly IGPRuchTowar_RW_Helper rwHelper;
        #endregion

        #region Properties
        public decimal RozchodRolkiRW { get; private set; }
        public bool CzyRolkaRozchodowana { get; set; }
        public override string Title => "RW";
        public override IGenericRepository<tblProdukcjaRuchTowar> Repository => UnitOfWork.tblProdukcjaRuchTowar;


        public string KodKreskowy
        {
            get { return kodKreskowy; }
            set
            {
                kodKreskowy = value;
                ZnajdzProduktWBazie(kodKreskowy);
            }
        }

        public bool IsAddButtonActive { get; set; } = true;
        public bool IsRozchodRolkiButtonEnabled { get; set; } = true;
        public string AddButtonToolTip { get; set; }
        #endregion

        #region Delegates
        public override Func<tblProdukcjaRuchTowar, int> GetElementId => (e) => e.IDProdukcjaRuchTowar;
        #endregion

        #region Commands
        public RelayCommand RozchodujRolkeCommand { get; set; }
        #endregion

        #region CTOR

        public GPRuchTowarRWViewModel(IViewModelService viewModelService,
                                          IGPRuchTowar_RW_Helper rwHelper)
            : base(viewModelService)
        {
            this.rwHelper = rwHelper;

            RozchodujRolkeCommand = new RelayCommand(RozchodujRolkeCommandExecute);

            RegisterMessengers();
        }

        protected virtual void RegisterMessengers()
        {
            // wiadomosc z formularza GPRuchTowarDodajView
            Messenger.Register<DodajEdytujGPRuchTowarMessage>(this, GdyPrzeslanoDodajEdytujMessage);

            // message ze ZlecenieCieciaEwidencjaViewModel lub ZlecenieProdukcyjneEwidencjaViewModel
            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowar);

            // message dot gniazda produkcujnego
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdoProdukcyjne);

            // message z PW gdy przesłano zaawansowanie rozchodu rolki RW
            Messenger.Register<ZaawansowanieRozchoduRolkiRwMessage>(this, GdyPrzeslanoRozchodRolkiRW);
            Messenger.Register<GPAddOnRWButtonMessage>(this, GdyPrzeslanoWylaczRWButton);

        }


        #endregion

        #region Messenger

        #region GdyPrzeslanoWylaczRWButton

        /// <summary>
        /// Metoda wylaczajaca mozliwosc dodawania rolki RW gdy z tej rolki powastala jakakolwiek rolka PW
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoWylaczRWButton(GPAddOnRWButtonMessage obj)
        {
            if (ListOfVMEntities.Any())
                IsAddButtonActive = obj.IsEnabled;
        }


        #endregion

        #region GdyPrzeslanoGniazdoProdukcyjne
        private void GdyPrzeslanoGniazdoProdukcyjne(tblProdukcjaGniazdoProdukcyjne obj)
        {
            gniazdoProdukcyjne = obj;
        }

        #endregion

        #region GdyPrzeslanoTowarZeZleceniaProdLubCiecia

        private void GdyPrzeslanoZlecenieTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            towarZeZelceniaProdukcyjnego = obj;
            zlecenieProdukcyjne = obj.tblProdukcjaZlecenie;
        }

        #endregion

        #region GdyPrzeslanoMessageZFormularzaDodawaniaRolki

        /// <summary>
        /// Gdy przeslano message z formularza dodawania rolki, rolka zostaje przypisana (dodana) do listy glownej
        /// Tylko 1 rolka może być dodana do RW!!!
        /// </summary>
        /// <param name="obj">Przeslany message z </param>
        private void GdyPrzeslanoDodajEdytujMessage(DodajEdytujGPRuchTowarMessage obj)
        {
            if (obj == null || obj.RuchStatus is null || obj.RuchTowar is null) return;
            if (obj.RuchStatus.IDRuchStatus == 0) return;
            if (obj.RuchStatus.IDRuchStatus != (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW) return;

            DodajRolkeDoListyRW(obj.RuchTowar);
            IsRozchodRolkiButtonEnabled = true;
        }



        private void WyslijRolkeDoPW(tblProdukcjaRuchTowar towar)
        {
            Messenger.Send(new RwProdukcjaRuchTowaruMessage { RwTowar = towar });
        }
        /// <summary>
        /// Metoda oceniajaca czy towar moze byc dodany do listy RW
        /// </summary>
        /// <param name="towar"></param>
        /// <returns></returns>
        private bool CzyDodacRolkeRwDoListy(tblProdukcjaRuchTowar towar)
        {
            if (towarZeZelceniaProdukcyjnego is null) return true; //dla odpalenia formularza w czasie testow

            return CzyTowarZgodyZeZleceniem(towar);
        }
        /// <summary>
        /// Metoda oceniajaca czy towar jest zgodny ze zleceniem ciecia lub prod.
        /// </summary>
        /// <param name="towar"></param>
        /// <returns></returns>
        private bool CzyTowarZgodyZeZleceniem(tblProdukcjaRuchTowar towar)
        {
            if (towarZeZelceniaProdukcyjnego.IDTowarGeowlokninaParametryGramatura != towar.IDGramatura)
                return false;
            if (towarZeZelceniaProdukcyjnego.IDTowarGeowlokninaParametrySurowiec != towar.IDTowarGeowlokninaParametrySurowiec)
                return false;
            if (towarZeZelceniaProdukcyjnego.Dlugosc_m > towar.Dlugosc_m)
                return false;
            if (towarZeZelceniaProdukcyjnego.Szerokosc_m > towar.Szerokosc_m)
                return false;

            return true;
        }

        #endregion

        #region RozchodRolki
        /// <summary>
        /// Message ktora przesyla PW gdy zakocznono ciecie rolki i nalezy zrobic rozchod
        /// </summary>
        /// <param name="obj"></param>
        private void GdyPrzeslanoRozchodRolkiRW(ZaawansowanieRozchoduRolkiRwMessage obj)
        {
            RozchodRolkiRW = obj.Rozchod;
        }

        private void ZmianaStatusuRozchoduAsync(ProdukcjaRuchTowarStatusEnum produkcjaRuchTowarStatusEnum)
        {
            //TODO
            SelectedVMEntity.IDProdukcjaRuchTowarStatus = (int)produkcjaRuchTowarStatusEnum;
        }

        #endregion

        #endregion

        #region Rozchodowanie rolki
        private async void RozchodujRolkeCommandExecute()
        {
            if (DialogService.ShowQuestion_BoolResult($"Czy rozchodować rolkę? " +
                                                     $"\r\nTa komenda wygeneruje odpad dla danej rolki RW jako różnica wagi z zarejestrownych rolek PW i wagi rolki RW."))
            {
                ZmianaStatusuRozchoduAsync(ProdukcjaRuchTowarStatusEnum.Rozchodowano);
                CzyRolkaRozchodowana = true;

                await PrzypiszOdpadDoRolkiRW();

                await UnitOfWork.SaveAsync();

                WyslijWiadomoscZRozchodemDoPW();
                PosprzatajPoRozchodzie();
                IsRozchodRolkiButtonEnabled = false;
            }
        }

        private void WyslijWiadomoscZRozchodemDoPW()
        {
            Messenger.Send(new RozchodCalkowityRolkiRWMessage { RolkaRW = ListOfVMEntities.First() });
        }

        private void PosprzatajPoRozchodzie()
        {
            ListOfVMEntities.Remove(SelectedVMEntity);
            RozchodRolkiRW = 0;
            KodKreskowy = default;
            IsAddButtonActive = true;
        }

        private async Task PrzypiszOdpadDoRolkiRW()
        {
            SelectedVMEntity.WagaOdpad_kg = await rwHelper.RolkaBazowaHelper.PobierzOdpadZRolkiRwAsync(SelectedVMEntity.IDProdukcjaRuchTowar, gniazdoProdukcyjne);
        }

        #endregion

        #region KodKreskowy
        private void ZnajdzProduktWBazie(string kodKreskowy)
        {
            if (string.IsNullOrEmpty(kodKreskowy)) return;

            var towar = ZnajdzTowarWBazie(kodKreskowy);
            if (towar is null) return;

            DodajRolkeDoListyRW(towar);
        }

        private void UzypelnijStatusy()
        {
            SelectedVMEntity.IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
        }

        private void UzupelnijDaneZPobranejRolkiRW(tblProdukcjaRuchTowar towar)
        {
            SelectedVMEntity.IDRolkaBazowa = towar.IDProdukcjaRuchTowar;
            SelectedVMEntity.IDProdukcjaRuchTowarWyjsciowy = towar.IDProdukcjaRuchTowar;
        }

        private static void OkreslKierunekPrzychdu(tblProdukcjaRuchTowar towar)
        {
            if (towar is null) return;
            towar.KierunekPrzychodu = towar.PobierzKierunekPrzychodu();
        }

        private tblProdukcjaRuchTowar ZnajdzTowarWBazie(string kodKreskowy)
        {
            if (!IsAddButtonActive)
            {
                DialogService.ShowInfo_BtnOK("Należy rozchodować rolkę bieżącą zanim doda się nową rolkę do rozchodu.");
                return null;
            }

            return PobierzRolkeZBazy(kodKreskowy);
        }

        private tblProdukcjaRuchTowar PobierzRolkeZBazy(string kodKreskowy)
        {
            var idGniazdaDoWyszukaniaRolki = PobierzGniazdoDoWyszukania(gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
            
            var listaTowarow = UnitOfWork.tblProdukcjaRuchTowar.Where(t => t.KodKreskowy == kodKreskowy
                                                                        && (t.IDProdukcjaRuchTowarStatus == null || t.IDProdukcjaRuchTowarStatus != (int)GAT_Produkcja.db.Enums.ProdukcjaRuchTowarStatusEnum.Rozchodowano)
                                                                        && t.IDProdukcjaGniazdoProdukcyjne==idGniazdaDoWyszukaniaRolki);

            if (!listaTowarow.Any())
            {
                DialogService.ShowInfo_BtnOK("Brak rolki o podanym kodzie w bazie danych. Sprawdź kod i spróbuj ponownie.");
                return null;
            }

            return listaTowarow.Last();
        }

        private int PobierzGniazdoDoWyszukania(int idGniazdo)
        {
            if (idGniazdo>1)
            {
                return idGniazdo - 1;
            }
            else
            {
                return idGniazdo;
            }
        }

        private void DodajRolkeDoListyRW(tblProdukcjaRuchTowar towar)
        {
            if (CzyDodacRolkeRwDoListy(towar))
            {
                DodajDoListyRW(towar);
                OkreslKierunekPrzychdu(SelectedVMEntity);
                UzupelnijDaneZTowaruZleceniaDlaNowejPozycjiRW(null, ListOfVMEntities);
                UzupelnijDaneZPobranejRolkiRW(SelectedVMEntity);
                UzypelnijStatusy();
                WyslijRolkeDoPW(SelectedVMEntity);
                CzyRolkaRozchodowana = false;
            }
            else
            {
                if (DialogService.ShowQuestion_BoolResult("Rolka nie jest zgodna ze zleceniem, czy pomimo tego dodać ją do RW?"))
                {
                    DodajDoListyRW(towar);
                }
            }
        }

        private void DodajDoListyRW(tblProdukcjaRuchTowar towar)
        {
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>();
            ListOfVMEntities.Add(towar);
            SelectedVMEntity = ListOfVMEntities.First();
        }
        #endregion

        #region Add

        protected override bool AddCommandCanExecute()
        {
            if (gniazdoProdukcyjne is null)
            {
                AddButtonToolTip = "Brak przypisanego gniazda produkcyjnego";
                return false;
            }

            if (towarZeZelceniaProdukcyjnego is null)
            {
                AddButtonToolTip = "Brak wybranego zlecenia. Dodaj zlecenie.";
                return false;
            }
            if (RozchodRolkiRW > 0)
            {
                AddButtonToolTip = "Zakończ rozchodowanie bieżącej rolki żeby dodać nową.";
                return false;
            
            }
            if (!IsAddButtonActive) return false;

            AddButtonToolTip = "Dodaj rolkę RW";
            return true;
        }

        public override Action ShowAddEditWindow => () =>
        {
            ViewService.Show<GPRuchTowarDodajViewModel>();

            Messenger.Send<DodajEdytujGPRuchTowarMessage, GPRuchTowarDodajViewModel>(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = GenerujRuchTowar()
            });
        };

        private tblProdukcjaRuchTowar GenerujRuchTowar()
        {
            if (towarZeZelceniaProdukcyjnego is null)
                return new tblProdukcjaRuchTowar();


            return new tblProdukcjaRuchTowar()
            {
                IDGramatura = towarZeZelceniaProdukcyjnego.IDTowarGeowlokninaParametryGramatura,
                IDTowarGeowlokninaParametrySurowiec = towarZeZelceniaProdukcyjnego.IDTowarGeowlokninaParametrySurowiec,
                CzyKalandrowana = towarZeZelceniaProdukcyjnego.CzyKalandrowana,
                tblTowarGeowlokninaParametrySurowiec = towarZeZelceniaProdukcyjnego.tblTowarGeowlokninaParametrySurowiec,
                tblTowarGeowlokninaParametryGramatura = towarZeZelceniaProdukcyjnego.tblTowarGeowlokninaParametryGramatura,
                //NrZleceniaProdukcjnego = zlecenieTowar.tblProdukcjaZlcecenieProdukcyjne.NrZleceniaProdukcyjnego.GetValueOrDefault()
            };
        }


        #endregion

        #region Save

        protected override Action<int?> UpdateEntityBeforeSaveAction => (idRuchNaglowek) =>
        {
            if (idRuchNaglowek == null || idRuchNaglowek == 0) return;

            UsunWszystkieChildTabeleZListyPW();
            UzupelnijDaneZTowaruZleceniaDlaNowejPozycjiRW(idRuchNaglowek, ListOfVMEntities);
            DodajNowePozycjeDoBazy();
            ZmianaStatusuRozchoduAsync(ProdukcjaRuchTowarStatusEnum.CzesciowoRozchodowano);

        };

        /// <summary>
        /// Metoda usuwajaca wszystkie obiekty typu child tak aby nie byly przypadkowo dodane do bazy
        /// </summary>
        private void UsunWszystkieChildTabeleZListyPW()
        {
            foreach (var pozycja in ListOfVMEntities)
            {
                if (pozycja.IDProdukcjaRuchTowar == 0)
                    pozycja.RemoveChildObjects();
            }
        }

        private void UzupelnijDaneZTowaruZleceniaDlaNowejPozycjiRW(int? idRuchNaglowek, ObservableCollection<tblProdukcjaRuchTowar> listaRW)
        {
            var rolkaRW = listaRW.First();
            rolkaRW = new RolkaRWDecorator(rolkaRW).Uzupelnij(idRuchNaglowek, towarZeZelceniaProdukcyjnego);
        }

        /// <summary>
        /// Metoda dodajac do bazy tylko nowy obiekty o Id=0, obiekt już istniejący -> update
        /// </summary>
        private void DodajNowePozycjeDoBazy()
        {
            var rolkaRWDoDodania = ListOfVMEntities.SingleOrDefault(p => p.IDProdukcjaRuchTowar == 0);

            if (rolkaRWDoDodania != null)
                UnitOfWork.tblProdukcjaRuchTowar.Add(rolkaRWDoDodania);
        }


        #endregion

        #region Load
        public override async Task LoadAsync(int? idZlecenieTowar)
        {
            if (idZlecenieTowar == null || idZlecenieTowar == 0) return;

            await DodajTowarDoListyRW(idZlecenieTowar);
            await PobierzZlecenia(idZlecenieTowar);
            IsChanged_False();
        }


        private async Task PobierzZlecenia(int? idZlecenieTowar)
        {
            //if (idZlecenieTowar is null) return;
            //var naglowek = await UnitOfWork.tblProdukcjaRuchNaglowek.GetByIdAsync(idZlecenieTowar.Value);

            //var rolkaPwDlaNaglowka = await UnitOfWork.tblProdukcjaRuchTowar.GetFirsAsync(e => e.IDProdukcjaRuchNaglowek == idZlecenieTowar.Value);
            //if (rolkaPwDlaNaglowka is null) return;

            if (idZlecenieTowar is null) return;

            towarZeZelceniaProdukcyjnego = await UnitOfWork.tblProdukcjaZlecenieTowar.GetByIdAsync(idZlecenieTowar.Value);
            if (towarZeZelceniaProdukcyjnego is null) return;

            zlecenieProdukcyjne = towarZeZelceniaProdukcyjnego.tblProdukcjaZlecenie;
        }

        /// <summary>
        /// Metoda dodajaca rolke RW wyszukujac po IdNaglowka. 
        /// Dla kazdego naglowka jest tylko jedna rolka RW!
        /// </summary>
        /// <param name="idZlecenieTowar">idNaglowka dla RuchuTowaru <see cref="GPRuchNaglowekViewModel"/></param>
        /// <returns></returns>
        private async Task DodajTowarDoListyRW(int? idZlecenieTowar)
        {
            var rolkiRWdlaNaglowka = await UnitOfWork.tblProdukcjaRuchTowar
                                                    .WhereAsync(r => r.IDProdukcjaZlecenieTowar == idZlecenieTowar
                                                                  && r.IDRuchStatus == (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW
                                                                  && r.IDProdukcjaRuchTowarStatus != (int)ProdukcjaRuchTowarStatusEnum.Rozchodowano);

            if (!rolkiRWdlaNaglowka.Any())
            {
                CzyscListeRW();
                return;
            }

            var rolkaRW = rolkiRWdlaNaglowka.OrderByDescending(r => r.DataDodania)
                                            .First();

            DodajDoListyRW(rolkaRW);
            WyslijRolkeDoPW(rolkaRW);
        }

        private void CzyscListeRW()
        {
            IsAddButtonActive = true;
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>();
            RozchodRolkiRW = 0;
        }

        #endregion
    }
}
