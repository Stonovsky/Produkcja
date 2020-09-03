using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja
{
    public class ZlecenieCieciaEwidencjaViewModel : ListCommandViewModelBase
    {
        
        #region Properties
        public IEnumerable<tblProdukcjaZlecenieTowar> ListaZlecenCiecia { get; set; }
        public tblProdukcjaZlecenieTowar WybraneZlecenie { get; set; }

        public string Tytul { get; set; }
        #endregion

        #region Command
        public RelayCommand EdytujCommand { get; set; }
        public RelayCommand DodajCommand { get; set; }
        #endregion
        public ZlecenieCieciaEwidencjaViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {

            EdytujCommand = new RelayCommand(EdytujCommandExecute);
            DodajCommand = new RelayCommand(DodajCommandExecute);

            Messenger.Register<tblProdukcjaZlecenieCiecia>(this, GdyPrzeslanoZlecenieCiecia);
        }

        private async void GdyPrzeslanoZlecenieCiecia(tblProdukcjaZlecenieCiecia obj)
        {
            await PobierzListeZlecen();
        }

        private void DodajCommandExecute()
        {
            ViewService.Show<ZlecenieCieciaNaglowekViewModel_old>();
        }

        private void EdytujCommandExecute()
        {
            ViewService.Show<ZlecenieCieciaNaglowekViewModel_old>();
            if (WybraneZlecenie != null)
            { 
                Messenger.Send(WybraneZlecenie.tblProdukcjaZlecenieCiecia);
                Messenger.Send(WybraneZlecenie);
            }
        }


        protected override async void LoadCommandExecute()
        {
            Tytul = "Ewidencja zleceń cięcia";
            await PobierzListeZlecen();
        }

        private async Task PobierzListeZlecen()
        {
            ListaZlecenCiecia = await UnitOfWork.tblProdukcjaZlecenieTowar.WhereAsync(t=>t.IDProdukcjaZlecenieCiecia>0);
        }
    }
}
