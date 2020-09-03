using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.Sprzedaz.Ewidencja;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Finanse.Sprzedaz.Ewidencja
{
    public class SprzedazEwidencjaViewModelTests : TestBase

    {
        private SprzedazEwidencjaViewModel sut;
        private Mock<IVwZestSprzedazyAGGRepository> vwZestSprzedazyAGG;

        public override void SetUp()
        {
            base.SetUp();

            vwZestSprzedazyAGG = new Mock<IVwZestSprzedazyAGGRepository>();
            UnitOfWork.Setup(s => s.vwZestSprzedazyAGG).Returns(vwZestSprzedazyAGG.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new SprzedazEwidencjaViewModel(ViewModelService.Object);
        }

        #region LoadCommandExecute
        [Test]
        public void LoadCommandExecute_LadujeEncjeZBazy()
        {
            sut.LoadCommand.Execute(null);

            vwZestSprzedazyAGG.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommandExecute_GenerujeListeGrup()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Grupa="B"},
                new vwZestSprzedazyAGG{Id=1, Grupa="C"},
                new vwZestSprzedazyAGG{Id=1, Grupa="A"},
            });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(3, sut.ListaGrup.Count());
            Assert.AreEqual("A", sut.ListaGrup.First());
            Assert.AreEqual("C", sut.ListaGrup.Last());

        }

        [Test]
        public void LoadCommandExecute_GenerujeListeHandlowcow()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Handlowiec="B"},
                new vwZestSprzedazyAGG{Id=1, Handlowiec="C"},
                new vwZestSprzedazyAGG{Id=1, Handlowiec="A"},
            });

            sut.LoadCommand.Execute(null);

            Assert.AreEqual(3, sut.ListaHandlowcow.Count());
            Assert.AreEqual("A", sut.ListaHandlowcow.First());
            Assert.AreEqual("C", sut.ListaHandlowcow.Last());

        }
        [Test]
        public void LoadCommandExecute_Podsumowuje()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="B", Handlowiec="MM", DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="C", Handlowiec="JR",DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="A", Handlowiec="TS",DataSprzedazy=DateTime.Now},
            });
            
            sut.LoadCommand.Execute(null);

            Assert.AreEqual(3, sut.Podsumowanie.Ilosc);
        }
        #endregion

        #region SzukajCommand
        [Test]
        public void SzukajCommandExecute_FiltrujePoGrupie()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Grupa="B", Handlowiec="MM", DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Grupa="C", Handlowiec="JR",DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Grupa="A", Handlowiec="TS",DataSprzedazy=DateTime.Now},
            });
            sut.LoadCommand.Execute(null);
            sut.WybranaGrupa = sut.ListaGrup.First();


            sut.FiltrujCommand.Execute(null);

            Assert.AreEqual(1, sut.ListaSprzedazy.Count());
        }

        [Test]
        public void SzukajCommandExecute_FiltrujePoHandlowcu()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Grupa="B", Handlowiec="MM", DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Grupa="C", Handlowiec="JR",DataSprzedazy=DateTime.Now},
                new vwZestSprzedazyAGG{Id=1, Grupa="A", Handlowiec="TS",DataSprzedazy=DateTime.Now},
            });
            sut.LoadCommand.Execute(null);
            sut.WybranyHandlowiec = "MM";


            sut.FiltrujCommand.Execute(null);

            Assert.AreEqual(1, sut.ListaSprzedazy.Count());
        }

        [Test]
        public void SzukajCommandExecute_FiltrujePoDacie()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Grupa="B", Handlowiec="MM", DataSprzedazy=DateTime.Now.Date.AddDays(1)},
                new vwZestSprzedazyAGG{Id=1, Grupa="C", Handlowiec="JR",DataSprzedazy=DateTime.Now.Date.AddDays(2)},
                new vwZestSprzedazyAGG{Id=1, Grupa="A", Handlowiec="TS",DataSprzedazy=DateTime.Now.Date},
            });
            sut.LoadCommand.Execute(null);
            sut.DataSprzedazyOd = DateTime.Now.Date.AddDays(1);
            sut.DataSprzedazyDo = DateTime.Now.Date.AddDays(2);


            sut.FiltrujCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaSprzedazy.Count());
        }

        [Test]
        public void SzukajCommandExecute_Podsumowuje()
        {
            vwZestSprzedazyAGG.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<vwZestSprzedazyAGG>
            {
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="B", Handlowiec="MM", DataSprzedazy=DateTime.Now.Date.AddDays(1)},
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="C", Handlowiec="JR",DataSprzedazy=DateTime.Now.Date.AddDays(2)},
                new vwZestSprzedazyAGG{Id=1, Ilosc=1, Grupa="A", Handlowiec="TS",DataSprzedazy=DateTime.Now},
            });
            sut.LoadCommand.Execute(null);
            sut.DataSprzedazyOd = DateTime.Now.Date.AddDays(1);
            sut.DataSprzedazyDo = DateTime.Now.Date.AddDays(2);

            sut.FiltrujCommand.Execute(null);

            Assert.AreEqual(2, sut.Podsumowanie.Ilosc);
        }
        #endregion
    }
}
