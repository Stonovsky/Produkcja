using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;
using PropertyChanged;
using GalaSoft.MvvmLight.Messaging;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.RuchTowaru
{
    [AddINotifyPropertyChangedInterface]

    public class MagazynRuchTowaruViewModel : ViewModelBase
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        #endregion

        #region Properties
        public IEnumerable<vwRuchTowaru> ListaRuchuTowarow { get; set; }
        public vwRuchTowaru WybranyTowar { get; set; }
        public string Tytul { get; set; }


        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand UsunRuchTowaruCommand { get; set; }

        #endregion

        #region CTOR
        public MagazynRuchTowaruViewModel(IUnitOfWork unitOfWork,
                                            IViewService viewService,
                                            IDialogService dialogService,
                                            IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            UsunRuchTowaruCommand = new RelayCommand(UsunRuchTowaruCommandExecute, UsunRuchTowaruCommandCanExecute);

            messenger.Register<string>(this, GdyPrzeslanoWiadomoscOdswiez);

            ListaRuchuTowarow = new List<vwRuchTowaru>();
            Tytul = "Ewidencja ruchu towarów";
        }

        private async void GdyPrzeslanoWiadomoscOdswiez(string obj)
        {
            if (obj == "Odswiez")             
                await  PobierzListeTowarowAsync();
        }

        private async void UsunRuchTowaruCommandExecute()
        {
            if (!dialogService.ShowQuestion_BoolResult("Czy usunąć pozycję?"))
                return;

            if (WybranyTowar == null || WybranyTowar.IDRuchTowar == 0)
                return;

            await UsunRuchTowarParametryGeowloknina();
            await UsunRuchTowar();
            await UsunRuchNaglowek();
            
            await unitOfWork.SaveAsync();
            messenger.Send("Odswiez");
        }

        private async Task UsunRuchTowarParametryGeowloknina()
        {
            var towarRuchGeowlokninaParametry = await unitOfWork.tblRuchTowarGeowlokninaParametry.GetByIdAsync(WybranyTowar.IDRuchTowarGeowlokninaParametry.GetValueOrDefault());
            if (towarRuchGeowlokninaParametry != null)
                unitOfWork.tblRuchTowarGeowlokninaParametry.Remove(towarRuchGeowlokninaParametry);
        }

        private async Task UsunRuchTowar()
        {
            var towarRuch = await unitOfWork.tblRuchTowar.GetByIdAsync(WybranyTowar.IDRuchTowar);
            if (towarRuch != null)
                unitOfWork.tblRuchTowar.Remove(towarRuch);
        }

        private async Task UsunRuchNaglowek()
        {
            var ruchTowarDlaNaglowka = await unitOfWork.tblRuchTowar.WhereAsync(n => n.IDRuchNaglowek == WybranyTowar.IDRuchNaglowek);
            if (ruchTowarDlaNaglowka.Count() == 1)
            {
                var ruchNaglowek = await unitOfWork.tblRuchNaglowek.GetByIdAsync(WybranyTowar.IDRuchNaglowek);
                unitOfWork.tblRuchNaglowek.Remove(ruchNaglowek);
            }
        }

        private bool UsunRuchTowaruCommandCanExecute()
        {
            return true;
        }

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            await PobierzListeTowarowAsync();
        }

        private async Task PobierzListeTowarowAsync()
        {
            ListaRuchuTowarow = await unitOfWork.vwRuchTowaru.GetAllAsync();

        }
        #endregion
    }
}
