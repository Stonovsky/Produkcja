using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.BaseClass
{
    public abstract class ZlecenieProdukcyjneTowarBase
        : ListAddEditDeleteMethodGenericViewModelBase<tblProdukcjaZlecenieTowar>
    {
        #region Fields
        protected abstract GniazdaProdukcyjneEnum gniazdoProdukcyjne { get; }
        private int? idZlecenieProdukcyjne;
        #endregion

        #region Properties

        public abstract override string Title { get; }
        public override IGenericRepository<tblProdukcjaZlecenieTowar> Repository => UnitOfWork.tblProdukcjaZlecenieTowar;


        #endregion

        public ZlecenieProdukcyjneTowarBase(IViewModelService viewModelService)
            : base(viewModelService)
        {
            ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>();
            Messenger.Register<ProdukcjaZlecenieDodajTowarMessage>(this, GdyPrzeslanoDodajMessage);
            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoTowar);

        }

        private void GdyPrzeslanoTowar(tblProdukcjaZlecenieTowar obj)
        {
            OstatnioDodanyTowar = obj;
        }

        public override Func<tblProdukcjaZlecenieTowar, int> GetElementId => (obj) => obj.IDProdukcjaZlecenieTowar;

        #region Add
        public override Action ShowAddEditWindow => () =>
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
            ProdukcjaZlecenieDodajTowarMessage messageToSend = CreateMessageToSend();

            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>(messageToSend);

        };

        private ProdukcjaZlecenieDodajTowarMessage CreateMessageToSend()
        {
            var messageToSend = new ProdukcjaZlecenieDodajTowarMessage
            {
                GniazdaProdukcyjneEnum = gniazdoProdukcyjne,
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
            };
            if (gniazdoProdukcyjne == GniazdaProdukcyjneEnum.LiniaDoKalandowania)
            {
                messageToSend.ZlecenieTowar = OstatnioDodanyTowar.DeepClone();
            }

            return messageToSend;
        }

        #endregion
        
        #region Edit
        protected override void EditCommandExecute()
        {
            ViewService.Show<ZlecenieDodajTowarViewModel>();
            Messenger.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>
                        (new ProdukcjaZlecenieDodajTowarMessage
                        {
                            ZlecenieTowar = SelectedVMEntity,
                            GniazdaProdukcyjneEnum = gniazdoProdukcyjne,
                            DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj,
                        });
        }
        #endregion

        #region Load

        public override async Task LoadAsync(int? idZlecenieProdukcyjne)
        {
            this.idZlecenieProdukcyjne = idZlecenieProdukcyjne;
            if (idZlecenieProdukcyjne is null) return;

            ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
                    (
                    await UnitOfWork.tblProdukcjaZlecenieTowar
                        .WhereAsync(t => t.IDProdukcjaZlecenie == idZlecenieProdukcyjne
                                      && t.IDProdukcjaGniazdoProdukcyjne == (int)gniazdoProdukcyjne)
                    );

            PrzypiszPolaNieMapowane(ListOfVMEntities);
        }
        private void PrzypiszPolaNieMapowane(ObservableCollection<tblProdukcjaZlecenieTowar> listaParametrowLiniaWloknin)
        {
            foreach (var towar in listaParametrowLiniaWloknin)
            {
                towar.Gramatura = towar.tblTowarGeowlokninaParametryGramatura?.Gramatura;
                towar.Surowiec = towar.tblTowarGeowlokninaParametrySurowiec?.Skrot;
            }
        }

        #endregion

        #region Messenger
        private void GdyPrzeslanoDodajMessage(ProdukcjaZlecenieDodajTowarMessage dodajMessage)
        {
            ViewService.Close<ZlecenieDodajTowarViewModel>();

            if (dodajMessage is null || dodajMessage.ZlecenieTowar is null) return;

            if (dodajMessage.GniazdaProdukcyjneEnum == gniazdoProdukcyjne)
                ListOfVMEntities.Add(dodajMessage.ZlecenieTowar);

            UstalOstatnioDodanyTowar(dodajMessage);
        }

        private void UstalOstatnioDodanyTowar(ProdukcjaZlecenieDodajTowarMessage dodajMessage)
        {
            if (dodajMessage.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                OstatnioDodanyTowar = dodajMessage.ZlecenieTowar;
            }
        }
        #endregion

        #region Save
        protected override Action<int?> UpdateEntityBeforeSaveAction => (id) =>
        {
            foreach (var entity in ListOfVMEntities)
            {
                entity.IDProdukcjaZlecenie = id;
                entity.IDProdukcjaGniazdoProdukcyjne = (int)gniazdoProdukcyjne;
                entity.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
            }
        };

        public tblProdukcjaZlecenieTowar OstatnioDodanyTowar { get; private set; }

        #endregion
    }
}
