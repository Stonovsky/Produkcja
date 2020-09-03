using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar
{

    public class ZlecenieDodajTowarViewModel
        : AddEditCommandGenericViewModelBase<tblProdukcjaZlecenieTowar>
    {
        #region Fields
        private tblTowarGeowlokninaParametryGramatura wybranaGramatura;
        private tblTowarGeowlokninaParametrySurowiec wybranySurowiec;
        private DodajUsunEdytujEnum status;
        private GniazdaProdukcyjneEnum gniazdoEnum;

        public tblProdukcjaGniazdoProdukcyjne Gniazdo { get; private set; }

        private tblProdukcjaZlecenieTowar przeslanyTowar;
        private vwTowarGTX towar;
        #endregion

        #region Properties

        public bool CzyUV
        {
            get { return VMEntity.CzyUv; }
            set
            {
                VMEntity.CzyUv = value;
            }
        }


        public decimal Szerokosc_m
        {
            get { return VMEntity.Szerokosc_m; }
            set
            {
                VMEntity.Szerokosc_m = value;
            }
        }
        public decimal Dlugosc_m
        {
            get { return VMEntity.Dlugosc_m; }
            set
            {
                VMEntity.Dlugosc_m = value;
            }
        }
        public decimal Ilosc_m2
        {
            get { return VMEntity.Ilosc_m2; }
            set
            {
                VMEntity.Ilosc_m2 = value;
                ObliczWage();
                ObliczIloscSzt();
            }
        }

        private void ObliczWage()
        {
            if (WybranaGramatura is null
                || WybranaGramatura.Gramatura is null)
            {
                VMEntity.Ilosc_kg = 0;
            }
            else
                VMEntity.Ilosc_kg = VMEntity.Ilosc_m2 * WybranaGramatura?.Gramatura.GetValueOrDefault() / 1000;
        }
        private void ObliczIloscSzt()
        {
            if (VMEntity.Ilosc_m2 == 0
                || VMEntity.Szerokosc_m == 0
                || VMEntity.Dlugosc_m == 0) return;

            VMEntity.Ilosc_szt = (int)(Ilosc_m2 / (VMEntity.Szerokosc_m * VMEntity.Dlugosc_m));
        }
        public IEnumerable<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; }

        public tblTowarGeowlokninaParametryGramatura WybranaGramatura
        {
            get => wybranaGramatura;
            set
            {
                wybranaGramatura = value;
                VMEntity.Gramatura = WybranaGramatura?.Gramatura;
            }
        }
        public IEnumerable<tblTowarGeowlokninaParametrySurowiec> ListaSurowcow { get; set; }
        public tblTowarGeowlokninaParametrySurowiec WybranySurowiec
        {
            get => wybranySurowiec;
            set
            {
                wybranySurowiec = value;
                VMEntity.Surowiec = WybranySurowiec?.Skrot;
            }
        }

        public vwTowarGTX Towar
        {
            get => towar;
            set
            {
                towar = value;
                VMEntity.IDTowar = Towar == null ? 0 : Towar.IdTowar;
            }
        }

        public override IGenericRepository<tblProdukcjaZlecenieTowar> Repository => UnitOfWork.tblProdukcjaZlecenieTowar;

        #endregion

        #region CTOR

        public ZlecenieDodajTowarViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
            VMEntity.Ilosc_szt = 1;
        }

        #endregion

        #region Messenger

        #region Registration
        protected override void RegisterMessengers()
        {
            Messenger.Register<ProdukcjaZlecenieDodajTowarMessage>(this, GdyPrzeslanoTowar);
        }
        #endregion

        private void GdyPrzeslanoTowar(ProdukcjaZlecenieDodajTowarMessage obj)
        {
            if (obj is null) return;

            status = obj.DodajUsunEdytujEnum;
            gniazdoEnum = obj.GniazdaProdukcyjneEnum;
            przeslanyTowar = obj.ZlecenieTowar;
        }

        #endregion

        protected override Func<int> GetVMEntityId => () => VMEntity.IDProdukcjaZlecenieTowar;

        public override Func<tblProdukcjaZlecenieTowar, int> GetIdFromEntityWhenSentByMessenger => (e) => e.IDProdukcjaZlecenieTowar;


        private void ObliczIlosc_m2()
        {
            VMEntity.Ilosc_m2 = VMEntity.Szerokosc_m * VMEntity.Dlugosc_m * VMEntity.Ilosc_szt;

        }

        #region Load


        protected override async void LoadCommandExecute()
        {
            ListaGramatur = await UnitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync().ConfigureAwait(false);
            ListaSurowcow = await UnitOfWork.tblTowarGeowlokninaParametrySurowiec.GetAllAsync().ConfigureAwait(false);

            if (gniazdoEnum != 0)
                Gniazdo = await UnitOfWork.tblProdukcjaGniazdoProdukcyjne.GetByIdAsync((int)gniazdoEnum).ConfigureAwait(false);

            CzyKalandrowana();

            if (przeslanyTowar is null)
                return;

            PodczytajDaneZTowaru();
        }

        private void PodczytajDaneZTowaru()
        {
            VMEntity = przeslanyTowar;
            WybranaGramatura = ListaGramatur.SingleOrDefault(g => g.IDTowarGeowlokninaParametryGramatura == przeslanyTowar.IDTowarGeowlokninaParametryGramatura);
            WybranySurowiec = ListaSurowcow.SingleOrDefault(s => s.IDTowarGeowlokninaParametrySurowiec == przeslanyTowar.IDTowarGeowlokninaParametrySurowiec);

        }

        private void CzyKalandrowana()
        {
            if (gniazdoEnum == GniazdaProdukcyjneEnum.LiniaDoKalandowania)
                VMEntity.CzyKalandrowana = true;
        }

        #endregion

        #region Save

        protected override bool SaveCommandCanExecute()
        {
            ObliczWage();
            ObliczIloscSzt();
            return VMEntity.IsValid;
        }

        protected override void SaveCommandExecute()
        {
            if (status == DodajUsunEdytujEnum.Dodaj)
                Messenger.Send(new ProdukcjaZlecenieDodajTowarMessage
                {
                    DodajUsunEdytujEnum = status,
                    GniazdaProdukcyjneEnum = gniazdoEnum,
                    ZlecenieTowar = VMEntity
                });

            ViewService.Close(this.GetType().Name);
        }
        #endregion
    }
}
