using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki;
using GAT_Produkcja.UI.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.RozliczenieFV.Szczegoly
{
    public class ZapotrzebowanieRozliczenieFVSzczegolyViewModel : ViewModelBase
    {
        #region Properties
        private vwFVKosztowezSubiektGT rozliczenieFV;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IPlikiCRUD plikiCRUD;
        private readonly IMessenger messenger;
        private ObservableCollection<tblPliki> listaPlikow;
        private tblPliki wybranyPlik;

        private string tytul;

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        public vwFVKosztowezSubiektGT RozliczenieFV
        {
            get { return rozliczenieFV; }
            set { rozliczenieFV = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<tblPliki> ListaPlikow
        {
            get { return listaPlikow; }
            set { listaPlikow = value; RaisePropertyChanged(); }
        }

        public tblPliki WybranyPlik
        {
            get { return wybranyPlik; }
            set { wybranyPlik = value; RaisePropertyChanged(); }
        }

        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand UsunPlikCommand { get; set; }
        public RelayCommand DodajPlikCommand { get; set; }
        public RelayCommand OtworzPlikCommand { get; set; }
        public RelayCommand OnWindowLoadedCommand { get; set; }
        #endregion

        #region CTOR
        public ZapotrzebowanieRozliczenieFVSzczegolyViewModel(IUnitOfWork unitOfWork,
                                                                IDialogService dialogService,
                                                                IViewService viewService,
                                                                IPlikiCRUD plikiCRUD,
                                                                IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.plikiCRUD = plikiCRUD;
            this.messenger = messenger;
            ListaPlikow = new ObservableCollection<tblPliki>();

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            DodajPlikCommand = new RelayCommand(DodajPlikCommandExecute);
            UsunPlikCommand = new RelayCommand(UsunPlikCommandExecute);
            OtworzPlikCommand = new RelayCommand(OtworzPlikCommandExecute);
            OnWindowLoadedCommand = new RelayCommand(OnWindowLoadedCommandExecute);
            messenger.Register<vwFVKosztowezSubiektGT>(this, GdyPrzeslanoRozliczenie);
        }
        #endregion


        private void OnWindowLoadedCommandExecute()
        {
            Tytul = "Rozliczenie zapotrzebowań - faktury VAT";
        }
        
        #region Usun
        private bool UsunCommandCanExecute()
        {
            if (ListaPlikow == null)
            {
                return false;
            }

            if (ListaPlikow.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć wszystkie pliki do tego rozliczenia zapotrzebowania?" +
                                                        "\r\n" +
                                                        "UWAGA!: niektóre pliki faktur mogą dotyczyć kilku zapotrzebowań!"))
            {
                try
                {
                    unitOfWork.tblPliki.RemoveRange(ListaPlikow);
                    plikiCRUD.UsunPlikZSerwera(ListaPlikow);
                    await unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {

                    dialogService.ShowInfo_BtnOK(ex.Message);
                }
            }
        }

        #endregion

        #region Zapisz
        private bool ZapiszCommandCanExecute()
        {
            if (ListaPlikow == null)
            {
                return false;
            }

            if (ListaPlikow.Count > 0 && ListaPlikow.Where(l => l.IDPlik == 0).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void ZapiszCommandExecute()
        {
            foreach (var plik in ListaPlikow)
            {
                if (plik.IDPlik == 0)
                {
                    plik.NrFvKlienta = RozliczenieFV.NrFVKlienta;
                    unitOfWork.tblPliki.Add(plik);
                    plikiCRUD.KopiujPlikNaSerwer(plik);
                }
            }

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK(ex.Message);
            }
            viewService.Close<ZapotrzebowanieRozliczenieFVSzczegolyViewModel>();
        }

        #endregion

        #region Pliki
        private void DodajPlikCommandExecute()
        {
            var listaPlikowDoDodania = plikiCRUD.PobierzListePlikowDoDodania(rozliczenieFV);

            if (listaPlikowDoDodania==null)
                return;

            if (listaPlikowDoDodania.Count > 0)
            {
                foreach (var plik in listaPlikowDoDodania)
                {
                    ListaPlikow.Add(plik);
                }
            }
        }
        private async void UsunPlikCommandExecute()
        {
            if (WybranyPlik == null)
                return;

            if (WybranyPlik.IDPlik == 0)
            {
                ListaPlikow.Remove(WybranyPlik);
            }
            else
            {
                try
                {
                    unitOfWork.tblPliki.Remove(WybranyPlik);
                    await unitOfWork.SaveAsync();

                    List<tblPliki> lista = new List<tblPliki>();
                    lista.Add(WybranyPlik);
                    plikiCRUD.UsunPlikZSerwera(lista);
                    ListaPlikow.Remove(WybranyPlik);
                }
                catch (Exception ex)
                {
                    dialogService.ShowInfo_BtnOK(ex.Message);
                }
            }
        }

        private void OtworzPlikCommandExecute()
        {
            if (WybranyPlik != null)
            {
                if (WybranyPlik.IDPlik == 0)
                {
                    System.Diagnostics.Process.Start(WybranyPlik.SciezkaLokalnaPliku);
                }
                else
                {
                    System.Diagnostics.Process.Start(WybranyPlik.SciezkaPliku);
                }
            }
        }
        #endregion

        private async void GdyPrzeslanoRozliczenie(vwFVKosztowezSubiektGT obj)
        {
            RozliczenieFV = obj;
            if (RozliczenieFV != null)
            {
                var pliki = await unitOfWork.tblPliki.WhereAsync(p => p.NrFvKlienta == RozliczenieFV.NrFVKlienta) as List<tblPliki>;
                ListaPlikow = new ObservableCollection<tblPliki>(pliki);

                Tytul = "Rozliczenie zapotrzebowania. FV klienta nr: " + RozliczenieFV.NrWewnetrznyZobowiazaniaSGT + "\r\n" +
                        "Dla zapotrzebowań: " + RozliczenieFV.NrZP;
            }
        }

    }
}
