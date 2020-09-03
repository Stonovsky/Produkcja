using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieCiecia.Dodaj.Towar
{
    [TestFixture, Ignore("Testy starego VM")]
    
    public class ZlecenieCieciaTowarViewModelTests : TestBaseGeneric<ZlecenieCieciaTowarViewModel_old>
    {
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();


            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieCieciaTowarViewModel_old(ViewModelService.Object);
        }


        #region Messengers

        #region Rejestracja
        [Test]
        public void Messengers_Rejestracja()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ProdukcjaZlecenieDodajTowarMessage>>(), It.IsAny<bool>()));

        }
        #endregion

        #region GdyPrzeslanoZlecenieCieciaTowar
        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_GdyTowarNull_NieDodawajDoListy()
        {
            MessengerSend((ProdukcjaZlecenieDodajTowarMessage)null);

            Assert.IsEmpty(sut.ListaTowarow);
        }

        #region Dodaj
        [Test]
        [Ignore("testy do starego VM")]
        public void GdyPrzeslanoZlecenieCieciaTowar_GdyTowarIdJestZero_DodajDoListy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 0 }
            });

            Assert.IsNotEmpty(sut.ListaTowarow);

        }
        [Test]
        [Ignore("testy do starego VM")]
        public void GdyPrzeslanoZlecenieCieciaTowar_Dodaj_GdyTowarIdJestWiekszeOdZera_EdytujObiektNaLiscie()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1, Szerokosc_m = 2, Dlugosc_m = 2 }
            },
            () => sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            }
            );

            var towar = sut.ListaTowarow.SingleOrDefault(s => s.IDProdukcjaZlecenieTowar == 1);
            Assert.AreEqual(2, towar.Szerokosc_m);
            Assert.AreEqual(2, towar.Dlugosc_m);
        }
        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_Dodaj_GdyTowarIdJestWiekszeOdZera_TowaruNieMaNaLiscie_NieEdytujObiektNaLiscie()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 3, Szerokosc_m = 2, Dlugosc_m = 2 }
            },
            () => sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            var towar = sut.ListaTowarow.SingleOrDefault(s => s.IDProdukcjaZlecenieTowar == 3);
            Assert.AreEqual(3, sut.ListaTowarow.Count());
        }
        #endregion

        [Test]
        [Ignore("testy do starego VM")]
        public void GdyPrzeslanoZlecenieCieciaTowar_Usun_GdyTowarIdJestZero_UsunZListyNieZBazy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Usun,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 0 },
            },
            ()=> sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            Assert.AreEqual(2, sut.ListaTowarow.Count());

            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        [Test]
        [Ignore("testy do starego VM")]
        public void GdyPrzeslanoZlecenieCieciaTowar_Usun_GdyTowarIdJestWiekszyOdZera_UsunZListyIZBazy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Usun,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 1 },
            },
            () => sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            Assert.AreEqual(2, sut.ListaTowarow.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion

        #endregion

        #region DodajTowarCommand
        [Test]
        public void DodajCommandExecute_PokazujeOknoDodawaniaTowaru()
        {
            sut.DodajTowarCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        #endregion

        #region EdytujTowarCommand
        [Test]
        public void EdytujTowarCommand_OtwieraOknoDodajTowar()
        {
            sut.EdytujTowarCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        [Test]
        public void EdytujTowarCommand_WysylaMessagePoOtwarciuOkna()
        {
            sut.EdytujTowarCommand.Execute(null);

            Messenger.Verify(v => v.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>(It.Is<ProdukcjaZlecenieDodajTowarMessage>(t => t.ZlecenieTowar == sut.WybranyTowar
                                                                                                                                                      && t.DodajUsunEdytujEnum==DodajUsunEdytujEnum.Edytuj
                                                                                                                                                      && t.GniazdaProdukcyjneEnum ==GniazdaProdukcyjneEnum.LiniaDoKonfekcji)));
        }
        #endregion

        #region UsunTowarCommand
        [Test]
        public void UsunTowarCommandExecute_PokazDialogBox()
        {
            sut.UsunTowarCommand.Execute(null);

            DialogService.Verify(v => v.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void UsunTowarCommandExecute_DialogService_True_IdPorukcjaZlecenieCieciaTowarJestZero_UsuwaZListyNieZBazy()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };
            sut.WybranyTowar = sut.ListaTowarow.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == 0);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunTowarCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaTowarow.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void UsunTowarCommandExecute_DialogService_True_IdPorukcjaZlecenieCieciaTowarJestWiekszeOdZera_UsuwaZListyIZBazy()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2},
            };
            sut.WybranyTowar = sut.ListaTowarow.SingleOrDefault(t => t.IDProdukcjaZlecenieTowar == 1);
            DialogService.Setup(s => s.ShowQuestion_BoolResult(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            sut.UsunTowarCommand.Execute(null);

            Assert.AreEqual(2, sut.ListaTowarow.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }

        #endregion

        #region LoadAsync
        [Test]
        public void LoadAsync_GdyIdJestNull_NiczegoNiePobieraj()
        {
            sut.LoadAsync(null);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()), Times.Never);
        }
        [Test]
        public void LoadAsync_GdyIdJestZero_NiczegoNiePobieraj()
        {
            sut.LoadAsync(0);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()), Times.Never);
        }

        [Test]
        public void LoadAsync_GdyIdJestWiekszeOdZera_PobierajZBazy()
        {
            sut.LoadAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>()));
        }
        [Test]
        public void LoadAsync_GdyIdJestWiekszeOdZera_PrzypiszPolaNotMapped()
        {
            tblProdukcjaZlecenieTowar.Setup(s => s.WhereAsync(It.IsAny<Expression<Func<tblProdukcjaZlecenieTowar, bool>>>())).ReturnsAsync(new List<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, IDTowarGeowlokninaParametryGramatura=1, IDTowarGeowlokninaParametrySurowiec=1,
                    tblTowarGeowlokninaParametryGramatura = new tblTowarGeowlokninaParametryGramatura{IDTowarGeowlokninaParametryGramatura=1, Gramatura=100},
                    tblTowarGeowlokninaParametrySurowiec = new tblTowarGeowlokninaParametrySurowiec{IDTowarGeowlokninaParametrySurowiec=1, Skrot="PP"}
                },
            });

            sut.LoadAsync(1);

            Assert.IsNotNull(sut.ListaTowarow.First().Gramatura);
            Assert.IsNotNull(sut.ListaTowarow.First().Surowiec);
        }


        #endregion

        #region SaveAsync
        [Test]
        public void SaveAsync_IdZlecenieCieciaNaglowekJestNull_NieZapisuj()
        {
            sut.SaveAsync(null);

            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        [Test]
        public void SaveAsync_IdZlecenieCieciaNaglowekJestZero_NieZapisuj()
        {
            sut.SaveAsync(0);

            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        [Test]
        public void SaveAsync_IdZlecenieCieciaNaglowekJestWiekszeOdZera_ZapiszWBazie()
        {
            sut.SaveAsync(1);

            UnitOfWork.Verify(v => v.SaveAsync());
        }

        [Test]
        public void SaveAsync_PozycjaIdZlecenieCieciaTowarJestZero_DodajDoBazy()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=1, IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=2, IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=4, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()));
        }
        [Test]
        public void SaveAsync_BrakPozycjiZIdZlecenieCieciaTowarRownymZero_NieDodawajDoBazy()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=4},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Never);
        }

        [Test]
        public void SaveAsync_GdyPrzeslanoIdWiekszeOdZera_ZmianaIdZlecenieCieciaNaPrzeslaneIdDlaWszystkichElementowListy()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=4},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=3, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            Assert.AreEqual(4, sut.ListaTowarow.Where(t => t.IDProdukcjaZlecenieCiecia == 1).Count());
        }
        #endregion

        #region DeleteAsync
        [Test]
        public void DeleteAsync_GdyIdJestZero_NicNieRob()
        {
            sut.DeleteAsync(0);

            tblProdukcjaZlecenieTowar.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }

        [Test]
        public void DeleteAsync_GdyIdWiekszeOdZera_Usun()
        {
            sut.ListaTowarow = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=1, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=1, IDProdukcjaZlecenieTowar=2},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=2, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=2, IDProdukcjaZlecenieTowar=4},
            };

            sut.DeleteAsync(1);

            Assert.AreEqual(2, sut.ListaTowarow.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion
    }
}
