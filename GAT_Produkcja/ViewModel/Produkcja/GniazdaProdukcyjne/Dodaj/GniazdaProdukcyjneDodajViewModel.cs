using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Ewidencja;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Dodaj
{
    [AddINotifyPropertyChangedInterface]

    public class GniazdaProdukcyjneDodajViewModel : ViewModelBase
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private bool isChanged;
        private tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjneOrg;
        private bool czyOminacWiadomoscPodczasZamykaniaOkna;

        #endregion

        #region Properties
        public tblProdukcjaGniazdoProdukcyjne Gniazdo { get; set; }
        public IEnumerable<tblTowarGrupa> ListaGrup { get; set; }
        public tblTowarGrupa WybranaGrupa { get; set; }
        public string Tytul { get; set; }
        public bool IsChanged
        { 
            get => isChanged = !Gniazdo.Compare(gniazdoProdukcyjneOrg); 
            set => isChanged = value;
        }

        #endregion

        #region Commands
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand ZamknijOknoCommand { get; set; }
        #endregion

        #region CTOR
        public GniazdaProdukcyjneDodajViewModel(IUnitOfWork unitOfWork,
                                                IUnitOfWorkFactory unitOfWorkFactory,
                                                IDialogService dialogService,
                                                IViewService viewService,
                                                IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.messenger = messenger;

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            ZamknijOknoCommand = new RelayCommand(ZamknijOknoCommandExecute);



            Gniazdo = new tblProdukcjaGniazdoProdukcyjne();
            gniazdoProdukcyjneOrg = Gniazdo.DeepClone();
            var hasChanges = unitOfWork.tblProdukcjaGniazdoProdukcyjne.HasChanges();

            WstawTytul();
        }
        private void WstawTytul()
        {
            if (Gniazdo.IDProdukcjaGniazdoProdukcyjne == 0)
                Tytul = "Dodawanie gniazda produkcyjnego";
            else
                Tytul = "Edytowanie gniazda produkcyjnego";
        }

        #endregion

        #region Commands

        #region ZamknijOknoCommand
        private void ZamknijOknoCommandExecute()
        {

            if (czyOminacWiadomoscPodczasZamykaniaOkna)
            {
                return;
            }
            if (IsChanged)
            {
                if (dialogService.ShowQuestion_BoolResult("Dane zostały zmodyfikowane lecz nie zostały zapisane i będą utracone." +
                                                          "\r\n" +
                                                          "\r\nCzy zamknąć okno?"))
                {
                    viewService.Close<GniazdaProdukcyjneDodajViewModel>();
                }
            }
        }
        #endregion

        #region ZaladujWartosciPoczatkoweCommand
        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            using (var uow = unitOfWorkFactory.Create())
            {
                ListaGrup = await uow.tblTowarGrupa.GetAllAsync();
                ListaGrup = ListaGrup.Where(l => l.Grupa.Contains("Geoko") || l.Grupa.Contains("Geow"));
            }
        }
        #endregion

        #region ZapiszCommand
        private bool ZapiszCommandCanExecute()
        {
            if (IsChanged == false)
                return false;
            if (Gniazdo.IsValid == false)
                return false;
            
            return true;
        }

        private async void ZapiszCommandExecute()
        {
            if (Gniazdo.IDProdukcjaGniazdoProdukcyjne == 0)
            {
                unitOfWork.tblProdukcjaGniazdoProdukcyjne.Add(Gniazdo);
            }

            await unitOfWork.SaveAsync();

            messenger.Send<string,GniazdaProdukcyjneEwidencjaViewModel>("Odswiez");
            czyOminacWiadomoscPodczasZamykaniaOkna = true;
            viewService.Close<GniazdaProdukcyjneDodajViewModel>();
        }


        #endregion

        #endregion




    }
}
