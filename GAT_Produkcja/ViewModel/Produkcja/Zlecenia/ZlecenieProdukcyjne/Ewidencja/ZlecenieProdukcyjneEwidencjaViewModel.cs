using ControlzEx.Standard;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.BaseClasses.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja
{
    public class ZlecenieProdukcyjneEwidencjaViewModel
        : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaZlecenieTowar>
    {
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;
        public override string Title => "Ewidencja zleceń produkcyjnych";
        public override IGenericRepository<tblProdukcjaZlecenieTowar> Repository => UnitOfWork.tblProdukcjaZlecenieTowar;

        #region Commands
        public RelayCommand DodajZlecProdukcyjneCommand { get; set; }
        public RelayCommand DodajZlecCieciaCommand { get; set; }
        #endregion

        public ZlecenieProdukcyjneEwidencjaViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
            DodajZlecProdukcyjneCommand = new RelayCommand(DodajZlecProdukcyjneCommandExecute);
            DodajZlecCieciaCommand = new RelayCommand(DodajZlecCieciaCommandExecute);
        }

        private void DodajZlecCieciaCommandExecute()
        {
            ViewService.Show<ZlecenieCieciaNaglowekViewModel>();
        }

        private void DodajZlecProdukcyjneCommandExecute()
        {
            ViewService.Show<ZlecenieProdukcyjneNaglowekViewModel>();
        }

        #region Messengers

        protected override void RegisterMessengers()
        {
            base.RegisterMessengers();
            Messenger.Register<tblProdukcjaZlecenie>(this, nameof(RefreshListMessage), GdyPrzeslanoZlecenie);
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdoProdukcyjne);

        }

        private void GdyPrzeslanoGniazdoProdukcyjne(tblProdukcjaGniazdoProdukcyjne obj)
        {
            gniazdoProdukcyjne = obj;
        }

        private async void GdyPrzeslanoZlecenie(tblProdukcjaZlecenie obj)
        {
            await LoadAsync();
        }
        #endregion

        #region Delegates
        public override Func<tblProdukcjaZlecenieTowar, int> GetElementSentId => (e) => e.IDProdukcjaZlecenieTowar;

        #endregion

        #region Load
        public async Task LoadAsync()
        {
            await LoadElements();
        }
        protected override async Task LoadElements()
        {
            //zeby message z gniazdem mogl dojsc 
            await Task.Delay(200);

            using (var uow = UnitOfWorkFactory.Create())
            {
                if (gniazdoProdukcyjne == null)
                {
                    ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
                    (
                        await uow.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie != null
                                                                            && t.IDProdukcjaZlecenie != 0)
                    );
                }
                else
                {
                    ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
                    (
                        await uow.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie != null
                                                                         && t.IDProdukcjaZlecenie != 0
                                                                         && t.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne
                                                                         && (t.tblProdukcjaZlecenie.IDProdukcjaZlecenieStatus == (int)ProdukcjaZlecenieStatusEnum.Oczekuje
                                                                             || t.tblProdukcjaZlecenie.IDProdukcjaZlecenieStatus == (int)ProdukcjaZlecenieStatusEnum.WTrakcie)
                                                                         )
                    );
                }
            }
            ListOfVMEntities= new ObservableCollection<tblProdukcjaZlecenieTowar> 
                (ListOfVMEntities.OrderByDescending(d => d.tblProdukcjaZlecenie.NrZlecenia));
        }
        #endregion

        #region AddEdit
        public override Action ShowAddEditWindow => () =>
        {
            ViewService.Show<ZlecenieProdukcyjneNaglowekViewModel>();
        };
        #endregion

        #region Delete
        public override Func<Task> DeleteAction =>
            async () =>
            {
                await UsunWybranyTowar();
                
                if (await CzyZleceniePuste())
                {
                   await  UsunZlecenieProdukcyjne();
                }


                #region Old do usuniecia
                //var zlecenieProdukcyjne = await UnitOfWork.tblProdukcjaZlecenie
                //                          .SingleOrDefaultAsync(z => z.IDProdukcjaZlecenie == SelectedVMEntity.IDProdukcjaZlecenie);
                ////towar dla LiniiWloknin i LiniiKalandra
                //var towar = await UnitOfWork.tblProdukcjaZlecenieTowar
                //                            .WhereAsync(t => t.IDProdukcjaZlecenie == SelectedVMEntity.IDProdukcjaZlecenie);
                //var mieszanka = await UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                //                                .WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == SelectedVMEntity.IDProdukcjaZlecenie);

                //UnitOfWork.tblProdukcjaZlecenieTowar.RemoveRange(towar);
                //UnitOfWork.tblProdukcjaZlecenie.Remove(zlecenieProdukcyjne);
                //UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(mieszanka);

                #endregion
            };

        /// <summary>
        /// Metoda usuwajaca zlecenie produkcyjne wraz z mieszanka
        /// </summary>
        /// <returns><see cref="void"/></returns>
        private async Task UsunZlecenieProdukcyjne()
        {
            var zlecenieProdukcyjne = await UnitOfWork.tblProdukcjaZlecenie
                                          .SingleOrDefaultAsync(z => z.IDProdukcjaZlecenie == SelectedVMEntity.IDProdukcjaZlecenie);
            //towar dla LiniiWloknin i LiniiKalandra
            var mieszanka = await UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                            .WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == SelectedVMEntity.IDProdukcjaZlecenie);

            UnitOfWork.tblProdukcjaZlecenie.Remove(zlecenieProdukcyjne);
            UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(mieszanka);
        }

        /// <summary>
        /// Metoda sprawdzajaca czy zlecenie nie posiada towarow
        /// </summary>
        /// <returns><see cref="bool"></see></returns>
        private async Task<bool> CzyZleceniePuste()
        {
            var towaryDlaZleceniaProdukcyjnego = await UnitOfWork.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie == SelectedVMEntity.IDProdukcjaZlecenie
                                                                                                         && t.IDProdukcjaZlecenieTowar != SelectedVMEntity.IDProdukcjaZlecenieTowar);
            
            if (!towaryDlaZleceniaProdukcyjnego.Any())
                return true;

            return false;
        }
        /// <summary>
        /// Metoda usuwajaca wskazany towar ze zlecenia
        /// </summary>
        /// <returns><see cref="void"/></returns>
        private async Task UsunWybranyTowar()
        {
            var wybranyTowar = await UnitOfWork.tblProdukcjaZlecenieTowar
                            .SingleOrDefaultAsync(t => t.IDProdukcjaZlecenieTowar == SelectedVMEntity.IDProdukcjaZlecenieTowar);
            UnitOfWork.tblProdukcjaZlecenieTowar.Remove(wybranyTowar);
        }


        protected override bool DeleteCommandCanExecute()
        {
            return SelectedVMEntity != null 
                && SelectedVMEntity.IDProdukcjaZlecenieStatus == (int) ProdukcjaZlecenieStatusEnum.Oczekuje;
        }
        #endregion
    }
}
