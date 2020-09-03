using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.Utilities.ExcelReportGenerator;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.PodsumowanieFinansowe
{
    public class PodsumowanieFinansoweViewModelTests : TestBase
    {
        private PodsumowanieFinansoweViewModel sut;
        private Mock<IVwZamOdKlientaAGGRepository> vwZamOdKlientaAGG;
        private Mock<IPodsumowanieFinansoweHelper> podsumowanieHelper;
        private Mock<IXlsService> xlsService;
        private Mock<IXlsServiceBuilder> xlsServiceBuilder;
        private Mock<IPodsumowanieZamowieniaOdKlientowHelper> podsumowanieZamowieniaOdKlientowHelper;
        private Mock<IPodsumowanieSprzedazHelper> podsumowanieSprzedazHelper;
        private Mock<IPodsumowanieProdukcjaHelper> podsumowanieProdukcjaHelper;
        private Mock<IPodsumowanieMagazynyHelper> podsumowanieMagazynyHelper;
        private Mock<IPodsumowanieNaleznosciIZobowiazaniaHelper> podsumowanieNaleznosciIZobowiazaniaHelper;
        private Mock<IPodsumowanieKontaBankoweHelper> podsumowanieKontBankowychHelper;

        public override void SetUp()
        {
            base.SetUp();

            vwZamOdKlientaAGG = new Mock<IVwZamOdKlientaAGGRepository>();
            UnitOfWork.Setup(s => s.vwZamOdKlientaAGG).Returns(vwZamOdKlientaAGG.Object);
            podsumowanieHelper = new Mock<IPodsumowanieFinansoweHelper>();
            xlsService = new Mock<IXlsService>();
            xlsServiceBuilder = new Mock<IXlsServiceBuilder>();

            podsumowanieZamowieniaOdKlientowHelper = new Mock<IPodsumowanieZamowieniaOdKlientowHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieZamowieniaOdKlientowHelper).Returns(podsumowanieZamowieniaOdKlientowHelper.Object);
            
            podsumowanieSprzedazHelper = new Mock<IPodsumowanieSprzedazHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieSprzedazHelper).Returns(podsumowanieSprzedazHelper.Object);

            podsumowanieProdukcjaHelper = new Mock<IPodsumowanieProdukcjaHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieProdukcjaHelper).Returns(podsumowanieProdukcjaHelper.Object);
            
            podsumowanieMagazynyHelper = new Mock<IPodsumowanieMagazynyHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieMagazynyHelper).Returns(podsumowanieMagazynyHelper.Object);

            podsumowanieNaleznosciIZobowiazaniaHelper = new Mock<IPodsumowanieNaleznosciIZobowiazaniaHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieNaleznosciIZobowiazaniaHelper).Returns(podsumowanieNaleznosciIZobowiazaniaHelper.Object);

            podsumowanieKontBankowychHelper = new Mock<IPodsumowanieKontaBankoweHelper>();
            podsumowanieHelper.Setup(s => s.PodsumowanieKontBankowychHelper).Returns(podsumowanieKontBankowychHelper.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new PodsumowanieFinansoweViewModel(ViewModelService.Object,podsumowanieHelper.Object, xlsServiceBuilder.Object);
        }

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_LadowaniePodsumowan()
        {
            sut.LoadCommand.Execute(null);

            podsumowanieZamowieniaOdKlientowHelper.Verify(v => v.PodsumujZamowieniaOdKlientow(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            podsumowanieSprzedazHelper.Verify(v => v.PobierzSprzedazAGGWDatach(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            podsumowanieProdukcjaHelper.Verify(v => v.PobierzPodsumowanieProdukcjiWDatach(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            podsumowanieMagazynyHelper.Verify(v => v.PobierzPodsumowanieMagazynuDoDaty(It.IsAny<object>(), It.IsAny<DateTime>()));
            podsumowanieNaleznosciIZobowiazaniaHelper.Verify(v => v.PobierzPodsumowanieNalzenosciIZobowiazan(It.IsAny<DateTime>()));
            podsumowanieKontBankowychHelper.Verify(v => v.PobierzStanKontZDaty(It.IsAny<DateTime>()));
        }

        [Test]
        public void LoadCommandExecute_LadowanieRozliczeniaMesAccessHelper()
        {
            sut.LoadCommand.Execute(null);

            podsumowanieHelper.Verify(v => v.PodsumowanieProdukcjaHelper.LoadAsync());
        }
        #endregion

        #region SzukajCommand

        #region CanExecute
        
        [Test]
        public void SzukajCommandCanExecute_GdyWszystkieIsActiveButtonTrue_True()
        {
            podsumowanieZamowieniaOdKlientowHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieSprzedazHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieProdukcjaHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieMagazynyHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieNaleznosciIZobowiazaniaHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieKontBankowychHelper.Setup(s => s.IsButtonActive).Returns(true);

            var isButtonActive = sut.SzukajCommand.CanExecute(null);

            Assert.IsTrue(isButtonActive);
        }

        [Test]
        public void SzukajCommandCanExecute_GdyChociazJedenIsActiveButtonFalse_False()
        {
            podsumowanieZamowieniaOdKlientowHelper.Setup(s => s.IsButtonActive).Returns(false);
            podsumowanieSprzedazHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieProdukcjaHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieMagazynyHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieNaleznosciIZobowiazaniaHelper.Setup(s => s.IsButtonActive).Returns(true);
            podsumowanieKontBankowychHelper.Setup(s => s.IsButtonActive).Returns(true);

            var isButtonActive = sut.SzukajCommand.CanExecute(null);

            Assert.IsFalse(isButtonActive);
        }

        #endregion
        #endregion

    }
}
