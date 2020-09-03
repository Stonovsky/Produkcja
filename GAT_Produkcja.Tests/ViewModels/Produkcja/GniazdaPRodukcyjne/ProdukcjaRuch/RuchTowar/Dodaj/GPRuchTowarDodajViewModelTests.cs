using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.ScaleService;
using GAT_Produkcja.Utilities.ZebraPrinter.S4M;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowaru.Dodaj
{
    [TestFixture]
    public class GPRuchTowarDodajViewModelTests : TestBase
    {

        private Fixture fixture;
        private Mock<IScaleLP7510Reader> scale;
        private Mock<IWeryfikacjaGramaturyGeowlokninHelper> weryfikacjaGramautry;
        private Mock<IGPRuchTowarBadaniaViewModel> vmBadania;
        private Mock<IZebraS4MService> printer;
        private Mock<IGPRuchTowar_RolkaHelper> rolkaHelper;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblTowarGeowlokninaParametryGramatura;
        private Mock<ITblTowarGeowlokninaParametrySurowiecRepository> tblTowarGeowlokninaParametrySurowiec;
        private GPRuchTowarDodajViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fixture = new Fixture();
            scale = new Mock<IScaleLP7510Reader>();
            weryfikacjaGramautry = new Mock<IWeryfikacjaGramaturyGeowlokninHelper>();
            vmBadania = new Mock<IGPRuchTowarBadaniaViewModel>();
            printer = new Mock<IZebraS4MService>();
            rolkaHelper = new Mock<IGPRuchTowar_RolkaHelper>();

            tblTowarGeowlokninaParametryGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblTowarGeowlokninaParametryGramatura.Object);

            tblTowarGeowlokninaParametrySurowiec = new Mock<ITblTowarGeowlokninaParametrySurowiecRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametrySurowiec).Returns(tblTowarGeowlokninaParametrySurowiec.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new GPRuchTowarDodajViewModel(ViewModelService.Object, rolkaHelper.Object, scale.Object, weryfikacjaGramautry.Object,vmBadania.Object,printer.Object);
        }

        #region Messenger
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenie>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieCiecia>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<DodajEdytujGPRuchTowarMessage>>(), It.IsAny<bool>()));
        }

        #region GdyPrzeslanoStatusRuchu
        [Test]
        public void GdyPrzeslanoStatusRuchu_GdyPrzeslanoRW_PrzypisujeTytulWlasciwy()
        {

            MessengerSend(
                new DodajEdytujGPRuchTowarMessage
                {
                    RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                    DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                    RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }

                });


            Assert.IsTrue(sut.Tytul.Contains("RW"));
        }
        [Test]
        public void GdyPrzeslanoStatusRuchu_GdyPrzeslanoPW_PrzypisujeTytulWlasciwy()
        {
            MessengerSend(
                new DodajEdytujGPRuchTowarMessage
                {
                    RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                    DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                    RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }

                });

            Assert.IsTrue(sut.Tytul.Contains("PW"));
        }

        #region GdyPrzeslanoRuchTowaru
        [Test]
        public void GdyPrzeslanoRuchTowaru_ZmianaWybranejGramaturyOrazSurowca()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = 1 },
                RuchTowar = new tblProdukcjaRuchTowar { IDTowarGeowlokninaParametrySurowiec = 1, IDGramatura = 1 }
            },
            () =>
            {
                sut.ListaGramatur = new List<tblTowarGeowlokninaParametryGramatura>
                    {
                        new tblTowarGeowlokninaParametryGramatura{ IDTowarGeowlokninaParametryGramatura=1, Gramatura=100}
                    };
                sut.ListaRodzajowSurowca = new List<tblTowarGeowlokninaParametrySurowiec>
                    {
                        new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=1, Skrot="PP"}
                    };
            }
            );

            Assert.AreEqual(1, sut.WybranaGramatura.IDTowarGeowlokninaParametryGramatura);
            Assert.AreEqual(1, sut.WybranySurowiec.IDTowarGeowlokninaParametrySurowiec);
        }
        #endregion

        #endregion

        #region GdyPrzeslanoDodajEdytujMessage
        [Test]
        public void GdyPrzeslanoDodajEdytujMessage_GdyMessageNull_NicNieRob()
        {
            MessengerSend((DodajEdytujGPRuchTowarMessage)null);

            Assert.IsTrue(sut.StatusRuchu.IDRuchStatus == 0);
        }

        [Test]
        public void GdyPrzeslanoDodajEdytujMessage_PrzypiszStatusRuchu()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = 1 },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            });

            Assert.IsTrue(sut.StatusRuchu.IDRuchStatus == 1);
            Assert.IsTrue(sut.RuchTowar.IDProdukcjaRuchTowar == 1);
            Assert.IsTrue(sut.DodajEdytujStatusEnum == DodajUsunEdytujEnum.Dodaj);
        }

        [Test]
        public void GdyPrzeslanoDodajEdytujMessage_UzupelnijDaneTowaru()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            });

            Assert.IsTrue(sut.RuchTowar.DataDodania!=default);
            Assert.IsNotNull(sut.RuchTowar.KodKreskowy);
        }



        [Test]
        [TestCase(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, "RW")]
        [TestCase(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, "PW")]
        public void GdyPrzeslanoDodajEdytujMessage_PrzypisujeTytulWlasciwyZgodnyZeStatusem(StatusRuchuTowarowEnum statusRuchuTowarowEnum, string expected)
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)statusRuchuTowarowEnum },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            });

            Assert.IsTrue(sut.Tytul.Contains(expected));
        }
        #endregion

        #region KodKreskowy
        [Test]
        public void GdyPrzeslanoDodajEdytujMessage_GdyEnumDodaj_GenerujNowyKodKreskowy()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            });

            Assert.IsNotNull(sut.RuchTowar.KodKreskowy);
            Assert.IsNotEmpty(sut.RuchTowar.KodKreskowy);
            Assert.AreNotEqual("0123456789111", sut.RuchTowar.KodKreskowy);
        }

        [Test]
        public void GdyPrzeslanoDodajEdytujMessage_GdyEnumEdytuj_NieGenerujeNowegoKoduKreskowego()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, KodKreskowy = "0123456789111" }
            });

            Assert.IsNotEmpty(sut.RuchTowar.KodKreskowy);
            Assert.AreEqual("0123456789111", sut.RuchTowar.KodKreskowy);
        }

        #endregion

        #endregion

        #region LoadCommand
        [Test]
        public void LoadCommandExecute_PobieraNiezbedneDaneZBazy()
        {

            sut.LoadCommand.Execute(null);

            tblTowarGeowlokninaParametryGramatura.Verify(v => v.GetAllAsync());
            tblTowarGeowlokninaParametrySurowiec.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommand_UruchamiaMetodyZalezne()
        {
            sut.LoadCommand.Execute(null);

            printer.Verify(x => x.LoadAsync());
            scale.Verify(v => v.LoadAsync());
        }

        [Test]
        public void LoadCommandExecute_PoPobraniu_KlonujeTowar()
        {
            tblTowarGeowlokninaParametryGramatura.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametryGramatura>
            {
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=1}
            });

            sut.LoadCommand.Execute(null);

            Assert.IsFalse(sut.IsChanged);

        }
        #endregion

        #region SaveCommand
        #region CanExecute
        [Test]
        public void SaveCommandCanExecute_GdyTowarIsNotValid_False()
        {
            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        private void TowarRW_Valid()
        {
            //sut.RuchTowar = fixture.Build<tblProdukcjaRuchTowar>()
            //    .Without(w => w.tblProdukcjaRozliczenieStatus)
            //    .Without(w => w.tblProdukcjaRuchNaglowek)
            //    .Without(w => w.tblProdukcjaRuchNaglowek.tblProdukcjaGniazdoProdukcyjne)
            //    .Without(w => w.tblProdukcjaRuchNaglowek.tblProdukcjaZlcecenieProdukcyjne)
            //    .Without(w => w.tblProdukcjaRuchNaglowek.tblProdukcjaZlecenieCiecia)
            //    .Without(w => w.tblProdukcjaRuchTowarWyjsciowy)
            //    .Without(w => w.tblProdukcjaZlecenieTowar)
            //    .Without(w => w.tblRuchStatus)
            //    .Without(w => w.tblTowarGeowlokninaParametryGramatura)
            //    .Without(w => w.tblTowarGeowlokninaParametrySurowiec)
            //    .Without(w => w.tblProdukcjaZlcecenieProdukcyjne)
            //    .Without(w => w.tblProdukcjaGniazdoProdukcyjne)
            //    .Without(w => w.tblProdukcjaRuchTowarStatus)
            //    .Create();
            //sut.RuchTowar.Dlugosc_m = 100M;
            //sut.RuchTowar.Szerokosc_m = 1M;

            sut.RuchTowar = new tblProdukcjaRuchTowar
            {
                IDGramatura=1,
                IDTowarGeowlokninaParametrySurowiec=1,
                Gramatura=100,
                Dlugosc_m = 100M,
                Szerokosc_m = 1M,
                Waga_kg=1,
                Ilosc_m2=1,
            };
        }

        [Test]
        public void SaveCommandCanExecute_GdyTowarIsValidIIsChanged_True_True()
        {
            TowarRW_Valid();
            sut.RuchTowar.Dlugosc_m = 100M;
            sut.RuchTowar.Szerokosc_m = 1M;

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        [Test]
        public void SaveCommandCanExecute_GdyTowarIsValidOrazIsChanged_False_False()
        {
            TowarRW_Valid();
            sut.RuchTowar.Ilosc_m2 = 100M;
            sut.IsChanged_False();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void SaveCommandExecute_PoZapisieWysylaMessage()
        {
            TowarRW_Valid();
            sut.StatusRuchu = new tblRuchStatus { IDRuchStatus = 1 };

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<DodajEdytujGPRuchTowarMessage>()));
        }

        [Test]
        public void SaveCommandExecute_PoZapisieZamykaOkno()
        {
            TowarRW_Valid();
            sut.StatusRuchu = new tblRuchStatus { IDRuchStatus = 1 };

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion
        #endregion

        #region PrintCECommand
        private void PrintCECommandCanExecute_True()
        {
            sut.RuchTowar = new tblProdukcjaRuchTowar
            {
                IDGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Gramatura = 100,
                Dlugosc_m = 100M,
                Szerokosc_m = 1M,
                Waga_kg = 1,
                Ilosc_m2 = 1,
            };
            printer.Setup(x => x.CanPrint()).Returns(true);
        }
        #endregion

        [Test]
        public void PrintCECommandCanExecute_WhenRuchTowarIsNotNullAndPrinterCanPrint_True()
        {
            PrintCECommandCanExecute_True();

            var result = sut.PrintCECommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void PrintCECommandCanExecute_WhenRuchTowarIsValidButPrinterCannotPrint_True()
        {
            PrintCECommandCanExecute_True();

            printer.Setup(x => x.CanPrint()).Returns(false);

            var result = sut.PrintCECommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        #region Properties

        [Test]
        public void WybranaGramatura_GdyZmiana_PrzypiszGramatureDoTowarRW()
        {
            sut.WybranaGramatura = new tblTowarGeowlokninaParametryGramatura { IDTowarGeowlokninaParametryGramatura = 1, Gramatura = 100 };

            Assert.AreEqual(100, sut.RuchTowar.Gramatura);
        }

        [Test]
        public void WybranySurowiec_GdyZmiana_IdSurowcaPrzypisaneDoTowarRW()
        {
            sut.WybranySurowiec = new tblTowarGeowlokninaParametrySurowiec { IDTowarGeowlokninaParametrySurowiec = 1, Nazwa = "Poliester", Skrot = "PES" };

            Assert.AreEqual(1, sut.RuchTowar.IDTowarGeowlokninaParametrySurowiec);
        }

        #region CzyStronaRolkiWidoczna
        [Test]
        public void CzyStronaRolkiWidoczna_GniazdoLiniaWloknin_True()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin },
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW }
            });

            Assert.IsTrue(sut.CzyStronaRolkiWidoczna);
        }

        [Test]
        public void CzyStronaRolkiWidoczna_GniazdoInneNizLiniaWloknin_False()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania },
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW }
            });

            Assert.IsFalse(sut.CzyStronaRolkiWidoczna);
        }
        #endregion

        #region WybranaStronaRolki
        [Test]
        public void WybranaStronaRolki_GdyPrzypisano_DodajeDoKoncaPelnegoNrRolki()
        {
            sut.RuchTowar = new tblProdukcjaRuchTowar { NrRolkiPelny = "W123" };

            sut.WybranaStronaRolki = "L";

            Assert.IsTrue(sut.RuchTowar.NrRolkiPelny.EndsWith("L"));
        }
        #endregion
        #endregion
    }
}
