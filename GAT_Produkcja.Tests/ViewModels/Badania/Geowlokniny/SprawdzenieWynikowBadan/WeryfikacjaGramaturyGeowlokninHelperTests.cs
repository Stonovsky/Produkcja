using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GAT_Produkcja.Tests.ViewModels.Badania.Geowlokniny.SprawdzenieWynikowBadan
{
    class WeryfikacjaGramaturyGeowlokninHelperTests : TestBaseGeneric<WeryfikacjaGramaturyGeowlokninHelper>
    {
        private Mock<ITblTowarGeowlokninaParametryRepository> tblTowarGeowlokninaParametry;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblTowarGeowlokninaParametryGramatura;

        public override void SetUp()
        {
            base.SetUp();

            tblTowarGeowlokninaParametry = new Mock<ITblTowarGeowlokninaParametryRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametry).Returns(tblTowarGeowlokninaParametry.Object);

            tblTowarGeowlokninaParametryGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblTowarGeowlokninaParametryGramatura.Object);

            DaneZBazy();
            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new WeryfikacjaGramaturyGeowlokninHelper(UnitOfWork.Object);
        }

        private void DaneZBazy()
        {
            tblTowarGeowlokninaParametry.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblTowarGeowlokninaParametry, bool>>>()))
                                        .ReturnsAsync(new List<tblTowarGeowlokninaParametry>
                                        {
                                            new tblTowarGeowlokninaParametry
                                            {
                                                IDTowarGeowlokninaParametrySurowiec=1,
                                                MasaPowierzchniowa=90,
                                                MasaPowierzchniowa_Maksimum=90+10,
                                                MasaPowierzchniowa_Minimum=90-10,
                                                IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_90
                                            },
                                            new tblTowarGeowlokninaParametry
                                            {
                                                IDTowarGeowlokninaParametrySurowiec=1,
                                                MasaPowierzchniowa=100,
                                                MasaPowierzchniowa_Maksimum=100+10,
                                                MasaPowierzchniowa_Minimum=100-10,
                                                IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_100
                                            },
                                            new tblTowarGeowlokninaParametry
                                            {
                                                IDTowarGeowlokninaParametrySurowiec=1,
                                                MasaPowierzchniowa=150,
                                                MasaPowierzchniowa_Maksimum=150+10,
                                                MasaPowierzchniowa_Minimum=150-10,
                                                IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_150
                                            },
                                            new tblTowarGeowlokninaParametry
                                            {
                                                IDTowarGeowlokninaParametrySurowiec=1,
                                                MasaPowierzchniowa=200,
                                                MasaPowierzchniowa_Maksimum=200+10,
                                                MasaPowierzchniowa_Minimum=200-10,
                                                IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_200
                                            },
                                            new tblTowarGeowlokninaParametry
                                            {
                                                IDTowarGeowlokninaParametrySurowiec=1,
                                                MasaPowierzchniowa=300,
                                                MasaPowierzchniowa_Maksimum=300+10,
                                                MasaPowierzchniowa_Minimum=300-10,
                                                IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_300
                                            }
                                        });


            tblTowarGeowlokninaParametryGramatura.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametryGramatura>
            {
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_90},
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_100},
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_150},
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_200},
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=(int) TowarGeowlokninaGramaturaEnum.Gramatura_300},
            });
        }

        [Test]
        [TestCase(90, 3, true)]
        [TestCase(100, 3, true)]
        [TestCase(80, 3, true)]
        [TestCase(101, 3, false)]
        [TestCase(79, 3, false)]
        public void CzyGramaturaZgodna_GdyJestOk_True(decimal gramatura, int idGramatura, bool resultPredicted)
        {
            sut.LoadAsync();

            var result = sut.CzyGramaturaZgodna(gramatura, idGramatura);

            Assert.AreEqual(resultPredicted, result);
        }


        [Test]
        public void PobierzWlasciwaGramatureId_GdyGramaturMniejszaNizMinimalnaMozliwa_Wyjatek()
        {
            sut.LoadAsync();

            var towarRuch = new tblProdukcjaRuchTowar { IDTowarGeowlokninaParametrySurowiec = 1 };

            Assert.Throws<GeowlokninaGramaturaException>(() => sut.PobierzWlasciwaGramature(78, towarRuch));

        }

        [Test]
        [TestCase(102, (int)TowarGeowlokninaGramaturaEnum.Gramatura_100)]
        [TestCase(141, (int)TowarGeowlokninaGramaturaEnum.Gramatura_150)]
        [TestCase(135, (int)TowarGeowlokninaGramaturaEnum.Gramatura_100)]
        [TestCase(350, (int)TowarGeowlokninaGramaturaEnum.Gramatura_300)]
        [TestCase(180, (int)TowarGeowlokninaGramaturaEnum.Gramatura_150)]
        [TestCase(89, (int)TowarGeowlokninaGramaturaEnum.Gramatura_90)]
        [TestCase(80, (int)TowarGeowlokninaGramaturaEnum.Gramatura_90)]

        public void PobierzWlasciwaGramatureId_GdyGramaturaZaWysoka_ZmieniaGramatureNaWlasciwa(decimal gramatura, int nowaGramaturaPrzewidywana)
        {
            sut.LoadAsync();

            var towarRuch = new tblProdukcjaRuchTowar { IDTowarGeowlokninaParametrySurowiec = 1 };

            var idGramaturaNowa = sut.PobierzWlasciwaGramature(gramatura, towarRuch);

            Assert.AreEqual(nowaGramaturaPrzewidywana, idGramaturaNowa.IDTowarGeowlokninaParametryGramatura);
        }
    }
}
