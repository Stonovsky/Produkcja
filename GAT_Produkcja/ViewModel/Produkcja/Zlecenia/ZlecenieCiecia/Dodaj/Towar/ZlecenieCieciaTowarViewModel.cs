using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar
{
    public class ZlecenieCieciaTowarViewModel 
        : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaZlecenieTowar>
          , IZlecenieCieciaTowarViewModel
    {
        public ZlecenieCieciaTowarViewModel(IViewModelService viewModelService) 
            : base(viewModelService)
        {
            Messenger.Register<ProdukcjaZlecenieDodajTowarMessage>(this, GdyPrzeslanoZlecenieTowar);
        }

        private async void GdyPrzeslanoZlecenieTowar(ProdukcjaZlecenieDodajTowarMessage obj)
        {
            if (obj is null) return;
            if (obj.ZlecenieTowar is null) return;

            //if (obj.ZlecenieTowar.IDProdukcjaGniazdoProdukcyjne != (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji) return;


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
                ListOfVMEntities.Add(obj.ZlecenieTowar);
            else
            {
                var towarDoEdycji = ListOfVMEntities.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == obj.ZlecenieTowar.IDProdukcjaZlecenieTowar);

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
                var towaryDoUsuniecia = ListOfVMEntities.Where(s => s.IDProdukcjaZlecenieTowar == 0);

                towaryDoUsuniecia
                    .ToList()
                    .ForEach(towarDoUsuniecia => ListOfVMEntities.Remove(towarDoUsuniecia));
            }
            else
            {
                var towarDoUsuniecia = ListOfVMEntities.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == obj.ZlecenieTowar.IDProdukcjaZlecenieTowar);
                ListOfVMEntities.Remove(towarDoUsuniecia);

                UnitOfWork.tblProdukcjaZlecenieTowar.Remove(towarDoUsuniecia);
                await UnitOfWork.SaveAsync();
            }
        }


        public override string Title => "Towary dla zlecenia ciecia";

        public override IGenericRepository<tblProdukcjaZlecenieTowar> Repository => UnitOfWork.tblProdukcjaZlecenieTowar;

        public override Func<tblProdukcjaZlecenieTowar, int> GetElementSentId => (e)=> e.IDProdukcjaZlecenieTowar;

        #region AddEdit

        public override Action ShowAddEditWindow => () => ViewService.Show<ZlecenieDodajTowarViewModel>();

        protected override void EditCommandExecute()
        {
            ShowAddEditWindow();

            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>
            (new ProdukcjaZlecenieDodajTowarMessage
            {
                ZlecenieTowar = SelectedVMEntity,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj,
            });
        }

        #endregion

        #region LoadAsync

        public async Task LoadAsync(int? idZlecenieCiecia)
        {
            if (idZlecenieCiecia is null || idZlecenieCiecia == 0)
                return;

            ListOfVMEntities= new ObservableCollection<tblProdukcjaZlecenieTowar>
                (await UnitOfWork.tblProdukcjaZlecenieTowar
                                .WhereAsync(t => t.IDProdukcjaZlecenieCiecia == idZlecenieCiecia.Value));

            PrzypiszWartosciDoPolNiemapowanych(ListOfVMEntities);
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
        public async Task SaveAsync(int? idZlecenie)
        {
            if (idZlecenie is null || idZlecenie == 0)
                return;

            UzupelnijEncje(idZlecenie);
            DodajTowaryZIdZero();
            await UnitOfWork.SaveAsync();
        }

        private void DodajTowaryZIdZero()
        {
            var towaryZIdZero = ListOfVMEntities.Where(t => t.IDProdukcjaZlecenieTowar == 0);
            if (towaryZIdZero.Count() > 0)
                UnitOfWork.tblProdukcjaZlecenieTowar.AddRange(towaryZIdZero);
        }

        private void UzupelnijEncje(int? idZlecenie)
        {
            ListOfVMEntities.ToList().ForEach(t =>
            {
                t.IDProdukcjaZlecenie = idZlecenie.GetValueOrDefault();
                t.IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji;
                t.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
            });
        }
        #endregion

        #region DeleteAsync
        public async Task DeleteAsync(int idZlecenieCiecia)
        {
            if (idZlecenieCiecia == 0) return;

            var listaTowarowDoUsuniecia = ListOfVMEntities.Where(t => t.IDProdukcjaZlecenieCiecia == idZlecenieCiecia);

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
            ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>(
                ListOfVMEntities.Where(t => t.IDProdukcjaZlecenieCiecia != idZlecenieCiecia).ToList());
        }

        #endregion

    }
}
