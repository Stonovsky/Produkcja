using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using PropertyChanged;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Dodaj;

namespace GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Ewidencja
{
    [AddINotifyPropertyChangedInterface]
    public class MieszankaEwidencjaViewModel : ViewModelBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        #region Properties
        public IEnumerable<tblMieszanka> ListaMieszanek { get; set; }
        public tblMieszanka WybranaMieszanka { get; set; }
        public string Tytul { get; set; }
        public string NazwaMieszankiDoWyszukania { get; set; }
        #endregion

        #region Commands
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand SzukajCommand { get; set; }
        public RelayCommand DodajMieszankeCommand { get; set; }
        public RelayCommand PokazSzczegolyCommand { get; set; }
        #endregion

        #region CTOR
        public MieszankaEwidencjaViewModel(IUnitOfWork unitOfWork, IViewService viewService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.messenger = messenger;
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            SzukajCommand = new RelayCommand(SzukajCommandExecute, SzukajCommandCanExecute);
            DodajMieszankeCommand = new RelayCommand(DodajMieszankeCommandExecute);
            PokazSzczegolyCommand = new RelayCommand(PokazSzczegolyCommandExecute);

            messenger.Register<string>(this, Odswiez);

            Tytul = "Ewidencja mieszanek";
        }

        private void PokazSzczegolyCommandExecute()
        {
            viewService.Show<MieszankaDodajViewModel>();
            messenger.Send<tblMieszanka, MieszankaDodajViewModel>(WybranaMieszanka);
        }

        private void DodajMieszankeCommandExecute()
        {
            viewService.Show<MieszankaDodajViewModel>();
        }

        private async void SzukajCommandExecute()
        {
            ListaMieszanek = await unitOfWork.tblMieszanka.WhereAsync(m => m.NazwaMieszanki.Contains(NazwaMieszankiDoWyszukania));
        }

        private bool SzukajCommandCanExecute()
        {
            return true;
        }

        private async void Odswiez(string obj)
        {
            await PobierzListeMieszanek();
        }
        #endregion

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            await PobierzListeMieszanek();
        }

        private async Task PobierzListeMieszanek()
        {
            ListaMieszanek = await unitOfWork.tblMieszanka.GetAllWithJmAsync();
        }
    }
}
