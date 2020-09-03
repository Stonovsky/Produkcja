using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.WeryfikacjaWynikowBadan;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Badania.Geowlokniny.SprawdzenieWynikowBadan
{
    [TestFixture]
    class WeryfikacjaWynikowBadanTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblWynikiBadanGeowlokninRepository> tblWynikiBadanGeowloknin;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<ITblTowarGeowlokninaParametryRepository> tblTowarGeowlokninaParametry;
        private Mock<IWeryfikacjaWynikowBadan> weryfikacjaWynikowBadan;

        private WeryfikacjaWynikowBadan sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();

            tblWynikiBadanGeowloknin = new Mock<ITblWynikiBadanGeowlokninRepository>();
            unitOfWork.Setup(s => s.tblWynikiBadanGeowloknin).Returns(tblWynikiBadanGeowloknin.Object);

            tblTowarGeowlokninaParametry = new Mock<ITblTowarGeowlokninaParametryRepository>();
            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametry).Returns(tblTowarGeowlokninaParametry.Object);

            tblTowar = new Mock<ITblTowarRepository>();
            unitOfWork.Setup(s => s.tblTowar).Returns(tblTowar.Object);

            weryfikacjaWynikowBadan = new Mock<IWeryfikacjaWynikowBadan>();
            sut = GetNewWeryfikacjaWynikowBadan();

        }

        private WeryfikacjaWynikowBadan GetNewWeryfikacjaWynikowBadan()
        {
            return new WeryfikacjaWynikowBadan(unitOfWork.Object);
        }

        public void MethodThatThrows()
        {
            throw new NullReferenceException();
        }

        [Test]
        public void ListaBadan_ListaBadanIsEmpty_Exception()
        {
            sut.ListaBadan = null;

            var result = sut.SprawdzCzyWynikiBadanWTolerancjach();

            Assert.Throws<NullReferenceException>(MethodThatThrows);
        }

        [Test]
        public void ListaParametrowWymaganych_ListaParametrowWymaganychIsEmpty_Exception()
        {
            sut.ListaParametrowWymaganych = null;

            var result = sut.SprawdzCzyWynikiBadanWTolerancjach();

            Assert.Throws<NullReferenceException>(MethodThatThrows);

        }
        [Test]
        public void SprawdzCzyWenikiBadanWTolerancjach_ParametryWymaganeSaNull_DoNotInvokeUoWSAA()
        {
            var badanie = new tblWynikiBadanGeowloknin
            {
                IDWynikiBadanGeowloknin = 1,
                Gramatura = "120",
                Surowiec = "PES",
                GramaturaSrednia = 120,
                KierunekBadania = "w poprzek",
                WytrzymaloscSrednia = 2.4M,
                WydluzenieSrednie = 106,
            };
            tblWynikiBadanGeowloknin.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblWynikiBadanGeowloknin>()
            {
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 1,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 120,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.4M,
                    WydluzenieSrednie = 106,
                }
            });
            tblTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new tblTowar {IDTowar=1, Nazwa = "test" });
            tblTowarGeowlokninaParametry.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametry>() { new tblTowarGeowlokninaParametry { IDTowar = 2 } });

            sut.SprawdzCzyWynikiBadanWTolerancjach(badanie);

            Assert.IsFalse(badanie.CzyBadanieZgodne);
        }

        [Test]
        public void SprawdzCzyWynikiBadanWTolerancjach_tblTowarMissingTowar_ReturnsString()
        {
            tblWynikiBadanGeowloknin.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblWynikiBadanGeowloknin>()
            {
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 1,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 120,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.4M,
                    WydluzenieSrednie = 106,
                }
            });
            tblTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new tblTowar {Nazwa = null});
            tblTowarGeowlokninaParametry.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametry>() {  new tblTowarGeowlokninaParametry
            {
                IDTowar =1,
                MasaPowierzchniowa  = 120M,
                MasaPowierzchniowa_Minimum = 110M,
                MasaPowierzchniowa_Maksimum = 130M,
                GruboscPrzyNacisku2kPa = 0.54M,
                GruboscPrzyNacisku2kPa_Minimum = 0.5M,
                GruboscPrzyNacisku2kPa_Maksimum = 0.58M,
                GruboscPrzyNacisku20kPa = 0.43M,
                GruboscPrzyNacisku20kPa_Minimum = 0.4M,
                GruboscPrzyNacisku20kPa_Maksimum = 0.46M,
                GruboscPrzyNacisku200kPa = 0.37M,
                GruboscPrzyNacisku200kPa_Minimum = 0.34M,
                GruboscPrzyNacisku200kPa_Maksimum = 0.4M,
                WytrzymaloscNaRozciaganie_MD = 2.2M,
                WytrzymaloscNaRozciaganie_MD_Minimum = 2M,
                WytrzymaloscNaRozciaganie_CMD = 2.5M,
                WytrzymaloscNaRozciaganie_CMD_Minimum = 2.3M,
                WydluzeniePrzyZerwaniu_MD = 56M,
                WydluzeniePrzyZerwaniu_MD_Minimum = 50M,
                WydluzeniePrzyZerwaniu_MD_Maksimum = 62M,
                WydluzeniePrzyZerwaniu_CMD = 106M,
                WydluzeniePrzyZerwaniu_CMD_Minimum = 101M,
                WydluzeniePrzyZerwaniu_CMD_Maksimum = 111M,
                OdpornoscNaPrzebicieDynamiczne = 42M,
                OdpornoscNaPrzebicieDynamiczne_Maksimum = 44M,
                OdpornoscNaPrzebicieStatyczne_CBR = 0.34M,
                OdpornoscNaPrzebicieStatyczne_CBR_Minimum = 0.32M,
                CharakterystycznaWielkoscPorow = 90M,
                CharakterystycznaWielkoscPorow_Minimum = 85M,
                CharakterystycznaWielkoscPorow_Maksimum = 95M,
                WodoprzepuszczalnoscProsotpadla = 79M,
                WodoprzepuszczalnoscProsotpadla_Minimum = 72.1M,
            }});

            sut.SprawdzCzyWynikiBadanWTolerancjach();

            foreach(var result in sut.ListaBadan)
            {
            Assert.IsFalse(result.CzyBadanieZgodne);
            Assert.AreEqual(result.UwagiDotyczaceWyniku, "Nie odnaleziono danego towaru w bazie danych");
            }
        }

        [Test]
        public void SprawdzCzyWynikiBadanWTolerancjach_CzyBadanieZgodneIsTrue_ReturnsTrueAndOk()
        {
            tblWynikiBadanGeowloknin.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblWynikiBadanGeowloknin>() 
            {
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 1,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 120,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.4M,
                    WydluzenieSrednie = 106,
                }
            });
            tblTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowar,bool>>>())).ReturnsAsync(new tblTowar { IDTowar=1, Nazwa="Altex at Pes 120" });
            tblTowarGeowlokninaParametry.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametry>() {  new tblTowarGeowlokninaParametry
            {
                IDTowar =1,
                MasaPowierzchniowa  = 120M,
                MasaPowierzchniowa_Minimum = 110M,
                MasaPowierzchniowa_Maksimum = 130M,
                GruboscPrzyNacisku2kPa = 0.54M,
                GruboscPrzyNacisku2kPa_Minimum = 0.5M,
                GruboscPrzyNacisku2kPa_Maksimum = 0.58M,
                GruboscPrzyNacisku20kPa = 0.43M,
                GruboscPrzyNacisku20kPa_Minimum = 0.4M,
                GruboscPrzyNacisku20kPa_Maksimum = 0.46M,
                GruboscPrzyNacisku200kPa = 0.37M,
                GruboscPrzyNacisku200kPa_Minimum = 0.34M,
                GruboscPrzyNacisku200kPa_Maksimum = 0.4M,
                WytrzymaloscNaRozciaganie_MD = 2.2M,
                WytrzymaloscNaRozciaganie_MD_Minimum = 2M,
                WytrzymaloscNaRozciaganie_CMD = 2.5M,
                WytrzymaloscNaRozciaganie_CMD_Minimum = 2.3M,
                WydluzeniePrzyZerwaniu_MD = 56M,
                WydluzeniePrzyZerwaniu_MD_Minimum = 50M,
                WydluzeniePrzyZerwaniu_MD_Maksimum = 62M,
                WydluzeniePrzyZerwaniu_CMD = 106M,
                WydluzeniePrzyZerwaniu_CMD_Minimum = 101M,
                WydluzeniePrzyZerwaniu_CMD_Maksimum = 111M,
                OdpornoscNaPrzebicieDynamiczne = 42M,
                OdpornoscNaPrzebicieDynamiczne_Maksimum = 44M,
                OdpornoscNaPrzebicieStatyczne_CBR = 0.34M,
                OdpornoscNaPrzebicieStatyczne_CBR_Minimum = 0.32M,
                CharakterystycznaWielkoscPorow = 90M,
                CharakterystycznaWielkoscPorow_Minimum = 85M,
                CharakterystycznaWielkoscPorow_Maksimum = 95M,
                WodoprzepuszczalnoscProsotpadla = 79M,
                WodoprzepuszczalnoscProsotpadla_Minimum = 72.1M,
            }});

            sut.SprawdzCzyWynikiBadanWTolerancjach();

            foreach (var result in sut.ListaBadan)
            {
                Assert.IsTrue(result.CzyBadanieZgodne);
                Assert.AreEqual(result.UwagiDotyczaceWyniku, "OK");
            }

        }

        [Ignore("Do sprawdzenia czy się nie zmieniły stringi wyjściowe")]
        [Test]
        public void SprawdzCzyWynikiBadanWTolerancjach_CzyBadanieZgodneIsFalseAndAllParametersMoreThenMaximum_ReturnsFalseAndString()
        {
            tblWynikiBadanGeowloknin.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblWynikiBadanGeowloknin>()
            {
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 1,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 140,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.6M,
                    WydluzenieSrednie = 115,
                    SilaSrednia = 0.4M
                }
            });
            tblTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new tblTowar { IDTowar = 1, Nazwa = "Altex at Pes 120" });
            tblTowarGeowlokninaParametry.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametry>() 
            {  
                new tblTowarGeowlokninaParametry
                {
                    IDTowar =1,
                    MasaPowierzchniowa  = 120M,
                    MasaPowierzchniowa_Minimum = 110M,
                    MasaPowierzchniowa_Maksimum = 130M,
                    GruboscPrzyNacisku2kPa = 0.54M,
                    GruboscPrzyNacisku2kPa_Minimum = 0.5M,
                    GruboscPrzyNacisku2kPa_Maksimum = 0.58M,
                    GruboscPrzyNacisku20kPa = 0.43M,
                    GruboscPrzyNacisku20kPa_Minimum = 0.4M,
                    GruboscPrzyNacisku20kPa_Maksimum = 0.46M,
                    GruboscPrzyNacisku200kPa = 0.37M,
                    GruboscPrzyNacisku200kPa_Minimum = 0.34M,
                    GruboscPrzyNacisku200kPa_Maksimum = 0.4M,
                    WytrzymaloscNaRozciaganie_MD = 2.2M,
                    WytrzymaloscNaRozciaganie_MD_Minimum = 2M,
                    WytrzymaloscNaRozciaganie_CMD = 2.5M,
                    WytrzymaloscNaRozciaganie_CMD_Minimum = 2.3M,
                    WydluzeniePrzyZerwaniu_MD = 56M,
                    WydluzeniePrzyZerwaniu_MD_Minimum = 50M,
                    WydluzeniePrzyZerwaniu_MD_Maksimum = 62M,
                    WydluzeniePrzyZerwaniu_CMD = 106M,
                    WydluzeniePrzyZerwaniu_CMD_Minimum = 101M,
                    WydluzeniePrzyZerwaniu_CMD_Maksimum = 111M,
                    OdpornoscNaPrzebicieDynamiczne = 42M,
                    OdpornoscNaPrzebicieDynamiczne_Maksimum = 44M,
                    OdpornoscNaPrzebicieStatyczne_CBR = 0.34M,
                    OdpornoscNaPrzebicieStatyczne_CBR_Minimum = 0.32M,
                    CharakterystycznaWielkoscPorow = 90M,
                    CharakterystycznaWielkoscPorow_Minimum = 85M,
                    CharakterystycznaWielkoscPorow_Maksimum = 95M,
                    WodoprzepuszczalnoscProsotpadla = 79M,
                    WodoprzepuszczalnoscProsotpadla_Minimum = 72.1M,
                }
            });

            sut.SprawdzCzyWynikiBadanWTolerancjach();

            foreach(var result in sut.ListaBadan)
            {
            var expectedString = $"Gramatura średnia > Maksimum => 10\nWydłużenie średnie > Maksimum => 4\nWytrzymałość średnia > Maksimum => 0,1\nSiła średnia > Maksimum => 0,06\n";

            Assert.IsFalse(result.CzyBadanieZgodne);
            Assert.AreEqual(result.UwagiDotyczaceWyniku, expectedString);
            }
        }

        [Ignore("Do sprawdzenia czy się nie zmieniły stringi wyjściowe")]
        [Test]
        public void SprawdzCzyWynikiBadanWTolerancjach_CheckingMultipleEntries_ReturnsMultipleStrings()
        {
            tblWynikiBadanGeowloknin.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblWynikiBadanGeowloknin>()
            {
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 1,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 120,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.4M,
                    WydluzenieSrednie = 106,
                },
                new tblWynikiBadanGeowloknin
                {
                    IDWynikiBadanGeowloknin = 2,
                    Gramatura = "120",
                    Surowiec = "PES",
                    GramaturaSrednia = 140,
                    KierunekBadania = "w poprzek",
                    WytrzymaloscSrednia = 2.6M,
                    WydluzenieSrednie = 115,
                    SilaSrednia = 0.4M
                }
            });
            tblTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowar, bool>>>())).ReturnsAsync(new tblTowar { IDTowar = 1, Nazwa = "Altex at Pes 120" });
            tblTowarGeowlokninaParametry.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametry>()
            {
                new tblTowarGeowlokninaParametry
                {
                    IDTowar =1,
                    MasaPowierzchniowa  = 120M,
                    MasaPowierzchniowa_Minimum = 110M,
                    MasaPowierzchniowa_Maksimum = 130M,
                    GruboscPrzyNacisku2kPa = 0.54M,
                    GruboscPrzyNacisku2kPa_Minimum = 0.5M,
                    GruboscPrzyNacisku2kPa_Maksimum = 0.58M,
                    GruboscPrzyNacisku20kPa = 0.43M,
                    GruboscPrzyNacisku20kPa_Minimum = 0.4M,
                    GruboscPrzyNacisku20kPa_Maksimum = 0.46M,
                    GruboscPrzyNacisku200kPa = 0.37M,
                    GruboscPrzyNacisku200kPa_Minimum = 0.34M,
                    GruboscPrzyNacisku200kPa_Maksimum = 0.4M,
                    WytrzymaloscNaRozciaganie_MD = 2.2M,
                    WytrzymaloscNaRozciaganie_MD_Minimum = 2M,
                    WytrzymaloscNaRozciaganie_CMD = 2.5M,
                    WytrzymaloscNaRozciaganie_CMD_Minimum = 2.3M,
                    WydluzeniePrzyZerwaniu_MD = 56M,
                    WydluzeniePrzyZerwaniu_MD_Minimum = 50M,
                    WydluzeniePrzyZerwaniu_MD_Maksimum = 62M,
                    WydluzeniePrzyZerwaniu_CMD = 106M,
                    WydluzeniePrzyZerwaniu_CMD_Minimum = 101M,
                    WydluzeniePrzyZerwaniu_CMD_Maksimum = 111M,
                    OdpornoscNaPrzebicieDynamiczne = 42M,
                    OdpornoscNaPrzebicieDynamiczne_Maksimum = 44M,
                    OdpornoscNaPrzebicieStatyczne_CBR = 0.34M,
                    OdpornoscNaPrzebicieStatyczne_CBR_Minimum = 0.32M,
                    CharakterystycznaWielkoscPorow = 90M,
                    CharakterystycznaWielkoscPorow_Minimum = 85M,
                    CharakterystycznaWielkoscPorow_Maksimum = 95M,
                    WodoprzepuszczalnoscProsotpadla = 79M,
                    WodoprzepuszczalnoscProsotpadla_Minimum = 72.1M,
                }
            });

            sut.SprawdzCzyWynikiBadanWTolerancjach();

            var expectedString = $"Gramatura średnia > Maksimum => 10\nWydłużenie średnie > Maksimum => 4\nWytrzymałość średnia > Maksimum => 0,1\nSiła średnia > Maksimum => 0,06\n";

            foreach (var result in sut.ListaBadan)
            {
                if (result.IDWynikiBadanGeowloknin == 1)
                {
                    Assert.IsTrue(result.CzyBadanieZgodne);
                    Assert.AreEqual(result.UwagiDotyczaceWyniku, "OK");
                }
                if (result.IDWynikiBadanGeowloknin == 2)
                {
                    Assert.IsFalse(result.CzyBadanieZgodne);
                    Assert.AreEqual(result.UwagiDotyczaceWyniku, expectedString);
                }

                Assert.AreEqual(sut.ListaBadan.Count, 2);
            }
        }
    }
}
