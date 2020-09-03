using Autofac;
using GAT_Produkcja.Services.CustomPinMessageBox;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.UI.ViewModel.Login;
using GAT_Produkcja.ViewModel._Test;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Naglowek;
using GAT_Produkcja.ViewModel.Konfiguracja.KonfiguracjaUrzadzen;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;

namespace GAT_Produkcja.Startup
{
    public static class StartUpView
    {
        public static Window GetStartUpWindow()
        {
            return IoC.Container.Resolve<LoginView>();
            //return IoC.Container.Resolve<TestowyView>();
            //return IoC.Container.Resolve<DodajKontrahentaView>();
            //return IoC.Container.Resolve<MainMenuView>();
            //return IoC.Container.Resolve<ProdukcjaEwidencjaView>();
            //return IoC.Container.Resolve<MagazynEwidencjaSubiektView>();
            //return IoC.Container.Resolve<FinanseZobowiazaniaNaleznosciView>();
            //return IoC.Container.Resolve<SprzedazEwidencjaView>();
            //return IoC.Container.Resolve<DodajCenyTransferoweView>();
            //return IoC.Container.Resolve<EwidencjaCenTransferowychView>();
            //return IoC.Container.Resolve<PodsumowanieFinansoweView>();
            //return IoC.Container.Resolve<DodajStanKontaView>();
            //return IoC.Container.Resolve<EwidencjaStanKontView>();

            #region Zlecenia
            //return IoC.Container.Resolve<MagazynStanTowaruView>();
            //return IoC.Container.Resolve<ZlecenieProdukcyjneEwidencjaView>();
            //return IoC.Container.Resolve<ZlecenieProdukcyjneNaglowekView>();
            //return IoC.Container.Resolve<ZlecenieCieciaEwidencjaView>();
            //return IoC.Container.Resolve<ZlecenieCieciaNaglowekView>();
            #endregion

            #region Rozliczenie produkcji
            return IoC.Container.Resolve<RozliczenieMsAccessView>();
            //return IoC.Container.Resolve<RozliczenieMsAccessEwidencjaView>();

            #endregion

            #region RejestracjaGniazd
            return IoC.Container.Resolve<GPRuchNaglowekView>();
            //return IoC.Container.Resolve<GPRuchTowarDodajView>();
            //return IoC.Container.Resolve<ProdukcjaEwidencjaView>();
            //return IoC.Container.Resolve<ProdukcjaRuchEwidencjaView>();
            //return IoC.Container.Resolve<ProdukcjaRuchEwidencjaPaletView>();
            return IoC.Container.Resolve<GPRuchTowarDodajView>();
            #endregion

            //return IoC.Container.Resolve<CustomPinMessageBoxView>();

            #region Kontrahent
            //return IoC.Container.Resolve<EwidencjaKontrahentowView>();
            //return IoC.Container.Resolve<DodajKontrahentaView>();
            #endregion

            //return IoC.Container.Resolve<EwidencjaPrzerobProdukcjaGeokomorkaView>();

            #region Konfiguracja
            return IoC.Container.Resolve<KonfiguracjaUrzadzenView>();

            #endregion
        }
    }
}
