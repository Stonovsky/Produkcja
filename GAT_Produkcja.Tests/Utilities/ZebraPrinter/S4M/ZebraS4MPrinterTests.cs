using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.ZebraPrinter.S4M;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zebra.Sdk.Printer;

namespace GAT_Produkcja.Tests.Utilities.ZebraPrinter.S4M
{
    [TestFixture]
    public class ZebraS4MPrinterTests
    {
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITblTowarGeowlokninaParametryRepository> tblTowarGeowlokninaParametry;
        private ZebraS4MPrinter sut;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();

            tblTowarGeowlokninaParametry = new Mock<ITblTowarGeowlokninaParametryRepository>();
            unitOfWork.Setup(s => s.tblTowarGeowlokninaParametry).Returns(tblTowarGeowlokninaParametry.Object);
            
            sut = new ZebraS4MPrinter();
        }

        /// <summary>
        /// Stosowac gdy drukarka podlaczona
        /// </summary>
        [Test]
        [Ignore("Stosowac gdy drukarka podlaczona")]
        public void Print_GdyDaneOk_Drukuj()
        {
            UzytkownikZalogowany.Uzytkownik = new tblPracownikGAT { Imie = "To", Nazwisko = "Str" };
            var towar = new tblProdukcjaRuchTowar
            {
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura { Gramatura = 100 },
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { Skrot = "PP" },
                Szerokosc_m = 2.15m,
                Dlugosc_m = 150,
                KodKreskowy = "123456789012",
                DataDodania = DateTime.Now.Date,
                IDProdukcjaGniazdoProdukcyjne=1,
                NrDokumentu="1WL1234",
            };
            var zlpMessage = new ZebraZLPLabelGenerator().GetInternalHorizontalLabel(towar,1);

            sut.Print("192.168.34.80", zlpMessage);
        }

        [Test]
        [Ignore("Wlaczyc test gdy Zebra jest podlaczona")]
        public async Task EtykietaCE_Condition_Expectations()
        {
            tblTowarGeowlokninaParametry.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblTowarGeowlokninaParametry, bool>>>()))
                                        .ReturnsAsync(new db.tblTowarGeowlokninaParametry
                                        {
                                            WytrzymaloscNaRozciaganie_MD = 10,
                                            WytrzymaloscNaRozciaganie_MD_Minimum = 9,
                                            WydluzeniePrzyZerwaniu_CMD = 100.5m,
                                            WydluzeniePrzyZerwaniu_MD = 100.6m,
                                            OdpornoscNaPrzebicieStatyczne_CBR = 0.9m,
                                            OdpornoscNaPrzebicieStatyczne_CBR_Minimum = 0.8m,
                                            OdpornoscNaPrzebicieDynamiczne = 1.50m,
                                            OdpornoscNaPrzebicieDynamiczne_Maksimum = 1.80m,
                                            WodoprzepuszczalnoscWPlaszczyznie_20kPa = 2,
                                            WodoprzepuszczalnoscWPlaszczyznie_20kPa_Minimum = 1,
                                            WodoprzepuszczalnoscWPlaszczyznie_100kPa = 100,
                                            WodoprzepuszczalnoscWPlaszczyznie_100kPa_Minimum = 90,
                                            WodoprzepuszczalnoscWPlaszczyznie_200kPa = 120,
                                            WodoprzepuszczalnoscWPlaszczyznie_200kPa_Minimum = 100,
                                            CharakterystycznaWielkoscPorow = 105,
                                            WodoprzepuszczalnoscProsotpadla = 10,
                                            WodoprzepuszczalnoscProsotpadla_Minimum = 9,
                                            OdpornoscNaWarunkiAtmosferyczne = "test",
                                            OdpornoscNaUtlenianie = 100,
                                            DataBadania=DateTime.Now.Date,
                                            NrDWU="ALTEX AT 100 15052020",
                                        });

            var towar = new tblProdukcjaRuchTowar
            {
                Szerokosc_m = 2.10m,
                Dlugosc_m = 50,
                IDGramatura = (int)TowarGeowlokninaGramaturaEnum.Gramatura_100,
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura { Gramatura = 100 },
                IDTowarGeowlokninaParametrySurowiec = (int)TowarGeowlokninaSurowiecEnum.PP,
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec { Skrot = "PP" }
            };
                
            var zplMessage = await new ZebraZPLCELabelGenerator(unitOfWork.Object).GetLabelCE(towar,1);
            sut.Print("192.168.34.80", zplMessage);

        }

        [Test]
        [Ignore("PrinterStatus jest abstract i trzeba wprowadzic dziedzicenie i interface do testow." +
                "Z uwagi na to, iż jest to klasa pobrana od Zebry nie bedziemy jej sprawdzac")]
        public void IsPrinterReady_StatusReady_True()
        {
            var moqPrinterStatus = new Mock<PrinterStatus>();
            moqPrinterStatus.SetupGet(s => s.isReadyToPrint).Returns(true);

            var isReady = sut.IsPrinterReady(moqPrinterStatus.Object);

            Assert.IsTrue(isReady);
        }

    }
}
