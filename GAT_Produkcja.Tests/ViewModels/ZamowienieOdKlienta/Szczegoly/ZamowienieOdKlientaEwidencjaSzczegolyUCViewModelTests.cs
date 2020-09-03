using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.MainMenu.Messages;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.ZamowienieOdKlienta.Szczegoly
{
    public class ZamowienieOdKlientaEwidencjaSzczegolyUCViewModelTests : TestBase
    {
        private Mock<IZamOdKlientaFiltrHelper> filtr;
        private ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel sut;

        public override void SetUp()
        {
            base.SetUp();
            filtr = new Mock<IZamOdKlientaFiltrHelper>();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel(ViewModelService.Object, filtr.Object);
        }


        #region LoadAsync
        [Test]
        public void LoadAsync_PobieraListeZFiltra()
        {
            sut.LoadAsync(null);

            filtr.Verify(v => v.FiltrujAsync(It.IsAny<ZK_Filtr>()));
        }

        [Test]
        public async Task LoadAsync_WysylaPodsumowanie()
        {
            await sut.LoadAsync(null);

            Messenger.Verify(v => v.Send(It.IsAny<ZK_Podsumowanie>()));
        }

        [Test]
        public async Task LoadAsync_SortujeListePoDacieWystawieniaMalejaco()
        {
            filtr.Setup(s => s.FiltrujAsync(null)).ReturnsAsync(new List<vwZamOdKlientaAGG>
            {
                new vwZamOdKlientaAGG{DataWyst = DateTime.Now.Date.AddDays(-2)},
                new vwZamOdKlientaAGG{DataWyst = DateTime.Now.Date},
            });

            await sut.LoadAsync(null);

            Assert.AreEqual(DateTime.Now.Date, sut.ListaZamowienOdKlientow.First().DataWyst);
        }
        #endregion


    }
}
