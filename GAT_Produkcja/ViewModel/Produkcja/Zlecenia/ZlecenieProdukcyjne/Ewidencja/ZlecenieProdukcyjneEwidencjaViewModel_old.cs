using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja
{
    public class ZlecenieProdukcyjneEwidencjaViewModel_old : ListCommandViewModelBase
    {
        #region Fields

        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne;

        #endregion

        #region Properties
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaTowarowZlecenProdukcyjnych { get; set; }
        public tblProdukcjaZlecenieTowar WybranyTowarZleceniaProdukcyjnego { get; set; }

        public string Tytul { get; set; }
        #endregion

        #region Commands
        public RelayCommand DodajCommand { get; set; }
        public RelayCommand EdytujCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        #endregion

        #region CTOR

        public ZlecenieProdukcyjneEwidencjaViewModel_old(IViewModelService viewModelService)
            : base(viewModelService)
        {

            DodajCommand = new RelayCommand(DodajCommandExecute);
            EdytujCommand = new RelayCommand(EdytujCommandExecute, EdytujCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);

            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenie);
            Messenger.Register<tblProdukcjaZlecenie>(this, GdyPrzeslanoZlecenie);
            Messenger.Register<tblProdukcjaGniazdoProdukcyjne>(this, GdyPrzeslanoGniazdoProdukcyjne);

            Tytul = "Ewidencja zleceń produkcyjnych";
        }

        private void GdyPrzeslanoGniazdoProdukcyjne(tblProdukcjaGniazdoProdukcyjne obj)
        {
            if (obj is null) return;

            gniazdoProdukcyjne = obj;

            LoadCommandExecute();
        }

        private void GdyPrzeslanoZlecenie(tblProdukcjaZlecenie obj)
        {
            LoadCommandExecute();
        }
        private void GdyPrzeslanoZlecenie(tblProdukcjaZlecenieTowar obj)
        {
            LoadCommandExecute();
        }
        #region UsunCommand

        private async void UsunCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć całe zlecenie produkcyjne?"))
                return;

            var towar = await UnitOfWork.tblProdukcjaZlecenieTowar
                                    .WhereAsync(t => t.IDProdukcjaZlecenie == WybranyTowarZleceniaProdukcyjnego.IDProdukcjaZlecenie);

            var zlecenieProdukcyjne = await UnitOfWork.tblProdukcjaZlecenie
                                    .SingleOrDefaultAsync(z => z.IDProdukcjaZlecenie == WybranyTowarZleceniaProdukcyjnego.IDProdukcjaZlecenie);

            var mieszanka = await UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                    .WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == WybranyTowarZleceniaProdukcyjnego.IDProdukcjaZlecenie);

            UnitOfWork.tblProdukcjaZlecenieTowar.RemoveRange(towar);
            UnitOfWork.tblProdukcjaZlecenie.Remove(zlecenieProdukcyjne);
            UnitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.RemoveRange(mieszanka);
            await UnitOfWork.SaveAsync();

            DialogService.ShowInfo_BtnOK("Zlecenie zostało usunięte");
            Messenger.Send(new tblProdukcjaZlecenieTowar());
        }

        private bool UsunCommandCanExecute()
        {
            return WybranyTowarZleceniaProdukcyjnego != null;
        }


        #endregion
        private void EdytujCommandExecute()
        {
            ViewService.Show<ZlecenieProdukcyjneNaglowekViewModel_old>();
            Messenger.Send<EdytujTowarMessage, ZlecenieProdukcyjneNaglowekViewModel_old>(new EdytujTowarMessage
            {
                Towar = WybranyTowarZleceniaProdukcyjnego
            });
            Messenger.Send(WybranyTowarZleceniaProdukcyjnego);
        }

        private bool EdytujCommandCanExecute()
        {
            return WybranyTowarZleceniaProdukcyjnego != null;
        }

        private void DodajCommandExecute()
        {
            ViewService.Show<ZlecenieProdukcyjneNaglowekViewModel_old>();
        }


        #endregion


        protected override async void LoadCommandExecute()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                if (gniazdoProdukcyjne == null)
                {
                    ListaTowarowZlecenProdukcyjnych = new ObservableCollection<tblProdukcjaZlecenieTowar>
                        (
                            await uow.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie != null
                                                                                    && t.IDProdukcjaZlecenie != 0)
                        );
                }
                else
                {
                    ListaTowarowZlecenProdukcyjnych = new ObservableCollection<tblProdukcjaZlecenieTowar>
                            (
                                await uow.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie != null
                                                                                 && t.IDProdukcjaZlecenie != 0
                                                                                 && t.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne)
                            );

                }
            }
        }
    }
}
