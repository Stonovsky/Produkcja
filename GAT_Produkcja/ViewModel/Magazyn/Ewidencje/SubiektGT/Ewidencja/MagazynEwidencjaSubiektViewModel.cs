using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja
{
    public class MagazynEwidencjaSubiektViewModel : ListCommandViewModelBase
    {
        public IMagazynEwidencjaSubiektUCViewModel MagazynEwidencjaUC { get; }
        public MagazynEwidencjaSubiektViewModel(IViewModelService viewModelService,
                                                IMagazynEwidencjaSubiektUCViewModel magazynEwidencjaUC
                                                ) 
            : base(viewModelService)
        {
            MagazynEwidencjaUC = magazynEwidencjaUC;
        }


        protected override async void LoadCommandExecute()
        {
            await MagazynEwidencjaUC.LoadAsync(null);
        }
    }
}
