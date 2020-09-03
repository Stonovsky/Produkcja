using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar
{

    [AddINotifyPropertyChangedInterface]
    public class ZlecenieProdukcyjneTowarViewModel : SaveDeleteMethodViewModelBase, IZlecenieProdukcyjneTowarViewModel
    {

        #region Properties
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaTowarowLiniaWloknin { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaParametrowLiniaWlokninOrg { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public tblProdukcjaZlecenieTowar WybranyTowarLiniaWloknin { get; set; } = new tblProdukcjaZlecenieTowar();
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaTowarowLiniaKalandra { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaParametrowLiniaKalandraOrg { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public tblProdukcjaZlecenieTowar WybranyTowarLiniaKalandra { get; set; } = new tblProdukcjaZlecenieTowar();
        #endregion

        #region Commands
        public RelayCommand DodajDoLiniiWlokninCommand { get; set; }
        public RelayCommand EdytujTowarLiniiWlokninCommand { get; set; }
        public RelayCommand DodajDoLiniiKalandraCommand { get; set; }
        public RelayCommand EdytujTowarLiniiKalandraCommand { get; set; }
        #endregion

        public ZlecenieProdukcyjneTowarViewModel(IViewModelService viewModelService)
                                            : base(viewModelService)
        {

            DodajDoLiniiWlokninCommand = new RelayCommand(DodajDoLiniiWlokninCommandExecute);
            DodajDoLiniiKalandraCommand = new RelayCommand(DodajDoLiniiKalandraCommandExecute);
            EdytujTowarLiniiWlokninCommand = new RelayCommand(EdytujTowarLiniiWlokninCommandExecute, EdytujTowarLiniiWlokninCommandCanExecute);
            EdytujTowarLiniiKalandraCommand = new RelayCommand(EdytujTowarLiniiKalandraCommandExecute, EdytujTowarLiniiKalandraCommandCanExecute);

            Messenger.Register<ProdukcjaZlecenieDodajTowarMessage>(this, GdyPrzeslanoDodajMessage);
        }

        private void EdytujTowarLiniiKalandraCommandExecute()
        {
            OtworzOknoIWyslijTowarMessengerem(WybranyTowarLiniaKalandra);
        }

        private bool EdytujTowarLiniiKalandraCommandCanExecute()
        {
            return WybranyTowarLiniaKalandra != null;
        }

        private void EdytujTowarLiniiWlokninCommandExecute()
        {
            OtworzOknoIWyslijTowarMessengerem(WybranyTowarLiniaWloknin);
        }

        private void OtworzOknoIWyslijTowarMessengerem(tblProdukcjaZlecenieTowar towar)
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
            Messenger.Send(new EdytujTowarMessage
            {
                Towar = towar
            });

        }

        private bool EdytujTowarLiniiWlokninCommandCanExecute()
        {
            return WybranyTowarLiniaWloknin != null;
        }

        private void GdyPrzeslanoDodajMessage(ProdukcjaZlecenieDodajTowarMessage dodajMessage)
        {
            ViewService.Close<ZlecenieDodajTowarViewModel>();

            if (dodajMessage is null || dodajMessage.ZlecenieTowar is null)
                return;
            //throw new NullReferenceException(nameof(dodajMessage));

            if (dodajMessage.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                ListaTowarowLiniaWloknin.Add(dodajMessage.ZlecenieTowar);
            }
            else if (dodajMessage.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaDoKalandowania)
            {
                ListaTowarowLiniaKalandra.Add(dodajMessage.ZlecenieTowar);
            }

        }

        private void DodajDoLiniiKalandraCommandExecute()
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>(new ProdukcjaZlecenieDodajTowarMessage
            {
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKalandowania,
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
            });

        }

        private void DodajDoLiniiWlokninCommandExecute()
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();

            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>(new ProdukcjaZlecenieDodajTowarMessage
            {
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaWloknin,
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
            });
        }

        public override bool IsChanged => !ListaTowarowLiniaWloknin.Compare(ListaParametrowLiniaWlokninOrg)
                                            || ListaTowarowLiniaKalandra.Compare(ListaParametrowLiniaKalandraOrg);

        public override bool IsValid => ListaParametrowIsValid(ListaTowarowLiniaWloknin)
                                    && ListaParametrowIsValid(ListaTowarowLiniaKalandra);

        public override async Task DeleteAsync(int idZlecenieProdukcyjne)
        {
            var towaryLiniWlokninDoUsuniecia = ListaTowarowLiniaWloknin
                                                    .Where(t => t.IDProdukcjaZlecenie == idZlecenieProdukcyjne)
                                                    .Where(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                                                    .Where(t => t.IDProdukcjaZlecenieTowar != 0);
            var towaryLiniKalandraDoUsuniecia = ListaTowarowLiniaKalandra
                                                    .Where(t => t.IDProdukcjaZlecenie == idZlecenieProdukcyjne)
                                                    .Where(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania)
                                                    .Where(t => t.IDProdukcjaZlecenieTowar != 0);

            if (towaryLiniWlokninDoUsuniecia.Any())
                UnitOfWork.tblProdukcjaZlecenieTowar.RemoveRange(towaryLiniWlokninDoUsuniecia);
            if (towaryLiniKalandraDoUsuniecia.Any())
                UnitOfWork.tblProdukcjaZlecenieTowar.RemoveRange(towaryLiniKalandraDoUsuniecia);

            await UnitOfWork.SaveAsync();
        }

        public override void IsChanged_False()
        {
            ListaParametrowLiniaKalandraOrg = ListaTowarowLiniaKalandra.DeepClone();
            ListaParametrowLiniaWlokninOrg = ListaTowarowLiniaWloknin.DeepClone();
        }

        public override async Task LoadAsync(int? idZlecenieProdukcyjne)
        {
            if (idZlecenieProdukcyjne is null) return;

            ListaTowarowLiniaWloknin = new ObservableCollection<tblProdukcjaZlecenieTowar>
                (
                await UnitOfWork.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie == idZlecenieProdukcyjne
                && t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                );

            PrzypiszPolaNieMapowane(ListaTowarowLiniaWloknin);

            ListaTowarowLiniaKalandra = new ObservableCollection<tblProdukcjaZlecenieTowar>
                (
                await UnitOfWork.tblProdukcjaZlecenieTowar.WhereAsync(t => t.IDProdukcjaZlecenie == idZlecenieProdukcyjne
                && t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania)
                );

            PrzypiszPolaNieMapowane(ListaTowarowLiniaKalandra);

        }

        private void PrzypiszPolaNieMapowane(ObservableCollection<tblProdukcjaZlecenieTowar> listaParametrowLiniaWloknin)
        {
            foreach (var towar in listaParametrowLiniaWloknin)
            {
                towar.Gramatura = towar.tblTowarGeowlokninaParametryGramatura?.Gramatura;
                towar.Surowiec = towar.tblTowarGeowlokninaParametrySurowiec?.Skrot;
            }
        }

        public override async Task SaveAsync(int? idZlecenieProdukcyjne)
        {
            if (idZlecenieProdukcyjne is null) return;

            foreach (var towar in ListaTowarowLiniaWloknin)
            {
                towar.IDProdukcjaZlecenie = idZlecenieProdukcyjne;
                towar.IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin;
            }

            foreach (var towar in ListaTowarowLiniaKalandra)
            {
                towar.IDProdukcjaZlecenie = idZlecenieProdukcyjne;
                towar.IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania;
            }

            ListaTowarowLiniaWloknin.ToList().ForEach(t => t.IDProdukcjaZlecenie = idZlecenieProdukcyjne);

            var listaTowarowWlokninDoDodaniaDoBazy = ListaTowarowLiniaWloknin.Where(t => t.IDProdukcjaZlecenieTowar == 0);
            var listaTowarowKalandraDoDodaniaDoBazy = ListaTowarowLiniaKalandra.Where(t => t.IDProdukcjaZlecenieTowar == 0);

            if (listaTowarowWlokninDoDodaniaDoBazy.Any())
                UnitOfWork.tblProdukcjaZlecenieTowar.AddRange(listaTowarowWlokninDoDodaniaDoBazy);
            if (listaTowarowKalandraDoDodaniaDoBazy.Any())
                UnitOfWork.tblProdukcjaZlecenieTowar.AddRange(listaTowarowKalandraDoDodaniaDoBazy);

            await UnitOfWork.SaveAsync();

        }

        private bool ListaParametrowIsValid(IEnumerable<tblProdukcjaZlecenieTowar> listaParametrow)
        {
            foreach (var parametr in listaParametrow)
            {
                if (!parametr.IsValid)
                    return false;
            }

            return true;
        }
    }
}
