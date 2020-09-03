using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont.Messages;

namespace GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.DodajStanKonta
{
    public class DodajStanKontaViewModel : SaveCommandGenericViewModelBase
    {
        #region Fields

        private IEnumerable<vwFinanseBankAGG> bankiAGG;
        private IEnumerable<vwFinanseBankGTX> bankiGTX;
        private IEnumerable<vwFinanseBankGTX2> bankiGTX2;
        private tblFinanseStanKonta stanKontaPrzeslany;

        #endregion

        #region Properties

        public ObservableCollection<tblFinanseStanKonta> StanyKont { get; set; } = new ObservableCollection<tblFinanseStanKonta>();
        public ObservableCollection<tblFinanseStanKonta> StanyKontOrg { get; set; } = new ObservableCollection<tblFinanseStanKonta>();

        public DateTime DataStanu { get; set; } = DateTime.Now.Date;
        public override bool IsChanged => !StanyKontOrg.CompareWithList(StanyKont);

        public override bool IsValid => StanyKont.Sum(s => s.Stan) > 0;

        public string TytulNaglowek { get; set; }

        #endregion

        #region Commands


        #endregion

        #region CTOR
        public DodajStanKontaViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            Messenger.Register<tblFinanseStanKonta>(this, GdyPrzeslanoStanKonta);

            TytulNaglowek = $"Dodawanie stanu kont na dzień:";

        }

        private void GdyPrzeslanoStanKonta(tblFinanseStanKonta obj)
        {
            stanKontaPrzeslany = obj;
        }


        #endregion

        public override void IsChanged_False()
        {
            StanyKontOrg = StanyKont.DeepClone();
        }

        protected override bool DeleteCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteCommandExecute()
        {
            throw new NotImplementedException();
        }

        #region LoadCommand

        protected override async void LoadCommandExecute()
        {
            bankiAGG = await UnitOfWork.vwFinanseBankAGG.GetAllAsync();
            bankiGTX = await UnitOfWork.vwFinanseBankGTX.GetAllAsync();
            bankiGTX2 = await UnitOfWork.vwFinanseBankGTX2.GetAllAsync();

            if (stanKontaPrzeslany is null)
            {
                StworzStanyKontDoDodaniaDla(bankiAGG);
                StworzStanyKontDoDodaniaDla(bankiGTX);
                StworzStanyKontDoDodaniaDla(bankiGTX2);
            }
            else
            {
                await DodajPrzeslanyStanKontaDoListy();
            }

            IsChanged_False();
        }

        private async Task DodajPrzeslanyStanKontaDoListy()
        {
            var stanKonta = await UnitOfWork.tblFinanseStanKonta.GetByIdAsync(stanKontaPrzeslany.IDFinanseStanKonta);
            StanyKont.Add(stanKonta);
        }

        private void StworzStanyKontDoDodaniaDla(IEnumerable<IFinanseBank> listsaBankow)
        {
            foreach (var bank in listsaBankow)
            {
                var stanKonta = new tblFinanseStanKonta()
                {
                    IdFirma = bank.IdFirma,
                    Firma = bank.Firma,
                    IdBank = bank.Id,
                    BankNazwa = bank.Nazwa,
                    NrKonta = bank.Numer,
                    Waluta = bank.Waluta,
                    IdOperator = UzytkownikZalogowany.Uzytkownik?.ID_PracownikGAT ?? 7,
                };

                StanyKont.Add(stanKonta);
            }
        }

        #endregion

        protected override bool SaveCommandCanExecute()
        {
            return IsValid;
        }

        protected override async void SaveCommandExecute()
        {
            var stanyKontNiezerowe = StanyKont.Where(s => s.Stan > 0
                                                       && s.IDFinanseStanKonta == 0);

            if (stanyKontNiezerowe.Any())
                DodajStanyKontDoBazy(stanyKontNiezerowe);

            await UnitOfWork.SaveAsync();

            IsChanged_False();
            Messenger.Send(StanyKont.First(),"ToList");
            //DialogService.ShowInfo_BtnOK("Dodano stany kont do bazy.");
            ViewService.Close(this.GetType().Name);
        }

        private void DodajStanyKontDoBazy(IEnumerable<tblFinanseStanKonta> stanyKontNiezerowe)
        {
            var data = DateTime.Now;
            stanyKontNiezerowe.ToList()
                .ForEach(s =>
                {
                    s.DataDodania = data;
                    s.DataStanu = DataStanu;
                });

            UnitOfWork.tblFinanseStanKonta.AddRange(stanyKontNiezerowe);
        }
    }
}
