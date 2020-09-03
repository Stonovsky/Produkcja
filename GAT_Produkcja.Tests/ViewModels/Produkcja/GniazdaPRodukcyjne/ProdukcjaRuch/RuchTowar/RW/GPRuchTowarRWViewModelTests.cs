using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Dodaj;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Messages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Messages;
using System.ComponentModel.DataAnnotations;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW.Helpers;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Messages;
using System.Data.Entity.Core.Metadata.Edm;
using GAT_Produkcja.Tests.ViewModels.Login;
using System.Data.Entity;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.RW
{
    [TestFixture]
    public class GPRuchTowarRWViewModelTests : TestBase
    {

        private Mock<IGPRuchTowar_RW_Helper> rwHelper;
        private Mock<ITblProdukcjaRuchTowarRepository> tblProdukcjaRuchTowar;
        private Mock<ITblProdukcjaRuchNaglowekRepository> tblProdukcjaRuchNaglowek;
        private GPRuchTowarRWViewModel sut;
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            rwHelper = new Mock<IGPRuchTowar_RW_Helper>();

            tblProdukcjaRuchTowar = new Mock<ITblProdukcjaRuchTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchTowar).Returns(tblProdukcjaRuchTowar.Object);

            tblProdukcjaRuchNaglowek = new Mock<ITblProdukcjaRuchNaglowekRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaRuchNaglowek).Returns(tblProdukcjaRuchNaglowek.Object);

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new GPRuchTowarRWViewModel(ViewModelService.Object, rwHelper.Object);
        }

        #region Messenger
        [Test]
        public void CTOR_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<DodajEdytujGPRuchTowarMessage>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaZlecenieTowar>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<tblProdukcjaGniazdoProdukcyjne>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ZaawansowanieRozchoduRolkiRwMessage>>(), It.IsAny<bool>()));
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<GPAddOnRWButtonMessage>>(), It.IsAny<bool>()));
        }


        #region GdyPrzeslanoWylaczRWButton
        [Test]
        public void GdyPrzeslanoWylaczRWButton_GdyListaRWNieJestPusta_WylaczButtonDodaj_False()
        {
            MessengerSend(new GPAddOnRWButtonMessage { IsEnabled = false },
                () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>() { new db.tblProdukcjaRuchTowar() }
                , () => { });

            Assert.IsFalse(sut.IsAddButtonActive);
        }
        [Test]
        public void GdyPrzeslanoWylaczRWButton_GdyListaRWNPusta_WlaczButtonDodaj_True()
        {
            MessengerSend(new GPAddOnRWButtonMessage { IsEnabled = false },
                () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>()
                , () => { });

            Assert.IsTrue(sut.IsAddButtonActive);
        }

        #endregion

        #region GdyPrzeslanoRozchodRolkiRW
        [Test]
        public void GdyPrzeslanoRozchodRolkiRW_PrzypiszDoPropertiesa()
        {
            MessengerSend(new ZaawansowanieRozchoduRolkiRwMessage { Rozchod = 0.5m });

            Assert.That(sut.RozchodRolkiRW, Is.EqualTo(0.5m));
        }
        #endregion

        #region GdyPrzeslanoDodajMessage

        [Test]
        [TestCase(2, 1, 50, 1, 0)]
        [TestCase(1, 2, 50, 1, 0)]
        [TestCase(1, 1, 30, 1, 0)]
        [TestCase(1, 1, 60, 1, 1)]
        [TestCase(1, 1, 60, 0.5, 0)]
        [TestCase(1, 1, 60, 2, 1)]
        [TestCase(1, 1, 50, 1, 1)]
        public void GdyPrzeslanoDodajEdytujMesaage_SprawdzCzyTowarZgodny_DodajLubNieDoListy(int IDGramatura, int IDSurowiec, decimal dlugosc, decimal szerokosc, int iloscElementowWLiscie)
        {

            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                RuchTowar = new tblProdukcjaRuchTowar
                {
                    IDProdukcjaRuchTowar = 1,
                    IDGramatura = IDGramatura,
                    IDTowarGeowlokninaParametrySurowiec = IDSurowiec,
                    Dlugosc_m = dlugosc,
                    Szerokosc_m = szerokosc
                }
            },
            () => MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Dlugosc_m = 50m,
                Szerokosc_m = 1m
            })
            );

            Assert.AreEqual(iloscElementowWLiscie, sut.ListOfVMEntities.Count());
        }
        [Test]
        [TestCase(1, 1, 60, 1, 1)]
        public void GdyPrzeslanoDodajEdytujMesaage_GdyDodajeRolke_ZmieniaBool_CzyRolkaRozchodowana_NaFalse(int IDGramatura, int IDSurowiec, decimal dlugosc, decimal szerokosc, int iloscElementowWLiscie)
        {

            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                RuchTowar = new tblProdukcjaRuchTowar
                {
                    IDProdukcjaRuchTowar = 1,
                    IDGramatura = IDGramatura,
                    IDTowarGeowlokninaParametrySurowiec = IDSurowiec,
                    Dlugosc_m = dlugosc,
                    Szerokosc_m = szerokosc
                }
            },
            () =>
            {
                MessengerSend(new tblProdukcjaZlecenieTowar
                {
                    IDTowarGeowlokninaParametryGramatura = 1,
                    IDTowarGeowlokninaParametrySurowiec = 1,
                    Dlugosc_m = 50m,
                    Szerokosc_m = 1m
                });
                sut.CzyRolkaRozchodowana = true;

            });
            

            Assert.IsFalse(sut.CzyRolkaRozchodowana);
        }

        #endregion

        #endregion

        #region KodKreskowy
        private void PrzeslijDaneWymagane()
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 });
        }
        [Test]
        //Metoda synchroniczna
        public void GdyPrzeslanoKodKreskowy_WyszukajRolkeWBazie()
        {
            PrzeslijDaneWymagane();

            sut.KodKreskowy = "1231231231";

            //tblProdukcjaRuchTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
            tblProdukcjaRuchTowar.Verify(v => v.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
        }

        [Test]
        public void GdyPrzeslanoKodKreskowy_BrakRolkiWBazie_PokazWiadomoscUzytkownikowi()
        {
            PrzeslijDaneWymagane();

            sut.KodKreskowy = "1231231231";

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }


        [Test]
        // Metoda synchroniczna
        public void GdyPrzeslanoKodKreskowy_RolkiWBazie_PrzypiszOstatniaDoTowarRuchRW()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2},
            });
            sut.KodKreskowy = "1231231231";

            Assert.IsNotNull(sut.ListOfVMEntities.First());
            Assert.AreEqual(2, sut.ListOfVMEntities.First().IDProdukcjaRuchTowar);
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaRWJestRolkaRozliczona_KierunekPrzychoduRolki_Magazyn()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.Rozliczono},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.Rozliczono},
            });

            sut.KodKreskowy = "1231231231";

            Assert.AreEqual("Magazyn", sut.ListOfVMEntities.First().KierunekPrzychodu);
        }
        [Test]
        public void KodKreskowyProperty_GdyRolkaRWJestRolkaNieRozliczona_KierunekPrzychoduRolki_Linia()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
            });

            sut.KodKreskowy = "1231231231";

            Assert.AreEqual("Linia", sut.ListOfVMEntities.First().KierunekPrzychodu);
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaOk_PrzypisujeJaDoSelectedVMEntity()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
            });

            sut.KodKreskowy = "1231231231";

            Assert.IsNotNull(sut.SelectedVMEntity);
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaOk_ZmieniaPropertyCzyRolkaRozchodowanaNa_False()
        {
            PrzeslijDaneWymagane();
            sut.CzyRolkaRozchodowana = true;
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2, IDProdukcjaRozliczenieStatus = (int) ProdukcjaRozliczenieStatusEnum.NieRozliczono},
            });

            sut.KodKreskowy = "1231231231";

            Assert.IsFalse(sut.CzyRolkaRozchodowana);
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaRWNieRozchodowana_WyswietlaKomunikat()
        {
            PrzeslijDaneWymagane();
            sut.IsAddButtonActive = false;

            sut.KodKreskowy = "1231231231";

            DialogService.Verify(v => v.ShowInfo_BtnOK(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaRWNieRozchodowana_NieDodajeNowejRolki()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2}
            });
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar> { new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 } };
            sut.IsAddButtonActive = false;

            sut.KodKreskowy = "1231231231";

            Assert.AreEqual(1, sut.ListOfVMEntities.Count());
            Assert.AreEqual(1, sut.ListOfVMEntities.First().IDProdukcjaRuchTowar);
        }

        [Test]
        public void KodKreskowyProperty_GdyRolkaRWRozchodowana_PobieraRolkeZBazy()
        {
            PrzeslijDaneWymagane();
            tblProdukcjaRuchTowar.Setup(s => s.Where(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>())).Returns(new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=2}
            });
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar> { new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 } };
            sut.IsAddButtonActive = true;

            sut.KodKreskowy = "1231231231";

            Assert.AreEqual(1, sut.ListOfVMEntities.Count());
            Assert.AreEqual(2, sut.ListOfVMEntities.First().IDProdukcjaRuchTowar);
        }

        [Test]
        public void KodKreskowyProperty_SprawdzenieListyZWyszukiwaniemNull()
        {
            var lista = new List<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{KodKreskowy="1", IDProdukcjaRuchTowarStatus=null},
                new tblProdukcjaRuchTowar{KodKreskowy="1",IDProdukcjaRuchTowarStatus=1},
                new tblProdukcjaRuchTowar{KodKreskowy="1",IDProdukcjaRuchTowarStatus=3},
            };

            var listaFiltrowana1 = lista.Where(t => t.KodKreskowy == "1"
                                                && (t.IDProdukcjaRuchTowarStatus == null
                                                    || t.IDProdukcjaRuchTowarStatus != (int)ProdukcjaRuchTowarStatusEnum.Rozchodowano));

            Assert.AreEqual(2, listaFiltrowana1.Count());
        }



        #endregion

        #region Properties



        [Test]
        public void GdyPrzeslanoRwZParametramiZgodnymiZeZleceniemIMozeBycDodaneDoListy_WyslijWiadomoscZIdRWDlaFormularzaPW()
        {
            MessengerSend(new DodajEdytujGPRuchTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                RuchStatus = new tblRuchStatus { IDRuchStatus = (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW },
                RuchTowar = new tblProdukcjaRuchTowar
                {
                    IDProdukcjaRuchTowar = 1,
                    IDGramatura = 1,
                    IDTowarGeowlokninaParametrySurowiec = 1,
                    Dlugosc_m = 50,
                    Szerokosc_m = 1
                }
            },
            () => MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Dlugosc_m = 50m,
                Szerokosc_m = 1m
            })
            );

            Messenger.Verify(v => v.Send(It.Is<RwProdukcjaRuchTowaruMessage>(m => m.RwTowar.IDProdukcjaRuchTowar == 1)));
        }

        [Test]
        public void RozchodujRolkeCommandExecute_DodajeStatusRozchoduDoWszystlkichElemetnowListy()
        {
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1}
            };
            sut.SelectedVMEntity = sut.ListOfVMEntities.First();

            sut.RozchodujRolkeCommand.Execute(null);

            Assert.AreEqual((int)ProdukcjaRuchTowarStatusEnum.Rozchodowano, sut.ListOfVMEntities.First().IDProdukcjaRuchTowarStatus);
        }
        #endregion

        #region Rozchodowanie rolki
        [Test]
        public void RozchodujRolkeCommandExecute_GdyKlikieto_WyswietlaUzytkownikowiDS()
        {
            sut.RozchodujRolkeCommand.Execute(null);

            DialogService.Verify(x => x.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        //[Test]
        //public void RozchodujRolkeCommandExecute_GdyKliknieto_PrzesylaWiadomoscDoPWCelemGenerowaniaEncjiZOdpadem()
        //{
        //    DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        //    rwHelper.Setup(x => x.RolkaBazowaHelper.PobierzOdpadZRolkiRwAsync(It.IsAny<int>()));
        //    sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar> { new db.tblProdukcjaRuchTowar() };
        //    sut.SelectedVMEntity = sut.ListOfVMEntities.First();

        //    sut.RozchodujRolkeCommand.Execute(null);

        //    Messenger.Verify(x => x.Send(It.IsAny<RozchodCalkowityRolkiRWMessage>()));
        //}

        [Test]
        public void RozchodujRolkeCommandExecute_GdyRozchodowano_DodajeOdpad()
        {
            PrzeslijDaneWymagane();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        }

        [Test]
        public void RozchodujRolkeCommandExecute_GdyRozchodowano_CzysciKodKreskowyProperty()
        {
            PrzeslijDaneWymagane();
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            rwHelper.Setup(x => x.RolkaBazowaHelper.PobierzOdpadZRolkiRwAsync(It.IsAny<int>()));
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar> { new db.tblProdukcjaRuchTowar() };
            sut.SelectedVMEntity = sut.ListOfVMEntities.First();
            sut.KodKreskowy = "1";

            sut.RozchodujRolkeCommand.Execute(null);

            Assert.IsNull(sut.KodKreskowy);
        }
        #endregion

        #region LoadAsync
        [Test]
        public async Task LoadAsync_GdyIdNull_TworzySieNowaListaRW()
        {
            await sut.LoadAsync(null);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }
        [Test]
        public async Task LoadAsync_GdyIdZero_TworzySieNowaListaRW()
        {
            await sut.LoadAsync(0);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }
        [Test]
        public async Task LoadAsync_GdyIdWiekszeOdZera_PobieraRWZBazy()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                              .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                              {
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now },
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now.AddDays(-1) },
                                              });

            await sut.LoadAsync(1);

            tblProdukcjaRuchTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));
        }

        [Test]
        public void LoadAsync_GdyIdWiekszeOdZera_AleBrakRolkiRWBoZostalaRozchodowana_NieDodajeDoListyRW()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()));

            sut.LoadAsync(1);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }


        [Test]
        public void LoadAsync_GdyIdWiekszeOdZera_DodajeRWDoListy()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                              .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                              {
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now },
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now.AddDays(-1) },
                                              });

            sut.LoadAsync(1);

            Assert.IsNotEmpty(sut.ListOfVMEntities);
            Assert.AreEqual(1, sut.ListOfVMEntities.Count());
        }

        [Test]
        public void LoadAsync_GdyIdWiekszeOdZera_WysylaRolkeRWdoPW()
        {
            tblProdukcjaRuchTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                              .ReturnsAsync(new List<tblProdukcjaRuchTowar>
                                              {
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now },
                                                  new tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1, DataDodania=DateTime.Now.AddDays(-1) },
                                              });

            sut.LoadAsync(1);

            Messenger.Verify(x => x.Send(It.IsAny<RwProdukcjaRuchTowaruMessage>()));
        }

        [Test]
        public void LoadAsync_PoPobraniuZBazy_IsChanged_False()
        {
            tblProdukcjaRuchTowar.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaRuchTowar, bool>>>()))
                                 .ReturnsAsync(new db.tblProdukcjaRuchTowar { IDProdukcjaRuchTowar = 1 });

            sut.LoadAsync(1);

            Assert.IsFalse(sut.IsChanged);
        }



        #endregion

        #region AddCommand

        #region CanExecute
        [Test]
        public void AddCommandCanExecute_GdyBrakGniazdaProdukcyjnego_False()
        {
            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        [Test]
        public void AddCommandCanExecute_GdyGniazdoProdukcyjneZostaloPrzeslane_ITowarZeZlecenieZostalPrzeslany_True()
        {
            AddCommandCanExecute_True();

            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        [Test]
        public void AddCommandCanExecute_GdyAddCommandCanExecute_True_GdyPrzeslanoTowarZPW_False()
        {
            AddCommandCanExecute_True(
                () => MessengerSend(new ZaawansowanieRozchoduRolkiRwMessage { Rozchod = 0.1m })
                );

            var actual = sut.AddCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }

        private void AddCommandCanExecute_True(Action messengerAction = default)
        {
            MessengerSend(new tblProdukcjaGniazdoProdukcyjne { IDProdukcjaGniazdoProdukcyjne = 1 }
            ,
            () => MessengerSend(new tblProdukcjaZlecenieTowar())
            ,
            messengerAction
            );
        }


        #endregion

        #region Execute
        [Test]
        public void AddCommandExecute_OtwieraOknoDodajTowarRW()
        {
            AddCommandCanExecute_True();

            sut.AddCommand.Execute(null);

            ViewService.Verify(v => v.Show<GPRuchTowarDodajViewModel>());
        }

        [Test]
        public void AddCommandExecute_WysylaMessage_DodajEdytujGPRuchTowarMessage()
        {
            AddCommandCanExecute_True();

            sut.AddCommand.Execute(null);

            Messenger.Verify(v => v.Send<DodajEdytujGPRuchTowarMessage, GPRuchTowarDodajViewModel>(It.IsAny<DodajEdytujGPRuchTowarMessage>()));
        }

        #endregion
        #endregion

        #region SaveAsync
        [Test]
        public void SaveAsync_GdyPrzeslanoNull_NicNieRob()
        {
            sut.SaveAsync(null);

            tblProdukcjaRuchTowar.Verify(v => v.AddRange(It.IsAny<List<tblProdukcjaRuchTowar>>()), Times.Never);
        }

        [Test]
        public void SaveAsync_GdyPrzeslanoIdZero_NicNieRob()
        {
            sut.SaveAsync(0);

            tblProdukcjaRuchTowar.Verify(v => v.AddRange(It.IsAny<List<tblProdukcjaRuchTowar>>()), Times.Never);
        }
        [Test]
        public void SaveAsync_GdyPrzeslanoIdRuchuNaglowka_WszystkieElementyListyMajaTakieId()
        {

            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchNaglowek=2},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchNaglowek=3},
            };

            sut.SaveAsync(1);

            Assert.AreEqual(1, sut.ListOfVMEntities.First().IDProdukcjaRuchNaglowek);
        }

        [Test]
        public void SaveAsync_GdyIDRuchTowarIstnieje_NieDodawajDoBD()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0},
            };

            sut.SaveAsync(1);

            tblProdukcjaRuchTowar.Verify(v => v.Add(It.IsAny<tblProdukcjaRuchTowar>()), Times.Never);
            //unitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveAsync_GdyIDRuchTowarJestZero_UsunWszystkieChildTabele()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0, tblTowarGeowlokninaParametryGramatura= new tblTowarGeowlokninaParametryGramatura() },
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0, tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec()}
            };

            sut.SaveAsync(1);

            Assert.IsNull(sut.ListOfVMEntities[0].tblTowarGeowlokninaParametryGramatura);
            Assert.IsNotNull(sut.ListOfVMEntities[1].tblTowarGeowlokninaParametrySurowiec);
        }

        [Test]
        public void SaveAsync_GdyIDRuchTowarJestZero_DodajDoBD()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar()
            {
                tblProdukcjaZlecenie = new tblProdukcjaZlecenie
                {
                    NrZlecenia = 1,
                }
            });
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
            };


            sut.SaveAsync(1);

            tblProdukcjaRuchTowar.Verify(v => v.Add(It.IsAny<tblProdukcjaRuchTowar>()), Times.Once);
            //unitOfWork.Verify(v => v.SaveAsync());
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

            Assert.AreEqual((int)ProdukcjaRozliczenieStatusEnum.NieRozliczono, sut.ListOfVMEntities.First().IDProdukcjaRozliczenieStatus);
            Assert.AreEqual((int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW, sut.ListOfVMEntities.First().IDRuchStatus);
        }
        [Test]
        [Ignore("Nie dokonczone")]
        public void SaveAsync_WszystkiePolaOznaczoneRequiredMuszaBycWypelnione()
        {
            MessengerSend(new tblProdukcjaZlecenieTowar
            {
                IDProdukcjaZlecenieTowar = 1,
                tblProdukcjaZlecenieCiecia = new tblProdukcjaZlecenieCiecia { IDProdukcjaZlecenieCiecia = 1 }
            });
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
            {
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=0, IDProdukcjaRuchNaglowek=0},
                new tblProdukcjaRuchTowar{IDProdukcjaRuchTowar=1, IDProdukcjaRuchNaglowek=0}
            };

            sut.SaveAsync(1);

            var rw = sut.ListOfVMEntities.First();
            Type type = rw.GetType();
            var propertiesRequired = type.GetProperties()
                                    .Where(prop => prop.IsDefined(typeof(RequiredAttribute), false));
            var propertiesRange = type.GetProperties()
                        .Where(prop => prop.IsDefined(typeof(System.ComponentModel.DataAnnotations.RangeAttribute), false));

            foreach (var prop in propertiesRange)
            {
                dynamic value = prop.GetValue(rw);

                Assert.IsTrue(value > 0);
            }

        }

        #endregion
    }
}
