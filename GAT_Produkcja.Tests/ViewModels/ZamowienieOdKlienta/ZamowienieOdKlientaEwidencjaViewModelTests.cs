using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.ZamowienieOdKlienta
{
    public class ZamowienieOdKlientaEwidencjaViewModelTests : TestBase
    {
        private ZamowienieOdKlientaEwidencjaViewModel sut;
        private Mock<IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel> szczegoly;
        private Mock<IVwZamOdKlientaAGGRepository> vwZamOdKlientaAGG;

        public override void SetUp()
        {
            base.SetUp();

            szczegoly = new Mock<IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel>();

            vwZamOdKlientaAGG = new Mock<IVwZamOdKlientaAGGRepository>();
            UnitOfWork.Setup(s => s.vwZamOdKlientaAGG).Returns(vwZamOdKlientaAGG.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ZamowienieOdKlientaEwidencjaViewModel(ViewModelService.Object,
                                                            szczegoly.Object);
        }

        #region LoadCommandExecute
        [Test]
        public void LoadCommandExecute_PobieraDaneZBD()
        {

            sut.LoadCommand.Execute(null);

            vwZamOdKlientaAGG.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommandExecute_PrzypisujeGrupy()
        {
            ListaElementowZBazy();

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(3, sut.Grupa.Count());
            Assert.AreEqual("Ga", sut.Grupa.First());
        }

        private void ListaElementowZBazy()
        {
            vwZamOdKlientaAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZamOdKlientaAGG>
            {
                new vwZamOdKlientaAGG{TowarNazwa="Geokomórka AT CELL 001.100", Ilosc=1, Grupa="Gc", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=6},
                new vwZamOdKlientaAGG{TowarNazwa="Geokomórka AT CELL 001.100", Ilosc=1, Grupa="Gc", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=6},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Gb", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=6},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Ga", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=6},
                new vwZamOdKlientaAGG{TowarNazwa="test2", Ilosc=2, Grupa="Ga", DataMag = DateTime.Now.Date, DataWyst=DateTime.Now.Date, TerminRealizacji=DateTime.Now.Date, Status=6},
            });
        }

        #endregion

        #region FiltrujCommand
        [Test]
        public void FiltrujCommandExecute_GdyNazwaTowaruWpisana_FiltrujePoNazwie()
        {
            sut.FiltrujCommand.Execute(null);

            Messenger.Verify(v => v.Send(sut.Filtr));
        }

        #endregion
    }
}
