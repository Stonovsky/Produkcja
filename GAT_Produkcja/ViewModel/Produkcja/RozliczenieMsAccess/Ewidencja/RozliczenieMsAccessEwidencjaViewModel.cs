using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja
{
    public class RozliczenieMsAccessEwidencjaViewModel 
        : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaRozliczenie_PWPodsumowanie>
    {
        private readonly IRozliczenieMsAccessEwidencjaDeleteHelper deleteHelper;
        private RozliczenieMsAccessEwidencjaFiltrHelper filtrHelper;
        private IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie> listBase;

        #region Properties
        public override string Title => "Ewidencja rozliczeń produkcji";
        public override IGenericRepository<tblProdukcjaRozliczenie_PWPodsumowanie> Repository => UnitOfWork.tblProdukcjaRozliczenie_PWPodsumowanie;
        public List<string> ListaRodzajow { get; set; } = new List<string> { "PES", "PP" };
        public string WybranyRodzaj { get; set; }
        public RozliczenieEwidencjaFiltrModel Filtr { get; set; } = new RozliczenieEwidencjaFiltrModel() { DataOd = DateTime.Now.Date, DataDo = DateTime.Now.Date };
        public RozliczenieEwidencjaPodsumowanieModel Podsumowanie { get; set; }
        #endregion

        #region Commands
        public RelayCommand SearchCommand { get; set; }

        #endregion

        #region CTOR
        public RozliczenieMsAccessEwidencjaViewModel(IViewModelService viewModelService,
                                                     IRozliczenieMsAccessEwidencjaDeleteHelper deleteHelper)
            : base(viewModelService)
        {
            this.deleteHelper = deleteHelper;

            SearchCommand = new RelayCommand(SearchCommandExecute);

            filtrHelper = new RozliczenieMsAccessEwidencjaFiltrHelper();
        }

        #endregion

        private void SearchCommandExecute()
        {
            var lista = filtrHelper.FiltrujListe(listBase, Filtr);
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                                    ( lista);
            GenerujPodsumowanie();
        }


        private void GenerujPodsumowanie()
        {
            Podsumowanie = new RozliczenieEwidencjaPodsumowanieModel
            {
                Ilosc_kg = ListOfVMEntities.Sum(e => e.Ilosc_kg),
                Ilosc_m2 = ListOfVMEntities.Sum(e => e.Ilosc),
                Koszt = ListOfVMEntities.Sum(e => e.Wartosc)
            };
        }

        protected override async Task LoadElements()
        {
            await base.LoadElements();
            listBase = ListOfVMEntities.CopyList();
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                                (ListOfVMEntities.OrderByDescending(e => e.tblProdukcjaRozliczenie_Naglowek.DataDodania).ToList());
            GenerujPodsumowanie();
        }

        #region Messenger
        public override Func<tblProdukcjaRozliczenie_PWPodsumowanie, int> GetElementSentId => (obj) => obj.IDProdukcjaRozliczenie_Naglowek;

        #endregion

        #region AddEdit
        public override Action ShowAddEditWindow => () => ViewService.Show<RozliczenieMsAccessViewModel>();

        #endregion

        #region Delete
        public override Func<Task> DeleteAction => async () =>
        {
           try
           {
               await deleteHelper.UsunRozliczenieAsync(SelectedVMEntity);
           }
           catch (ArgumentException ex)
           {
               DialogService.ShowError_BtnOK(ex.Message);
           }
        };

            

        #endregion

    }
}
