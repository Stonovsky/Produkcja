using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaKalandra;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar.LiniaWloknin;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek
{

    [AddINotifyPropertyChangedInterface]

    public class ZlecenieProdukcyjneNaglowekViewModel
        : AddEditCommandGenericViewModelBase<tblProdukcjaZlecenie>
    {
        #region Fields
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        #endregion

        #region Properties
        public IZlecenieProdukcyjneMieszankaViewModel MieszankaViewModel { get; }
        public IZlecenieProdukcyjneTowarLiniaWlokninViewModel LiniaWlokninViewModel { get; }
        public IZlecenieProdukcyjneTowarLiniaKalandraViewModel LiniaKalandraViewModel { get; }
        public IZlecenieProdukcyjneTowarViewModel TowarViewModel { get; }

        public IEnumerable<tblProdukcjaZlecenieStatus> ListaStatusowZlecen { get; set; }
        public tblProdukcjaZlecenieStatus WybranyStatusZlecenia { get; set; }
        public override bool IsValid => base.IsValid;
        //&& MieszankaViewModel.IsValid;


        #endregion

        public ZlecenieProdukcyjneNaglowekViewModel(IViewModelService viewModelService,
                                                    IZlecenieProdukcyjneMieszankaViewModel zlecenieProdukcyjneMieszankaViewModel,
                                                    IZlecenieProdukcyjneTowarLiniaWlokninViewModel liniaWlokninViewModel,
                                                    IZlecenieProdukcyjneTowarLiniaKalandraViewModel liniaKalandraViewModel)
            : base(viewModelService)
        {
            MieszankaViewModel = zlecenieProdukcyjneMieszankaViewModel;
            LiniaWlokninViewModel = liniaWlokninViewModel;
            LiniaKalandraViewModel = liniaKalandraViewModel;

            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowar);
            //Messenger.Register<EdytujTowarMessage>(this, GdyPrzeslanoEdycjaMessage);

            Title = "Nowe zlecenie produkcyjne";
        }

        public override IGenericRepository<tblProdukcjaZlecenie> Repository => UnitOfWork.tblProdukcjaZlecenie;

        #region Delegates

        public override Func<tblProdukcjaZlecenie, int> GetIdFromEntityWhenSentByMessenger => (obj) => obj.IDProdukcjaZlecenie;

        protected override Func<int> GetVMEntityId => () => VMEntity.IDProdukcjaZlecenie;

        protected override void OnElementSent(tblProdukcjaZlecenie obj)
        {
            base.OnElementSent(obj);

            VMEntity.WartoscMieszanki_zl = obj.WartoscMieszanki_zl;
            VMEntity.CenaMieszanki_zl = obj.CenaMieszanki_zl;
            VMEntity.UdzialSurowcowWMieszance = obj.UdzialSurowcowWMieszance;
        }

        private void GdyPrzeslanoZlecenieTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            zlecenieTowar = obj;
        }

        #endregion        


        #region LoadCommand

        protected override async void LoadCommandExecute()
        {
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            await Task.Delay(200);
            await PobierzDaneWspolne();
            if (zlecenieTowar is null)
            {
                await GenerujNoweZlecenieProdukcyjne();
            }
            else
            {
                await PobierzZlecenieProdukcyjneDoEdycji();
            }

            IsChanged_False();
        }

        private async Task PobierzDaneWspolne()
        {
            ListaStatusowZlecen = await UnitOfWork.tblProdukcjaZlecenieStatus.GetAllAsync();
        }

        private async Task PobierzZlecenieProdukcyjneDoEdycji()
        {
            VMEntity = await UnitOfWork.tblProdukcjaZlecenie
                                        .SingleOrDefaultAsync(z => z.IDProdukcjaZlecenie == zlecenieTowar.IDProdukcjaZlecenie);
            WybranyStatusZlecenia = ListaStatusowZlecen.SingleOrDefault(s => s.IDProdukcjaZlecenieStatus == VMEntity.IDProdukcjaZlecenieStatus);

            await WywolajLoadAsyncNaZaleznychViewModelach();

            Title = $"Edycja zlecenia produkcyjnego nr {VMEntity?.NrDokumentu}";
        }

        private async Task WywolajLoadAsyncNaZaleznychViewModelach()
        {
            await LiniaWlokninViewModel.LoadAsync(VMEntity.IDProdukcjaZlecenie);
            await LiniaKalandraViewModel.LoadAsync(VMEntity.IDProdukcjaZlecenie);
            await MieszankaViewModel.LoadAsync(VMEntity.IDProdukcjaZlecenie);
        }

        private async Task GenerujNoweZlecenieProdukcyjne()
        {
            VMEntity.IDZlecajacy = UzytkownikZalogowany.Uzytkownik is null ? 7 : UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT; // ID 7 to Tomasz Strączek - na potrzeby testow
            VMEntity.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
            VMEntity.NrZlecenia = await PobierzNrZleceniaAsync();
            VMEntity.NrDokumentu = await PobierzNrDokumentu();
            VMEntity.KodKreskowy = VMEntity.NrDokumentu?.Replace("/", string.Empty);


            Title = $"Nowe zlecenie produkcyjne, nr: {VMEntity.NrDokumentu}";
        }
        /// <summary>
        /// Pobiera nowy, pelny numer dokumentu z bazy
        /// </summary>
        /// <returns></returns>
        private async Task<string> PobierzNrDokumentu()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                return await uow.tblProdukcjaZlecenie
                .GetNewFullNumberAsync(t => t.DataRozpoczecia.Value.Year == DateTime.Now.Year,
                                     t => t.NrZlecenia.Value, "ZLP");
            }
        }
        /// <summary>
        /// Pobiera nowy nr zlecenia z bazy
        /// </summary>
        /// <returns></returns>
        private async Task<int> PobierzNrZleceniaAsync()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                return await uow.tblProdukcjaZlecenie
                    .GetNewNumberAsync(t => t.DataRozpoczecia.Value.Year == DateTime.Now.Year,
                                         t => t.NrZlecenia.Value);
            }
        }

        #endregion

        #region SaveCommand


        #region CanExecute
        protected override bool SaveCommandCanExecute()
        {
            return base.SaveCommandCanExecute()
                && (LiniaWlokninViewModel.ListOfVMEntities.Count() > 0
                  || LiniaKalandraViewModel.ListOfVMEntities.Count() > 0);
        }
        #endregion

        #region Execute
        protected override Func<Task> UpdateEntityBeforeSaveAction => async () =>
        {
            VMEntity.DataUtworzenia = DateTime.Now;
            VMEntity.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
        };

        protected override Func<Task> SaveAdditional => async () =>
        {
            await MieszankaViewModel.SaveAsync(VMEntity.IDProdukcjaZlecenie);
            await LiniaWlokninViewModel.SaveAsync(VMEntity.IDProdukcjaZlecenie);
            await LiniaKalandraViewModel.SaveAsync(VMEntity.IDProdukcjaZlecenie);
        };
        #endregion

        #endregion


    }
}
