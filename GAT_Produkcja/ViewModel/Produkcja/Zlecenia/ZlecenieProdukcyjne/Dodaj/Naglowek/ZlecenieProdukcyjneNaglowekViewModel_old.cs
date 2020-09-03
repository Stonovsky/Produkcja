using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Naglowek
{

    [AddINotifyPropertyChangedInterface]

    public class ZlecenieProdukcyjneNaglowekViewModel_old : SaveCommandGenericViewModelBase
    {
        #region Fields
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        #endregion

        #region Properties

        public tblProdukcjaZlecenie ZlecenieProdukcyjne { get; set; } = new tblProdukcjaZlecenie();
        public tblProdukcjaZlecenie ZlecenieProdukcyjneOrg { get; set; } = new tblProdukcjaZlecenie();

        public IZlecenieProdukcyjneMieszankaViewModel MieszankaViewModel { get; }
        public IZlecenieProdukcyjneTowarViewModel TowarViewModel { get; }

        public string Tytul { get; set; }

        #endregion



        public ZlecenieProdukcyjneNaglowekViewModel_old(IViewModelService viewModelService,
                                                    IZlecenieProdukcyjneMieszankaViewModel zlecenieProdukcyjneMieszankaViewModel,
                                                    IZlecenieProdukcyjneTowarViewModel zlecenieProdukcyjneTowarViewModel)
            : base(viewModelService)
        {
            MieszankaViewModel = zlecenieProdukcyjneMieszankaViewModel;
            TowarViewModel = zlecenieProdukcyjneTowarViewModel;

            Messenger.Register<tblProdukcjaZlecenieTowar>(this, GdyPrzeslanoZlecenieTowar);
            Messenger.Register<tblProdukcjaZlecenie>(this, GdyPrzeslanoZlecenieProdukcyjne);
            Messenger.Register<EdytujTowarMessage>(this, GdyPrzeslanoEdycjaMessage);

            Tytul = "Nowe zlecenie produkcyjne";
        }


        #region Delegates
        private void GdyPrzeslanoZlecenieProdukcyjne(tblProdukcjaZlecenie obj)
        {
            if (obj is null) return;

            ZlecenieProdukcyjne.WartoscMieszanki_zl = obj.WartoscMieszanki_zl;
            ZlecenieProdukcyjne.CenaMieszanki_zl = obj.CenaMieszanki_zl;
            ZlecenieProdukcyjne.UdzialSurowcowWMieszance = obj.UdzialSurowcowWMieszance;
        }

        private void GdyPrzeslanoZlecenieTowar(tblProdukcjaZlecenieTowar obj)
        {
            if (obj is null) return;

            zlecenieTowar = obj;
        }
        private async void GdyPrzeslanoEdycjaMessage(EdytujTowarMessage obj)
        {
            if (obj is null) return;
            if (obj.Towar is null) return;

            zlecenieTowar = obj.Towar;
            await PobierzZlecenieProdukcyjneDoEdycji();
        }


        #endregion        
        public override bool IsChanged => !ZlecenieProdukcyjneOrg.Compare(ZlecenieProdukcyjne);

        public override bool IsValid => ZlecenieProdukcyjne.IsValid
                                     && MieszankaViewModel.IsValid;

        public override void IsChanged_False()
        {
            ZlecenieProdukcyjneOrg = ZlecenieProdukcyjne.DeepClone();
        }

        #region DeleteCommand

        protected override bool DeleteCommandCanExecute()
        {
            return ZlecenieProdukcyjne.IDProdukcjaZlecenie != 0;
        }

        protected override async void DeleteCommandExecute()
        {
            if (!DialogService.ShowQuestion_BoolResult("Czy usunąć bieżące zlecenie produkcyjne?"))
                return;

            await MieszankaViewModel.DeleteAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);
            await TowarViewModel.DeleteAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);

            UnitOfWork.tblProdukcjaZlecenie.Remove(ZlecenieProdukcyjne);
            await UnitOfWork.SaveAsync();
        }


        #endregion

        #region LoadCommand

        protected override async void LoadCommandExecute()
        {
            if (zlecenieTowar is null)
            {
                await GenerujNoweZlecenieProdukcyjne();
            }
            else
            {
                await PobierzZlecenieProdukcyjneDoEdycji();
            }
        }

        private async Task PobierzZlecenieProdukcyjneDoEdycji()
        {
            ZlecenieProdukcyjne = await UnitOfWork.tblProdukcjaZlecenie
                                        .SingleOrDefaultAsync(z => z.IDProdukcjaZlecenie == zlecenieTowar.IDProdukcjaZlecenie);

            await WywolajLoadAsyncNaZaleznychViewModelach();

            Tytul = $"Edycja zlecenia produkcyjnego nr {ZlecenieProdukcyjne?.NrDokumentu}";
        }

        private async Task WywolajLoadAsyncNaZaleznychViewModelach()
        {
            await TowarViewModel.LoadAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);
            await MieszankaViewModel.LoadAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);
        }

        private async Task GenerujNoweZlecenieProdukcyjne()
        {
            ZlecenieProdukcyjne.IDZlecajacy = UzytkownikZalogowany.Uzytkownik is null ? 7 : UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT; // ID 7 to Tomasz Strączek - na potrzeby testow
            ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;
            ZlecenieProdukcyjne.NrZlecenia = await PobierzNrZleceniaAsync();
            ZlecenieProdukcyjne.NrDokumentu = await PobierzNrDokumentu();
            ZlecenieProdukcyjne.KodKreskowy = ZlecenieProdukcyjne.NrDokumentu?.Replace("/", string.Empty);

            Tytul = $"Nowe zlecenie produkcyjne, nr: {ZlecenieProdukcyjne.NrDokumentu}";
        }
        #endregion

        #region SaveCommand
        protected override bool SaveCommandCanExecute()
        {
            return IsValid;
        }

        protected override async void SaveCommandExecute()
        {
            if (ZlecenieProdukcyjne.IDProdukcjaZlecenie == 0)
            {
                ZlecenieProdukcyjne.DataUtworzenia = DateTime.Now;
                ZlecenieProdukcyjne.IDProdukcjaZlecenieStatus = (int)ProdukcjaZlecenieStatusEnum.Oczekuje;

                UnitOfWork.tblProdukcjaZlecenie.Add(ZlecenieProdukcyjne);
            }

            await UnitOfWork.SaveAsync();

            await MieszankaViewModel.SaveAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);
            await TowarViewModel.SaveAsync(ZlecenieProdukcyjne.IDProdukcjaZlecenie);

            Messenger.Send(ZlecenieProdukcyjne);
            ViewService.Close(this.GetType().Name);
        }


        #endregion

        private async Task<int> PobierzNrZleceniaAsync()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                return await uow.tblProdukcjaZlecenie
                    .GetNewNumberAsync(t => t.DataRozpoczecia.Value.Year == DateTime.Now.Year,
                                         t => t.NrZlecenia.Value);
            }
        }

        private async Task<string> PobierzNrDokumentu()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                return await uow.tblProdukcjaZlecenie
                .GetNewFullNumberAsync(t => t.DataRozpoczecia.Value.Year == DateTime.Now.Year,
                                     t => t.NrZlecenia.Value, "ZLP");
            }
        }
    }
}
