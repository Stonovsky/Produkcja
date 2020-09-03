using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using PropertyChanged;

namespace GAT_Produkcja.ViewModel.Magazyn.Towar.Szczegoly
{
    [AddINotifyPropertyChangedInterface]

    public class TowarSzczegolyViewModel : ViewModelBase
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private tblTowar towar;
        private IEnumerable<tblKodKreskowyTyp> listaTypowKodowKreskowych;
        private tblKodKreskowyTyp wybranyTypKoduKreskowego;
        private IEnumerable<tblJm> listaJM;
        private tblJm wybranaJM;
        private string tytul;

        private IEnumerable<tblTowar> listaTowarow;

        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }

        public string Tytul { get; set; }
        public tblTowar Towar { get; set; }
        public IEnumerable<tblTowarGrupa> ListaGrupTowarow { get; set; }
        public tblTowarGrupa WybranaGrupa { get; set; }
        public IEnumerable<tblKodKreskowyTyp> ListaTypowKodowKreskowych { get; set; }
        public tblKodKreskowyTyp WybranyTypKoduKreskowego { get; set; }
        public IEnumerable<tblJm> ListaJM { get; set; }
        public tblJm WybranaJM { get; set; }
        #endregion

        #region CTOR
        public TowarSzczegolyViewModel(IUnitOfWork unitOfWork,
                                        IDialogService dialogService,
                                        IViewService viewService,
                                        IMessenger messenger
            )
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);

            messenger.Register<tblTowar>(this, GdyPrzeslanoTowar);

            Towar = new tblTowar();
            //Towar.MetaSetUp();
        }
        #endregion

        #region Commands
        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            Tytul = "Dodaj towar";
            ListaJM = await unitOfWork.tblJm.GetAllAsync().ConfigureAwait(false);
            ListaTypowKodowKreskowych = await unitOfWork.tblKodKreskowyTyp.GetAllAsync().ConfigureAwait(false);
            ListaGrupTowarow = await unitOfWork.tblTowarGrupa.GetAllAsync().ConfigureAwait(false);

        }
        private bool UsunCommandCanExecute()
        {
            if (Towar.IDTowar == 0)
                return false;

            return true;
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć towar?"))
            {
                unitOfWork.tblTowar.Remove(Towar);
                await unitOfWork.SaveAsync();
            }
        }

        private bool ZapiszCommandCanExecute()
        {
            if (Towar.IsValid)
                return true;

            return false;
        }

        private async void ZapiszCommandExecute()
        {
            if (Towar.IDTowar == 0)
            {
                if (await CzyTowarIstniejeWBazie())
                    dialogService.ShowInfo_BtnOK("Towar o takiej samej nazwie istnieje w bazie danych. " +
                                                 "\rZmień nazwę i spróbuj ponownie");
                else
                {
                    unitOfWork.tblTowar.Add(Towar);
                    await unitOfWork.SaveAsync();
                    dialogService.ShowInfo_BtnOK("Towar dodany");
                    messenger.Send<string, TowarEwidencjaViewModel>("Odswiez");
                    viewService.Close<TowarSzczegolyViewModel>();
                }
            }
        }

        private async Task<bool> CzyTowarIstniejeWBazie()
        {
            var towar = await unitOfWork.tblTowar.WhereAsync(c => c.Nazwa.ToLower().Contains(Towar.Nazwa.ToLower()));
            if (towar == null)
                return false;

            if (towar.Count() == 0)
                return false;

            return true;
        }

        #endregion

        #region Messengers
        private async void GdyPrzeslanoTowar(tblTowar obj)
        {
            listaTowarow = await unitOfWork.tblTowar.GetAllAsync() as List<tblTowar>;
            Towar = await unitOfWork.tblTowar.GetByIdAsync(obj.IDTowar);
            ListaJM = await unitOfWork.tblJm.GetAllAsync();
            ListaTypowKodowKreskowych = await unitOfWork.tblKodKreskowyTyp.GetAllAsync();

            Tytul = $"Szczegóły Towaru: {Towar.Nazwa}";
        }
        #endregion
    }
}
