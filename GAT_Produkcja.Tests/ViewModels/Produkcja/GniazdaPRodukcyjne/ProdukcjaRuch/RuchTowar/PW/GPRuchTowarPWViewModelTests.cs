using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Decorator;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW
{
    public class GPRuchTowarPWViewModelTests : TestBase
    {

        private Messenger messengerOrg;
        private Mock<IGPRuchTowar_PW_Helper> pwHelper;
        private Mock<IGPRuchTowar_PW_ZaawansowanieHelper> zaawansowanieHelper;
        private Mock<IGPRuchTowar_PW_PodsumowanieHelper> podsumowanieHelper;
        private Mock<IGPRuchTowar_RolkaHelper> rolkaHelper;
        private Mock<IGPRuchTowar_PW_KonfekcjaHelper> konfekcjaHelper;
        private Mock<IGPRuchTowar_PW_RolkaBazowaHelper> rolkaBazowaHelper;
        private Mock<IGPRuchTowar_PW_PodsumowanieStrategyFactory> podsumowanieStrategy;
        private Mock<IGPRuchTowar_PW_PodsumowanieHelper> podsumowanie;
        private Mock<ITblTowarRepository> tblTowar;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblTowarGeowlokninaParametryGramatura;
        private Mock<ITblTowarGeowlokninaParametrySurowiecRepository> tblTowarGeowlokninaParametrySurowiec;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;
        private Mock<ITblProdukcjaZlecenieRepository> tblProdukcjaZlcecenie;
        private Mock<ITblProdukcjaZlecenieCieciaRepository> tblProdukcjaZlecenieCiecia;
        private GPRuchTowarPWViewModel sut;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;

        public Mock<RolkaPWDecorator> RolkaPWDecorator { get; private set; }
        public Mock<PWChildObjectHelper> PWChildObjectHelper { get; private set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            pwHelper = new Mock<IGPRuchTowar_PW_Helper>();

            #region Helper
            zaawansowanieHelper = new Mock<IGPRuchTowar_PW_ZaawansowanieHelper>();
            pwHelper.Setup(s => s.ZaawansowanieHelper).Returns(zaawansowanieHelper.Object);

            podsumowanieHelper = new Mock<IGPRuchTowar_PW_PodsumowanieHelper>();
            pwHelper.Setup(s => s.PodsumowanieHelper).Returns(podsumowanieHelper.Object);

            rolkaHelper = new Mock<IGPRuchTowar_RolkaHelper>();
            pwHelper.Setup(s => s.RolkaHelper).Returns(rolkaHelper.Object);

            konfekcjaHelper = new Mock<IGPRuchTowar_PW_KonfekcjaHelper>();
            pwHelper.Setup(s => s.KonfekcjaHelper).Returns(konfekcjaHelper.Object);

            rolkaBazowaHelper = new Mock<IGPRuchTowar_PW_RolkaBazowaHelper>();
            pwHelper.Setup(s => s.RolkaBazowaHelper).Returns(rolkaBazowaHelper.Object);
            #endregion

            #region Strategy
            podsumowanieStrategy = new Mock<IGPRuchTowar_PW_PodsumowanieStrategyFactory>();
            podsumowanie = new Mock<IGPRuchTowar_PW_PodsumowanieHelper>();

            podsumowanieStrategy.Setup(s => s.PobierzPodsumowanieStrategy(It.IsAny<GniazdaProdukcyjneEnum>())).Returns(podsumowanie.Object);
            #endregion

            tblTowar = new Mock<ITblTowarRepository>();
            UnitOfWork.Setup(s => s.tblTowar.PobierzTowarZParametrowAsync(It.IsAny<tblTowarGeowlokninaParametryGramatura>(), It.IsAny<tblTowarGeowlokninaParametrySurowiec>(), It.IsAny<bool>()));

            tblTowarGeowlokninaParametryGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblTowarGeowlokninaParametryGramatura.Object);

            tblTowarGeowlokninaParametrySurowiec = new Mock<ITblTowarGeowlokninaParametrySurowiecRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametrySurowiec).Returns(tblTowarGeowlokninaParametrySurowiec.Object);

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            tblProdukcjaZlcecenie = new Mock<ITblProdukcjaZlecenieRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenie).Returns(tblProdukcjaZlcecenie.Object);

            tblProdukcjaZlecenieCiecia = new Mock<ITblProdukcjaZlecenieCieciaRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieCiecia).Returns(tblProdukcjaZlecenieCiecia.Object);

            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchNaglowek).Returns(tblProdukcjaRuchNaglowek.Object);

            RolkaPWDecorator = new Mock<RolkaPWDecorator>();
            PWChildObjectHelper = new Mock<PWChildObjectHelper>();
            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new GPRuchTowarPWViewModel(ViewModelService.Object, pwHelper.Object, podsumowanieStrategy.Object);
        }

        private void DodajElementyTestoweDoList()
        {
            sut.ListaGramatur = new List<tblTowarGeowlokninaParametryGramatura>
            {
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=1},
                new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=2}
            };

            sut.ListaSurowcow = new List<tblTowarGeowlokninaParametrySurowiec>
            {
                new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=1},
                new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=2},
                new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=3}
            };
            tblTowarGeowlokninaParametryGramatura.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametryGramatura>
                {
                    new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=1},
                    new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=2}
                });
            tblTowarGeowlokninaParametrySurowiec.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametrySurowiec>
                {
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=1},
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=2},
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=3}
                });

        }

        #region Messengers

        #region Rejestracja
        [Test]
        public void CTOR_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaGniazdoProdukcyjne>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<DodajEdytujGPRuchTowarMessage>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<RwProdukcjaRuchTowaruMessage>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<RozchodCalkowityRolkiRWMessage>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblPracownikGAT>>(), It.IsAny<bool>()));
        }

        #endregion


        #region GdyPrzeslano_DodajEdytujGPRuchTowarMessage
        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_GdyMessageJestNull_Lub_JakikolwiekPropertiesJestNullem()
        {
            MessengerSend((DodajEdytujGPRuchTowarMessage)null);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }

        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_GdyMessageJestOK_Parametry_Edytuj()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                RuchTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 },
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj
            });

            Assert.IsEmpty(sut.ListOfVMEntities);
        }


        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_DlugoscSzerokoscIloscM2PozostajaNiezmienione()
        {
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(true);

            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZeZleceniem(It.IsAny<tblProdukcjaZlecenieTowar>(),
                                                                     It.IsAny<tblProdukcjaRuchTowar>(),
                                                                     It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(true);

            MessengerSend(new tblProdukcjaZlecenieTowar { Szerokosc_m = 1, Dlugosc_m = 50 }
                , () => { }
                , () => MessengerSend(new DodajEdytujGPRuchTowarMessage
                {
                    RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                    RuchTowar = new db.tblProdukcjaRuchTowar
                    {
                        IDProdukcjaRuchTowar = 1,
                        IDGramatura = 1,
                        IDTowarGeowlokninaParametrySurowiec = 1,
                        CzyKalandrowana = true,
                        NrRolkiPelny = "test",
                        KodKreskowy = "1234",
                        Szerokosc_m = 2,
                        Dlugosc_m = 100
                    }
                })
                );


            DodajElementyTestoweDoList();

            Assert.AreEqual(2, sut.ListOfVMEntities.First().Szerokosc_m);
            Assert.AreEqual(100, sut.ListOfVMEntities.First().Dlugosc_m);
        }


        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_SprawdzCzyTowarMoznaDodacDoListyPW()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                RuchTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, IDGramatura = 1, IDTowarGeowlokninaParametrySurowiec = 1, Ilosc_m2 = 1 }
            });


            konfekcjaHelper.Verify(v => v.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()));
        }

        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_GdySumaM2DodawanychRolekPW_JestWiekszaNizM2RW_NieDodawajDoListy()
        {
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(false);

            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                RuchTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, IDGramatura = 1, IDTowarGeowlokninaParametrySurowiec = 1, Ilosc_m2 = 1 }
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar
                {
                    Ilosc_m2=1,
                }
            });

            Assert.AreEqual(1, sut.ListOfVMEntities.Count());
            Messenger.Verify(v => v.Send(It.IsAny<GPSaveMessage>()), Times.Never);
        }

        [Test]
        public void GdyPrzeslano_DodajEdytujGPRuchTowarMessage_GdySumaM2DodawanychRolekPW_JestMniejszLubRownaM2naRW_DodajDoListy()
        {
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(true);
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZeZleceniem(It.IsAny<tblProdukcjaZlecenieTowar>(),
                                                         It.IsAny<tblProdukcjaRuchTowar>(),
                                                         It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(true);

            WysylijWiadomoscDoDodaniaDoListy();

            Assert.AreEqual(2, sut.ListOfVMEntities.Count());
        }

        private void WysylijWiadomoscDoDodaniaDoListy()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                RuchTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, IDGramatura = 1, IDTowarGeowlokninaParametrySurowiec = 1, Ilosc_m2 = 1 }
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar
                {
                    Ilosc_m2=1,
                }
            });
        }

        [Test]
        public void GdyDodanoDoListy_Wysyla_GPSaveMessage()
        {
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<tblProdukcjaRuchTowar>(),
                                                                  It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                           .Returns(true);
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZeZleceniem(It.IsAny<tblProdukcjaZlecenieTowar>(),
                                                                     It.IsAny<tblProdukcjaRuchTowar>(),
                                                                     It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                            .Returns(true);

            WysylijWiadomoscDoDodaniaDoListy();

            Messenger.Verify(v => v.Send(It.IsAny<GPSaveMessage>()));
        }

        [Test]
        public void GdyPrzeslano_ParametryOK_DodajeDoListy_OrderByDescPoLP()
        {
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZRolkaRW(It.IsAny<tblProdukcjaRuchTowar>(),
                                                      It.IsAny<tblProdukcjaRuchTowar>(),
                                                      It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
               .Returns(true);
            konfekcjaHelper.Setup(s => s.CzyIloscM2ZgodnaZeZleceniem(It.IsAny<tblProdukcjaZlecenieTowar>(),
                                                                     It.IsAny<tblProdukcjaRuchTowar>(),
                                                                     It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                            .Returns(true);

            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW },
                RuchTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, IDGramatura = 1, IDTowarGeowlokninaParametrySurowiec = 1, Ilosc_m2 = 1 }
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                            new tblProdukcjaRuchTowar{Ilosc_m2=1, LP=1},
                            new tblProdukcjaRuchTowar{Ilosc_m2=1, LP=2},
                            new tblProdukcjaRuchTowar{Ilosc_m2=1, LP=3},
            });

            //Sprawdza czy nowododana encja jest na pierwszym miejscu listy
            Assert.AreEqual(4, sut.ListOfVMEntities.First().LP);
        }

        #endregion

        #region GdyPrzeslanoZlecenieTowar
        [Test]
        public void GdyPrzeslanoZlecenieTowar_GdyObjNieJestNull_PrzpiszDaneWyjsciowe()
        {
            //DodajElementyTestoweDoList();

            //MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });
            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieCiecia = 1,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { NrZleceniaCiecia = 1 }
            },
            () =>
            {
                tblTowarGeowlokninaParametryGramatura.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametryGramatura>
                {
                    new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=1},
                    new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=2}
                });

                tblTowarGeowlokninaParametrySurowiec.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<tblTowarGeowlokninaParametrySurowiec>
                {
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=1},
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=2},
                    new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=3}
                });

                rolkaBazowaHelper.Setup(s => s.PobierzDaneZeZlecenia(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaZlecenieTowar>()))
                                 .Returns(new db.tblProdukcjaRuchTowar());
                rolkaBazowaHelper.Setup(s => s.PobierzDaneZRolkiRw(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaGniazdoProdukcyjne>()))
                                 .Returns(new db.tblProdukcjaRuchTowar());
                rolkaBazowaHelper.Setup(s => s.PobierzNoweNryDlaRolki(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaGniazdoProdukcyjne>(), It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                                 .ReturnsAsync(new db.tblProdukcjaRuchTowar());
                sut.LoadAsync(null);
            }
            );


            Assert.AreEqual(1, sut.WybranaGramatura.IDTowarGeowlokninaParametryGramatura);
            Assert.AreEqual(1, sut.WybranySurowiec.IDTowarGeowlokninaParametrySurowiec);
        }


        [Test]
        public void GdyPrzeslanoZlecenieTowar_GdyZlecenieCieciaJestNull_NicNieRob()
        {
            DodajElementyTestoweDoList();

            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });

            MessengerSend((tblProdukcjaZlecenieCiecia)null);

            Assert.IsNull(sut.WybranaGramatura);
        }


        [Test]
        public void GdyPrzeslanoZlecenieTowar_GdyNiePrzeslanoZlecenia_NieGenerujRuchuTowaruPW()
        {
            DodajElementyTestoweDoList();

            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });
            MessengerSend((tblProdukcjaZlecenieTowar)null);

            Assert.IsTrue(sut.BazowaRolkaPW.Szerokosc_m == 0);
            Assert.IsTrue(sut.BazowaRolkaPW.Dlugosc_m == 0);
            Assert.IsTrue(sut.BazowaRolkaPW.NrZlecenia == 0);
        }

        #endregion

        #region GdyPrzeslanoTowarZRW
        [Test]
        public void GdyPrzeslanoTowarZRW_RwTowarPusty_NieDodawajDanychDoModeluPW()
        {
            MessengerSend(new RwProdukcjaRuchTowaruMessage
            {
                RwTowar = null
            });

            //Assert.Throws(typeof(ArgumentNullException), GdyPrzeslanoTowarZRW);
            Assert.IsNull(sut.BazowaRolkaPW.NrRolkiBazowej);
            Assert.IsFalse(sut.BazowaRolkaPW.CzyKalandrowana);
        }
        [Test]
        public void GdyPrzeslanoTowarZRW_GdyJestOk_DodajDaneDoModeluPWPoprzezUruchomienieHelpera()
        {
            MessengerSend(new RwProdukcjaRuchTowaruMessage
            {
                RwTowar = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            },
            () =>
            {
                MessengerSend(new tblProdukcjaZlecenieTowar());
            }
            );

            rolkaBazowaHelper.Verify(x => x.PobierzDaneZeZlecenia(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<tblProdukcjaZlecenieTowar>()));
        }

        #endregion

        #region GdyPrzeslanoGniazdo
        [Test]
        public void GdyPrzeslanoGniazdo_LiniaKonfekcji_PokazIloscSztNaFormularzu()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });

            Assert.IsTrue(sut.IloscSztWidoczne);
        }
        [Test]
        public void GdyPrzeslanoGniazdo_LiniaWlokninLubLiniaKalandra_NiePokazujIlosciSztNaFormularzu()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin });

            Assert.IsFalse(sut.IloscSztWidoczne);

            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania });

            Assert.IsFalse(sut.IloscSztWidoczne);
        }
        #endregion

        #region GdyPrzeslanoRozchodCalkowityRolki

        [Test]
        public void GdyPrzeslanoRozchodCalkowityRolki_AddCommandCanExecute_False()
        {
            AddCommandCanExecute_True();
            MessengerSend(new RozchodCalkowityRolkiRWMessage
            {
                RolkaRW = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 2, Waga_kg = 5 }
            }
            , () =>
                {
                    sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
                    {
                    new tblProdukcjaRuchTowar{IDRolkaBazowa=2, Waga_kg=1},
                    new tblProdukcjaRuchTowar{IDRolkaBazowa=2, Waga_kg=1},
                    new tblProdukcjaRuchTowar{IDRolkaBazowa=2, Waga_kg=1},
                    };

                    sut.GniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji };
                }
            );

            var result = sut.AddCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        #endregion

        #endregion

        #region Properties
        #endregion

        #region AddCommand

        #region CanExecute
        [Test]
        [TestCase(GniazdaProdukcyjneEnum.LiniaWloknin, true)]
        [TestCase(GniazdaProdukcyjneEnum.LiniaDoKalandowania, false)]
        [TestCase(GniazdaProdukcyjneEnum.LiniaDoKonfekcji, false)]
        public void AddCommandCanExecute_GdyBrakPrzelsanegoRWDlaLiniKonfekcjiLubKalandra_False(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum, bool expected)
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)gniazdaProdukcyjneEnum },()=>MessengerSend(new tblPracownikGAT()));

            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsTrue(actual == expected);
        }
        [Test]
        public void AddCommandCanExecute_GdyGniazdoProdukcyjneJestNull_True()
        {
            MessengerSend(new RwProdukcjaRuchTowaruMessage { RwTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 } }, () => MessengerSend(new tblPracownikGAT()));

            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        [Test]
        public void AddCommandCanExecute_GdyGniazdoProdukcyjneNieJestNullIRwNieJestNull_False()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });

            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void AddCommandCanExecute_GdyPrzelsanoRW_True()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });

            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                RuchStatus = new tblRuchStatus
                {
                    IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW
                },
                RuchTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 }
            }, () => MessengerSend(new tblPracownikGAT()));
            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        private void AddCommandCanExecute_True()
        {
            sut.GniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin };
            MessengerSend(new tblPracownikGAT());
        }
        #endregion

        #region Execute
        [Test]
        public void DodajCommandExecute_OtwieraOknoDodaj()
        {
            AddCommandCanExecute_True();

            sut.AddCommand.Execute(null);

            ViewService.Verify(v => v.ShowDialog<GPRuchTowarDodajViewModel>(It.IsAny<Action>()));
        }

        [Test]
        public void DodajCommandExecute_WysylaMessage_DodajEdytujGPRuchTowarMessage()
        {
            AddCommandCanExecute_True();
            Action callback = null;
            ViewService.Setup(v => v.ShowDialog<GPRuchTowarDodajViewModel>(It.IsAny<Action>()))
                                    .Callback((Action action) => callback=action) ;

            sut.AddCommand.Execute(null);
            callback.Invoke();

            Messenger.Verify(v => v.Send<DodajEdytujGPRuchTowarMessage, GPRuchTowarDodajViewModel>(It.IsAny<DodajEdytujGPRuchTowarMessage>()));
        }

        [Test]
        public void DodajCommandExecute_GdyWysylaMessage_DodajEdytujGPRuchTowarMessage_Dla_GPRuchTowarDodajViewModel_BiezacyVM_NieOdbierMessegaKtoregoSamWysyla()
        {
            sut.AddCommand.Execute(null);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }

        #endregion
        #endregion

        #region EditCommand
        #region CanExecute
        [Test]
        public void EditCommandCanExecute_GdyObiektNull_False()
        {
            sut.SelectedVMEntity = null;

            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void EditCommandCanExecute_GdyObiektNieNull_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };

            var actual = sut.EditCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void EditCommandExecute_WyswietlOkno()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };

            sut.EditCommand.Execute(null);

            ViewService.Verify(v => v.Show<GPRuchTowarDodajViewModel>());
        }
        [Test]
        public void EdytujExecuteCommand_WyslijTowar()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 };

            sut.EditCommand.Execute(null);

            Messenger.Verify(v => v.Send<DodajEdytujGPRuchTowarMessage, GPRuchTowarDodajViewModel>(It.IsAny<DodajEdytujGPRuchTowarMessage>()));
        }
        [Test]
        public void EdytujExecuteCommand_GdyPobranoZlecenieZBazy_WysylaZlecenie()
        {
            tblProdukcjaZlcecenie.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(new tblProdukcjaZlecenie { IDProdukcjaZlecenie = 1 });
            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenie = 1,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { NrZleceniaCiecia = 1 }
            });




        }
        #endregion
        #endregion


        #region UsunCommand

        #region CanExecute
        [Test]
        public void UsunCommandCanExecute_GdySelectedVMEntityJestNull_False()
        {
            sut.SelectedVMEntity = null;

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void UsunCommandCanExecute_GdySelectedVMEntityNieJestNull_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar();

            var actual = sut.DeleteCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void UsunCommandExecute_PrzedUsunieciemWyswietlDialog()
        {
            sut.SelectedVMEntity = new tblProdukcjaRuchTowar();

            sut.DeleteCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void UsunCommandExecute_GdyIdZero_UsunTylkoZListy()
        {
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0},
            };
            sut.SelectedVMEntity = sut.ListOfVMEntities.First();

            sut.DeleteCommand.Execute(null);

            Assert.IsEmpty(sut.ListOfVMEntities);
            tblProdukcjaRuchTowar.Verify(v => v.Remove(sut.SelectedVMEntity), Times.Never);
        }

        [Test]
        public void UsunCommandExecute_GdyIdWiekszeOdZera_UsunZListyIBazy()
        {
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            tblProdukcjaRuchTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaRuchTowar());
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1},
            };
            sut.SelectedVMEntity = sut.ListOfVMEntities.First();

            sut.DeleteCommand.Execute(null);

            Assert.IsEmpty(sut.ListOfVMEntities);
            tblProdukcjaRuchTowar.Verify(v => v.Remove(sut.SelectedVMEntity));
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion

        #endregion

        #region LoadAsync
        [Test]
        public void LoadAsync_PobieraZBazy()
        {
            sut.LoadAsync(1);

            tblTowarGeowlokninaParametryGramatura.Verify(v => v.GetAllAsync());
            tblTowarGeowlokninaParametrySurowiec.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadAsync_PrzypiszDaneWyjsciowe_GdyZlecenieProdJestNull_SelectedVMEntityJestNull()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji });

            sut.LoadAsync(1);

            //Assert.AreEqual(0, sut.SelectedVMEntity.IDProdukcjaRuchTowar);
            //Assert.AreEqual(default, sut.SelectedVMEntity);
            Assert.IsNull(sut.SelectedVMEntity);
        }

        [Test]
        public void LoadAsync_GdyIdNaglowekOK_PobieraZBazyZleceniaDoSledzeniaPrzezDbContext()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 });

            sut.LoadAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }

        [Test]
        public void LoadAsync_PodczasOtwieraniaFormularza_PobieraKoljenyNrPalety()
        {
            //TODO PW!!!
            
            //doZrobienia
            //przetestujRozliczenieProdukcji
            //przetestujRejestracjeGniazd!
        }
        #endregion

        #region SaveAsync


        private void WyslanoZlecenieTowar()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne(),
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura(),
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie()
            });

            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne(),
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura(),
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie()
            });

        }

        [Test]
        public void SaveAsync_GdyIdRuchNaglowekJestNull_BrakZapisuDoBD()
        {
            WyslanoZlecenieTowar();

            sut.SaveAsync(null);

            tblProdukcjaRuchTowar.Verify(v => v.AddRange(It.IsAny<ObservableCollection<tblProdukcjaRuchTowar>>()), Times.Never);
        }
        [Test]
        public void SaveAsync_GdyIdRuchNaglowekJestZero_BrakZapisuDoBD()
        {
            sut.SaveAsync(0);

            tblProdukcjaRuchTowar.Verify(v => v.AddRange(It.IsAny<ObservableCollection<tblProdukcjaRuchTowar>>()), Times.Never);
        }



        [Test]
        public void SaveAsync_GdyIDRuchTowarIstnieje_NieDodawajDoBD()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaRuchNaglowek=0}
            };

            sut.SaveAsync(1);

            tblProdukcjaRuchTowar.Verify(v => v.Add(It.IsAny<tblProdukcjaRuchTowar>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveAsync_GdyIDRuchTowarJestZero_DodajDoBD()
        {
            sut.GniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania };
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0}
            };

            sut.SaveAsync(1);

            tblProdukcjaRuchTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()), Times.Once);
            UnitOfWork.Verify(v => v.SaveAsync());
        }


        [Test]
        public void SaveAsync_KolejnoscWykonywaniaMetod()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new db.tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Szerokosc_m = 1, Dlugosc_m = 1 });
            WyslanoZlecenieTowar();
            sut.GniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
            };
            string kolejnosc = string.Empty;
            pwHelper.Setup(s => s.RolkaHelper.PobierzKosztRolki(It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GniazdaProdukcyjneEnum>()))
                    .Callback(() => kolejnosc += "1");
            tblProdukcjaRuchTowar.Setup(s => s.AddRange(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>()))
                                 .Callback(() => kolejnosc += "2");
            UnitOfWork.Setup(s => s.SaveAsync())
                      .Callback(() => kolejnosc += "3");
            pwHelper.Setup(s => s.ZaawansowanieHelper.PobierzZaawansowanie_ProdukcjaZlecenieTowar(It.IsAny<tblProdukcjaZlecenieTowar>()))
                    .Callback(() => kolejnosc += "4");
            pwHelper.Setup(s => s.ZaawansowanieHelper.PobierzZawansowanie_ProdukcjaZlecenie(It.IsAny<int>()))
                    .Callback(() => kolejnosc += "5");

            sut.SaveAsync(1);

            Assert.AreEqual("123453", kolejnosc);
        }

        [Test]
        public void SaveAsync_GdyRWTowarJestNull_NicNieRob()
        {
            MessengerSend((RwProdukcjaRuchTowaruMessage)null);

            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0}
            };

            sut.SaveAsync(1);

            foreach (var item in sut.ListOfVMEntities)
            {
                Assert.AreEqual(null, item.IDProdukcjaRuchTowarWyjsciowy);
            }
        }

        [Test]
        public void SaveAsync_GdyRWTowarNieJestNullIIdJestWiekszeOdZera_UzupelnijPozycjePW()
        {

            tblProdukcjaZlecenieTowar.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne(),
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura(),
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie()
            });


            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne(),
                tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura(),
                tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie()
            },
            () =>
                {
                    MessengerSend(new RwProdukcjaRuchTowaruMessage
                    {
                        RwTowar = new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, IDGramatura = 1, IDTowarGeowlokninaParametrySurowiec = 1 }
                    });

                    sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
                    {
                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
                    new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0}
                    };
                    sut.GniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 };
                }
            );


            sut.SaveAsync(1);


            var nowyTowar = sut.ListOfVMEntities.FirstOrDefault(s => s.IDProdukcjaRuchTowar == 0);
            Assert.AreEqual(1, nowyTowar.IDProdukcjaRuchTowarWyjsciowy);
            Assert.AreEqual(1, nowyTowar.IDProdukcjaRuchNaglowek);

            //TODO Wprowadzic w nr rolki
            //Assert.AreEqual("W1", nowyTowar.NrRolkiPelny);


        }

        [Test]
        public void SaveAsync_DlaWszysktichElementowListy_PrzypiszWszystkieNiezbedneDaneDoModelu()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0}
            };

            sut.SaveAsync(1);

            Assert.AreEqual(1, sut.ListOfVMEntities.First().IDProdukcjaRuchNaglowek);
            Assert.AreEqual((int)ProdukcjaRozliczenieStatusEnum.NieRozliczono, sut.ListOfVMEntities.First().IDProdukcjaRozliczenieStatus);
            Assert.AreEqual((int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW, sut.ListOfVMEntities.First().IDRuchStatus);
        }

        [Test]
        public void SaveAsync_UsunChildObiektyTylkoDlaEncjiNowododawanych_GdzieIdJestZero()
        {
            WyslanoZlecenieTowar();
            sut.RolkaPWDecorator = RolkaPWDecorator.Object;
            RolkaPWDecorator.Setup(s => s.UzupelnijPozycjePW(It.IsAny<int?>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GPRuchTowarPWViewModel>()));

            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar
                {
                    IDProdukcjaRuchTowar=0,
                    IDProdukcjaRuchNaglowek=0,
                    tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura(),
                    tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                    tblProdukcjaRozliczenieStatus = new tblProdukcjaRozliczenieStatus()
                },
                new tblProdukcjaRuchTowar
                {
                    IDProdukcjaRuchTowar=1,
                    IDProdukcjaRuchNaglowek=0,
                    tblProdukcjaRozliczenieStatus=new tblProdukcjaRozliczenieStatus(),
                    tblTowarGeowlokninaParametryGramatura=new tblTowarGeowlokninaParametryGramatura(),
                    tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec(),
                }
            };

            sut.SaveAsync(1);

            Assert.IsNull(sut.ListOfVMEntities[0].tblProdukcjaRuchTowarStatus);
            Assert.IsNull(sut.ListOfVMEntities[0].tblTowarGeowlokninaParametryGramatura);
            Assert.IsNotNull(sut.ListOfVMEntities[1].tblTowarGeowlokninaParametryGramatura);
            Assert.IsNotNull(sut.ListOfVMEntities[1].tblTowarGeowlokninaParametrySurowiec);

        }

        /// <summary>
        /// Sprawdza wlasciwa kolejnosc metod podczas zapisywania.
        /// Gdy kolejnosc zla nie generuje sie nazwa ani symbol towaru poniewaz zalezne tabele sa usuwane
        /// </summary>
        [Test]
        public void SaveAsync_KolejnoscMetodPodczasZapisywania()
        {
            string kolejnosc = string.Empty;
            sut.RolkaPWDecorator = RolkaPWDecorator.Object;
            sut.PWChildObjectHelper = PWChildObjectHelper.Object;
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0}
            };
            RolkaPWDecorator.Setup(s => s.UzupelnijPozycjePW(It.IsAny<int?>(), It.IsAny<tblProdukcjaRuchTowar>(), It.IsAny<GPRuchTowarPWViewModel>()))
                            .Callback(() => kolejnosc += "1");
            PWChildObjectHelper.Setup(s => s.Remove(It.IsAny<IEnumerable<tblProdukcjaRuchTowar>>(), It.IsAny<IEnumerable<string>>()))
                               .Callback(() => kolejnosc += "2");

            sut.SaveAsync(1);

            Assert.AreEqual("12", kolejnosc);
        }
        #endregion

        #region PodsumowaniePW
        [Test]
        [Ignore("Dodac w pozniejszym terminie")]
        public void DeleteCommandExecute_GenerujPodsumowanieDwaRazy_PodczasPrzypisywaniaListyIUsuwaniaZListy()
        {
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            MessengerSend(new tblProdukcjaZlecenieTowar { IDTowarGeowlokninaParametryGramatura = 1 });
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=1, Ilosc_m2=1, Waga_kg=1, IDGramatura=1},
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=2, Ilosc_m2=1, Waga_kg=1, IDGramatura=2},
                new tblProdukcjaRuchTowar{ IDProdukcjaRuchTowar=3, Ilosc_m2=1, Waga_kg=1, IDGramatura=1},
            };
            sut.SelectedVMEntity = sut.ListOfVMEntities.First();

            //sut.UsunCommand.Execute(null);

            podsumowanieHelper.Verify(v => v.PodsumowanieRolekKwalifikowanych(), Times.Exactly(2));
            podsumowanieHelper.Verify(v => v.PodsumowanieRolekNieKwalifikowanych(), Times.Exactly(2));
            podsumowanieHelper.Verify(v => v.PodsumowaniePozostalo(), Times.Exactly(2));
        }


        #endregion
    }
}
