using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.RozliczenieFV.Szczegoly;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Zapotrzebowanie.RozliczenieFV.Szczegoly
{
    [TestFixture]
    public class ZapotrzebowanieRozliczenieFVSzczegolyViewModelTests
    {

        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IViewService> viewService;
        private Mock<IDialogService> dialogService;
        private Mock<IMessenger> messenger;
        private Messenger messengerOrg;
        private Mock<IPlikiCRUD> pliki;
        private ZapotrzebowanieRozliczenieFVSzczegolyViewModel sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            viewService = new Mock<IViewService>();
            dialogService = new Mock<IDialogService>();
            messenger = new Mock<IMessenger>();
            messengerOrg = new Messenger();
            pliki = new Mock<IPlikiCRUD>();


            sut = CreateSut(messenger.Object);
        }

        private ZapotrzebowanieRozliczenieFVSzczegolyViewModel CreateSut(IMessenger messenger)
        {
            return new ZapotrzebowanieRozliczenieFVSzczegolyViewModel(unitOfWork.Object, 
                                                                      dialogService.Object, 
                                                                      viewService.Object, 
                                                                      pliki.Object, 
                                                                      messenger);
        }

        #region ZapiszCommand
        #region CanExecute
        [Test]
        public void ZapiszCommandCanExecute_GdyListaPlikowNiepusta_True()
        {
            sut.ListaPlikow = new ObservableCollection<tblPliki>
            {
                new tblPliki{ IDPlik=0, }
            };

            var actual = sut.ZapiszCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        #endregion
        #endregion
    }
}
