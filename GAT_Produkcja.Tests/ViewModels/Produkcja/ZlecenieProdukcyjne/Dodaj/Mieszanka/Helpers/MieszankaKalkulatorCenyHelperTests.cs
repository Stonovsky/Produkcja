using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieProdukcyjne.Dodaj.Mieszanka.Helpers
{
    [TestFixture]
    public class MieszankaKalkulatorCenyHelperTests
    {
        private MieszankaKalkulatorCenyHelper sut;
        private List<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPrzykladowa;
        
        [SetUp]
        public void SetUp()
        {
            sut = new MieszankaKalkulatorCenyHelper();
        }

        [Test]
        public void PrzypiszCeneJednostkowaZgodnieZUdzialem_GdyListaJestNull_ZwracaNull()
        {

            Assert.Throws<ArgumentException>(() => sut.DodajWartoscMieszankiDoPozycjiListy(null));
        }


        private void StworzListeElemenowTestowych()
        {
            listaPrzykladowa= new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, ZawartoscProcentowa=0.5M,IloscKg=1, Cena_kg=1M},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=2, ZawartoscProcentowa=0.5M,IloscKg=3, Cena_kg=1M},
            };
        }


        [Test]
        public void ObliczWartoscPozycjiZgodnieZUdzialem_GdyListaNieJestPusta_ObliczaWartoscPozycji()
        {
            StworzListeElemenowTestowych();

            var lista = sut.ObliczWartoscPozycji(listaPrzykladowa);

            Assert.AreEqual(1, lista.SingleOrDefault(s=>s.IDZlecenieProdukcyjneMieszanka==1).Wartosc_kg);
            Assert.AreEqual(3, lista.SingleOrDefault(s=>s.IDZlecenieProdukcyjneMieszanka==2).Wartosc_kg);
        }

        [Test]
        public void DodajWartoscMieszankiDoPozycjiListy_GdyListaJestPusta_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => sut.DodajWartoscMieszankiDoPozycjiListy(listaPrzykladowa));
        }

        [Test]
        public void DodajWartoscMieszankiDoPozycjiListy_GdyListaNieJestPusta()
        {
            StworzListeElemenowTestowych();

            var lista = sut.DodajWartoscMieszankiDoPozycjiListy(listaPrzykladowa);

            Assert.AreEqual(4, lista.First().WartoscMieszanki);
        }

        [Test]
        public void ObliczWartoscMieszanki_GdyListaNiepusta_ObliczWartosc()
        {
            StworzListeElemenowTestowych();

            var wartoscMieszanki = sut.ObliczWartoscMieszanki(listaPrzykladowa);

            Assert.AreEqual(4, wartoscMieszanki);
        }

        [Test]
        public void ObliczSredniaCeneMieszankiZaKg_GdySumarycznaIloscJestZerowa_Exception()
        {

            var lista = new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IloscKg=0},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IloscKg=0},
            };

            var cena = sut.ObliczSredniaCeneMieszankiZaKg(lista);

            Assert.AreEqual(0, cena);
        }

        [Test]
        public void ObliczSredniaCeneMieszankiZaKg_GdySumarycznaIloscNieJestZerowa_ObliczCeneSrednia()
        {
            var lista = new List<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
            {
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IloscKg=1, Cena_kg=1},
                new tblProdukcjaZlecenieProdukcyjne_Mieszanka{IDZlecenieProdukcyjneMieszanka=1, IloscKg=1, Cena_kg=1},
            };

            var sredniaCenaMieszanki= sut.ObliczSredniaCeneMieszankiZaKg(lista);

            Assert.AreEqual(1, sredniaCenaMieszanki);
        }
    }
}
