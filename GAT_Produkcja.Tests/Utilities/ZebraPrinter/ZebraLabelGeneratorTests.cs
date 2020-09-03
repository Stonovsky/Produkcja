using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.ZebraPrinter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ZebraPrinter
{
    [TestFixture]
    public class ZebraLabelGeneratorTests
    {
        private ZebraLabelGenerator sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ZebraLabelGenerator();
        }

        [Test]
        public void GenerateLabel_LabelModelIsNull_ReturnsEmptyString()
        {
            //Arrange
            LabelModel label = null;

            //Act
            string result = sut.EtykietaProdukcja(label);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GenerateLabel_LabelModelIsNotNull_GeneratedStringIsNotEmpty()
        {
            //Arrange

            LabelModel label = new LabelModel
            {
                RodzajSurowca = "PP",
                SzerokoscRolki = 2,
                DlugoscNawoju = 100,
                Kalandrowana = true,
                Gramatura = 120,
                IloscEtykietDoDruku = 1,
                TypKoduKreskowego=TypKoduKreskowegoEnum.EAN13
                
            };

            //Act
            string result = sut.EtykietaProdukcja(label);

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Ignore("Brak asserta, błędne odwołanie do klasy ZebraLabelPrinter - testujemy ZebraLabelGenerator")]
        [Test]
        public void PrintProductionLabel()
        {
            var zebraPrinter = new ZebraLabelPrinter();
            UzytkownikZalogowany.Uzytkownik = new tblPracownikGAT { Imie = "Danuta", Nazwisko = "Kowalska" };

            LabelModel label = new LabelModel
            {
                NrZP="1",
                RodzajSurowca = "PP",
                SzerokoscRolki = 2,
                DlugoscNawoju = 100,
                Kalandrowana = true,
                Gramatura = 120,
                IloscEtykietDoDruku = 1,
                KodKreskowy="123123123123",
                IsValid=true,
                TypKoduKreskowego=TypKoduKreskowegoEnum.EAN13
            };
            
            var result = sut.EtykietaProdukcja(label);

            zebraPrinter.PrintAsync(result);
        }


        [Ignore("Brak asserta, błędne odwołanie do klasy ZebraLabelPrinter - testujemy ZebraLabelGenerator")]
        [Test]
        public void PrintCELabel()
        {
            var zebraPrinter = new ZebraLabelPrinter();
            var parametryGeowlokniny = new tblTowarGeowlokninaParametry
            {
                IDTowar = 1,
                IDCertyfikatCE = 1,
                DataBadania = new DateTime(2019, 11, 1),
                NrDWU = "test"

            };
            var labelModel = new LabelModel
            {
                DlugoscNawoju = 100,
                SzerokoscRolki = 2,
                Gramatura = 100,
                RodzajSurowca = "PP"
            };
            var result = sut.EtykietaCE_PL(labelModel, parametryGeowlokniny);

            zebraPrinter.PrintAsync(result);
        }
    }
}
