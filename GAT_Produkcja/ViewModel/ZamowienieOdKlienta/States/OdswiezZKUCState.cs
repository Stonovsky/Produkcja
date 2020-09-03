using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.MainMenu.Messages;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using System;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.States
{
    public class OdswiezZKUCState : IZKUCState
    {
        private ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel szczegolyVM;
        private readonly IZamOdKlientaFiltrHelper filtr;

        public OdswiezZKUCState(ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel ewidencjaSzczegolyUCViewModel,
                                IZamOdKlientaFiltrHelper filtr)
        {
            this.szczegolyVM = ewidencjaSzczegolyUCViewModel;
            this.filtr = filtr;

            szczegolyVM.Messenger.Register<OdswiezMainMenuMessage>(szczegolyVM, Odswiez);
        }

        private async void Odswiez(OdswiezMainMenuMessage obj)
        {
            szczegolyVM.ListaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>(
                                            await filtr.FiltrujAsync(null));
        }

        #region Old

        //public void ZarejesturjMessengery<T>(Action<T> action)
        //{
        //    var odswiezAction = action as Action<OdswiezMainMenuMessage>;
        //    if (odswiezAction is null) return;

        //    szczegolyVM.Messenger.Register<OdswiezMainMenuMessage>(this, odswiezAction);
        //} 
        #endregion
    }
}
