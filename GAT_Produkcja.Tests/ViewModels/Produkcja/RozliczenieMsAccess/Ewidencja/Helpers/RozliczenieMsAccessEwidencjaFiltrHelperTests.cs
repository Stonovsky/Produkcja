using GAT_Produkcja.db;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers
{
    public class RozliczenieMsAccessEwidencjaFiltrHelperTests : TestBaseGeneric<RozliczenieMsAccessEwidencjaFiltrHelper>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new RozliczenieMsAccessEwidencjaFiltrHelper();
        }

        #region Guard clause

        [Test]
        public void FiltrujListe_GdyBrakListy_Wyjatek()
        {
            var filtr = new RozliczenieEwidencjaFiltrModel();
            var lista = new List<tblProdukcjaRozliczenie_PWPodsumowanie>
            {
                new tblProdukcjaRozliczenie_PWPodsumowanie{IDProdukcjaRozliczenie_PWPodsumowanie=1},
                new tblProdukcjaRozliczenie_PWPodsumowanie{IDProdukcjaRozliczenie_PWPodsumowanie=2},
            };

            Assert.Throws<ArgumentException>(() => sut.FiltrujListe(null, filtr));
        }

        [Test]
        public void FiltrujListe_GdyListaPusta_ZwrocListePusta()
        {
            var filtr = new RozliczenieEwidencjaFiltrModel();
            var lista = new List<tblProdukcjaRozliczenie_PWPodsumowanie>();

            var listaFiltrowana = sut.FiltrujListe(lista, filtr);

            Assert.IsEmpty(listaFiltrowana);
        }

        [Test]
        public void FiltrujListe_GdyBrakFiltra_ZwracaListeBazowa()
        {
            var lista = new List<tblProdukcjaRozliczenie_PWPodsumowanie>
            {
                new tblProdukcjaRozliczenie_PWPodsumowanie{IDProdukcjaRozliczenie_PWPodsumowanie=1},
                new tblProdukcjaRozliczenie_PWPodsumowanie{IDProdukcjaRozliczenie_PWPodsumowanie=2},
            };

            var listaFiltrowana = sut.FiltrujListe(lista, null);

            Assert.AreEqual(2, listaFiltrowana.Count());
        }
        #endregion
        [Test]
        public void FiltrujListe_ArgumentyOK_FiltrujeListe()
        {
            var lista = new List<tblProdukcjaRozliczenie_PWPodsumowanie>
            {
                new tblProdukcjaRozliczenie_PWPodsumowanie
                {
                    IDProdukcjaRozliczenie_PWPodsumowanie=1,
                    tblProdukcjaRozliczenie_Naglowek = new tblProdukcjaRozliczenie_Naglowek{DataDodania=DateTime.Now.Date.AddDays(-1)},
                    SymbolTowaruSubiekt="PES",
                    NazwaTowaruSubiekt="ALTEX AT PES 100",
                    Jm = "kg",
                },
                new tblProdukcjaRozliczenie_PWPodsumowanie
                {
                    IDProdukcjaRozliczenie_PWPodsumowanie=1,
                    tblProdukcjaRozliczenie_Naglowek = new tblProdukcjaRozliczenie_Naglowek{DataDodania=DateTime.Now.Date.AddDays(-1)},
                    SymbolTowaruSubiekt="PP",
                    NazwaTowaruSubiekt="ALTEX AT PP 200",
                    Jm = "m",
                },
                new tblProdukcjaRozliczenie_PWPodsumowanie
                {
                    IDProdukcjaRozliczenie_PWPodsumowanie=1,
                    tblProdukcjaRozliczenie_Naglowek = new tblProdukcjaRozliczenie_Naglowek{DataDodania=DateTime.Now.Date.AddDays(-1)},
                    SymbolTowaruSubiekt="PP",
                    NazwaTowaruSubiekt="ALTEX AT PP 200",
                    Jm = "kg",
                },
                new tblProdukcjaRozliczenie_PWPodsumowanie
                {
                    IDProdukcjaRozliczenie_PWPodsumowanie=1,
                    tblProdukcjaRozliczenie_Naglowek = new tblProdukcjaRozliczenie_Naglowek{DataDodania=DateTime.Now.Date.AddDays(-2)},
                    SymbolTowaruSubiekt="PP",
                    NazwaTowaruSubiekt="ALTEX AT PP 200",
                    Jm = "kg",
                },
            };
            var filtr = new RozliczenieEwidencjaFiltrModel()
            {
                DataOd = DateTime.Now.Date.AddDays(-1),
                DataDo = DateTime.Now.Date,
                Rodzaj="PP",
                Jm="kg"
            };

            var listaFiltrowana = sut.FiltrujListe(lista, filtr);

            Assert.AreEqual(1, listaFiltrowana.Count());
        }

    }
}
