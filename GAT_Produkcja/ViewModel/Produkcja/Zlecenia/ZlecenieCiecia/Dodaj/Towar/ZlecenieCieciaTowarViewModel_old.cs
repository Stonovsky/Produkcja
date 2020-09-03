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

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar
{
    [AddINotifyPropertyChangedInterface]

    public class ZlecenieCieciaTowarViewModel_old : SaveDeleteMethodViewModelBase, IZlecenieCieciaTowarViewModel
    {
        #region Fields


        #endregion

        #region Properties
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaTowarow { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public ObservableCollection<tblProdukcjaZlecenieTowar> ListaTowarowOrg { get; set; } = new ObservableCollection<tblProdukcjaZlecenieTowar>();
        public tblProdukcjaZlecenieTowar WybranyTowar { get; set; }
        #endregion


        #region Commands
        public RelayCommand DodajTowarCommand { get; set; }
        public RelayCommand UsunTowarCommand { get; set; }
        public RelayCommand EdytujTowarCommand { get; set; }
        #endregion

        public ZlecenieCieciaTowarViewModel_old(IViewModelService viewModelService) : base(viewModelService)
        {
            DodajTowarCommand = new RelayCommand(DodajTowarCommandExecute);
            UsunTowarCommand = new RelayCommand(UsunTowarCommandExecute);
            EdytujTowarCommand = new RelayCommand(EdytujTowarCommandExecute);

            Messenger.Register<ProdukcjaZlecenieDodajTowarMessage>(this, GdyPrzeslanoZlecenieCieciaTowar);
        }

        private void EdytujTowarCommandExecute()
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
            //Messenger.Send(new EdytujTowarMessage
            //{
            //    Towar = WybranyTowar
            //});
            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>
            (new ProdukcjaZlecenieDodajTowarMessage
            {
                ZlecenieTowar = WybranyTowar,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj,
            });

        }

        private async void UsunTowarCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć pozycję?"))
                return;

            if (WybranyTowar.IDProdukcjaZlecenieTowar == 0)
                ListaTowarow.Remove(WybranyTowar);
            else
            {
                ListaTowarow.Remove(WybranyTowar);
                UnitOfWork.tblProdukcjaZlecenieTowar.Remove(WybranyTowar);
                await UnitOfWork.SaveAsync();
            }
        }

        private async void GdyPrzeslanoZlecenieCieciaTowar(ProdukcjaZlecenieDodajTowarMessage obj)
        {
            if (obj is null) return;
            if (obj.ZlecenieTowar is null) return;
            if (obj.ZlecenieTowar.IDProdukcjaGniazdoProdukcyjne != (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji) return;


            if (obj.DodajUsunEdytujEnum == DodajUsunEdytujEnum.Dodaj)
            {
                DodajTowar(obj);
            }
            else if (obj.DodajUsunEdytujEnum == DodajUsunEdytujEnum.Usun)
            {
                await UsunTowar(obj);
            }
        }

        private void DodajTowar(ProdukcjaZlecenieDodajTowarMessage obj)
        {
            if (obj.ZlecenieTowar.IDProdukcjaZlecenieTowar == 0)
                ListaTowarow.Add(obj.ZlecenieTowar);
            else
            {
                var towarDoEdycji = ListaTowarow.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == obj.ZlecenieTowar.IDProdukcjaZlecenieTowar);

                if (towarDoEdycji is null) return;

                towarDoEdycji.Dlugosc_m = obj.ZlecenieTowar.Dlugosc_m;
                towarDoEdycji.Szerokosc_m = obj.ZlecenieTowar.Szerokosc_m;
                towarDoEdycji.Ilosc_m2 = obj.ZlecenieTowar.Ilosc_m2;
                towarDoEdycji.Ilosc_szt = obj.ZlecenieTowar.Ilosc_szt;
                towarDoEdycji.CzyKalandrowana = obj.ZlecenieTowar.CzyKalandrowana;
                towarDoEdycji.CzyUv = obj.ZlecenieTowar.CzyUv;
                towarDoEdycji.IDTowarGeowlokninaParametryGramatura = obj.ZlecenieTowar.IDTowarGeowlokninaParametryGramatura;
                towarDoEdycji.IDTowarGeowlokninaParametrySurowiec = obj.ZlecenieTowar.IDTowarGeowlokninaParametrySurowiec;
                towarDoEdycji.IDTowar = obj.ZlecenieTowar.IDTowar;

                towarDoEdycji.tblTowarGeowlokninaParametryGramatura = obj.ZlecenieTowar.tblTowarGeowlokninaParametryGramatura;
                towarDoEdycji.tblTowarGeowlokninaParametrySurowiec = obj.ZlecenieTowar.tblTowarGeowlokninaParametrySurowiec;
                //towarDoEdycji.tblTowar = obj.ZlecenieTowar.tblTowar;
                towarDoEdycji.tblProdukcjaZlecenieCiecia = obj.ZlecenieTowar.tblProdukcjaZlecenieCiecia;

            }
        }

        private async Task UsunTowar(ProdukcjaZlecenieDodajTowarMessage obj)
        {
            if (obj.ZlecenieTowar.IDProdukcjaZlecenieTowar == 0)
            {
                var towaryDoUsuniecia = ListaTowarow.Where(s => s.IDProdukcjaZlecenieTowar == 0);
                
                towaryDoUsuniecia
                    .ToList()
                    .ForEach(towarDoUsuniecia => ListaTowarow.Remove(towarDoUsuniecia));
            }
            else
            {
                var towarDoUsuniecia = ListaTowarow.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == obj.ZlecenieTowar.IDProdukcjaZlecenieTowar);
                ListaTowarow.Remove(towarDoUsuniecia);

                UnitOfWork.tblProdukcjaZlecenieTowar.Remove(towarDoUsuniecia);
                await UnitOfWork.SaveAsync();
            }
        }

        private void GdyPrzeslanoZlecenieCieciaTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null || obj.IDProdukcjaZlecenieTowar == 0)
                return;

            ListaTowarow.Add(obj);
        }

        private void DodajTowarCommandExecute()
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
        }

        public override bool IsChanged => !ListaTowarow.Compare(ListaTowarowOrg);

        public override bool IsValid => ListaTowarow.Where(t => t.IsValid == false).ToList().Count() == 0;
        
        #region DeleteAsync
        public override async Task DeleteAsync(int idZlecenieCiecia)
        {
            if (idZlecenieCiecia == 0) return;

            var listaTowarowDoUsuniecia = ListaTowarow.Where(t => t.IDProdukcjaZlecenieCiecia == idZlecenieCiecia);

            if (listaTowarowDoUsuniecia == null) return;

            UsunZListyTowarow(idZlecenieCiecia);
            await UsunZBazy(listaTowarowDoUsuniecia);


        }

        private async Task UsunZBazy(IEnumerable<tblProdukcjaZlecenieTowar> listaTowarowDoUsuniecia)
        {
            UnitOfWork.tblProdukcjaZlecenieTowar.RemoveRange(listaTowarowDoUsuniecia);
            await UnitOfWork.SaveAsync();
        }

        private void UsunZListyTowarow(int idZlecenieCiecia)
        {
            ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>(
                ListaTowarow.Where(t => t.IDProdukcjaZlecenieCiecia != idZlecenieCiecia).ToList());
        }

        #endregion
        public override void IsChanged_False()
        {
            ListaTowarowOrg = ListaTowarow.DeepClone();
        }

        #region LoadAsync

        public override async Task LoadAsync(int? idZlecenieCiecia)
        {
            if (idZlecenieCiecia is null || idZlecenieCiecia == 0)
                return;

            ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
                (await UnitOfWork.tblProdukcjaZlecenieTowar
                                .WhereAsync(t => t.IDProdukcjaZlecenieCiecia == idZlecenieCiecia.Value));

            PrzypiszWartosciDoPolNiemapowanych(ListaTowarow);
        }

        private void PrzypiszWartosciDoPolNiemapowanych(ObservableCollection<tblProdukcjaZlecenieTowar> listaTowarow)
        {
            foreach (var towar in listaTowarow)
            {
                towar.Gramatura = towar.tblTowarGeowlokninaParametryGramatura.Gramatura;
                towar.Surowiec = towar.tblTowarGeowlokninaParametrySurowiec.Skrot;
            }

        }

        #endregion

        #region SaveAsync
        public override async Task SaveAsync(int? idZlecenie)
        {
            if (idZlecenie is null || idZlecenie == 0)
                return;

            UzupelnijEncje(idZlecenie);
            DodajTowaryZIdZero();
            await UnitOfWork.SaveAsync();
        }

        private void DodajTowaryZIdZero()
        {
            var towaryZIdZero = ListaTowarow.Where(t => t.IDProdukcjaZlecenieTowar == 0);
            if (towaryZIdZero.Count() > 0)
                UnitOfWork.tblProdukcjaZlecenieTowar.AddRange(towaryZIdZero);
        }

        private void UzupelnijEncje(int? idZlecenie)
        {
            ListaTowarow.ToList().ForEach(t =>
            {
                t.IDProdukcjaZlecenieCiecia = idZlecenie.GetValueOrDefault();
                t.IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji;
            });
        }
        #endregion
    }
}
