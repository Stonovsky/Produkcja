using GAT_Produkcja.db;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_KonfekcjaHelperTests : TestBaseGeneric<GPRuchTowar_PW_KonfekcjaHelper>
    {
        public override void SetUp()
        {
            base.SetUp();

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowar_PW_KonfekcjaHelper(DialogService.Object,UnitOfWork.Object);
        }

        #region CzyIloscM2ZgodnaZRolkaRW

        /// <summary>
        /// Gdy brak rolki RW - sytuacja dla gniazda Wloknin gdzie nie bedzie rolek RW
        /// </summary>
        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_GdyBrakRolki_ZwracaTrue()
        {
            var result = sut.CzyIloscM2ZgodnaZRolkaRW(null, new tblProdukcjaRuchTowar(), new List<tblProdukcjaRuchTowar>());

            Assert.IsTrue(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_GdyRolkaRwWiekszaNizSumaPW_ZwracaTrue()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar=1, Ilosc_m2 = 5 };
            var rolkaPW = new tblProdukcjaRuchTowar { Ilosc_m2 = 1, IDRolkaBazowa=1 };
            var listaRolekPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
            };

            var result = sut.CzyIloscM2ZgodnaZRolkaRW(rolkaRW, rolkaPW, listaRolekPW);

            Assert.IsTrue(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_GdyRolkaRwRownaSumiePW_ZwracaTrue()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar=1, Ilosc_m2 = 5 };
            var rolkaPW = new tblProdukcjaRuchTowar { Ilosc_m2 = 3 ,IDRolkaBazowa=1};
            var listaRolekPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
            };

            var result = sut.CzyIloscM2ZgodnaZRolkaRW(rolkaRW, rolkaPW, listaRolekPW);

            Assert.IsTrue(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_GdyRolkaRwMniejszaNizSumaPW_ZwracaFalse()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar=1, Ilosc_m2 = 5 };
            var rolkaPW = new tblProdukcjaRuchTowar { Ilosc_m2 = 3 ,IDRolkaBazowa=1};
            var listaRolekPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1, IDRolkaBazowa=2},
            };

            var result = sut.CzyIloscM2ZgodnaZRolkaRW(rolkaRW, rolkaPW, listaRolekPW);

            Assert.IsFalse(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_BrakListyPW_Wyjatek()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { Ilosc_m2 = 5 };
            var rolkaPW = new tblProdukcjaRuchTowar { Ilosc_m2 = 3 };


            Assert.Throws<ArgumentException>(() => sut.CzyIloscM2ZgodnaZRolkaRW(rolkaRW, rolkaPW, null));
        }

        [Test]
        public void CzyIloscM2ZgodnaZRolkaRW_BrakRolkiPW_True()
        {
            var rolkaRW = new tblProdukcjaRuchTowar { Ilosc_m2 = 5 };
            var rolkaPW = new tblProdukcjaRuchTowar { Ilosc_m2 = 3 };
            var listaRolekPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ Ilosc_m2=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1},
                new tblProdukcjaRuchTowar{ Ilosc_m2=1},
            };

            var result = sut.CzyIloscM2ZgodnaZRolkaRW(rolkaRW, null, listaRolekPW);

            Assert.IsTrue(result);
        }
        #endregion

        #region CzyIloscM2ZgodnaZeZleceniem

        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyIloscPoDodaniuRolkiPW_MnijeszaNizNaZleceniu_True()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };
            var rolkaPW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Ilosc_m2 = 1 };
            var listaPW = new List<tblProdukcjaRuchTowar> { new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 } };

            var result = sut.CzyIloscM2ZgodnaZeZleceniem(zlecenieTowar, rolkaPW, listaPW);

            Assert.IsTrue(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyIloscPoDodaniuRolkiPW_WiekszaNizNaZleceniu_False()
        {

            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };
            var rolkaPW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Ilosc_m2 = 1 };
            var listaPW = new List<tblProdukcjaRuchTowar> 
            { 
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 }, 
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 }, 
            };
            
            var result = sut.CzyIloscM2ZgodnaZeZleceniem(zlecenieTowar, rolkaPW, listaPW);

            Assert.IsFalse(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyZlecenieTowarNull_False()
        {

            var rolkaPW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Ilosc_m2 = 1 };
            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
            };

            var result = sut.CzyIloscM2ZgodnaZeZleceniem(null, rolkaPW, listaPW);

            Assert.IsFalse(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyRolkaPWNull_False()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };
            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
            };

            var result = sut.CzyIloscM2ZgodnaZeZleceniem(zlecenieTowar, null, listaPW);

            Assert.IsFalse(result);
        }

        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyListaPWNull_PWmniejszeOdZlecenieTowar_True()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };
            var rolkaPW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Ilosc_m2 = 1 };
            var listaPW = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
                new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, Ilosc_m2 = 1 },
            };

            var result = sut.CzyIloscM2ZgodnaZeZleceniem(zlecenieTowar, rolkaPW, null);

            Assert.IsTrue(result);
        }



        [Test]
        public void CzyIloscM2ZgodnaZeZleceniem_GdyListaPWPusta_PWmniejszeOdZlecenieTowar_True()
        {
            var zlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Ilosc_m2 = 2 };
            var rolkaPW = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Ilosc_m2 = 1 };
            var listaPW = new List<tblProdukcjaRuchTowar>();

            var result = sut.CzyIloscM2ZgodnaZeZleceniem(zlecenieTowar, rolkaPW, listaPW);

            Assert.IsTrue(result);
        }

        #endregion
    }
}
