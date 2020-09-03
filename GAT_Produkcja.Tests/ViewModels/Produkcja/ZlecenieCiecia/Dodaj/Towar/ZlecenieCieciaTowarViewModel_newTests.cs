using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.Tests.BaseClasses;
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
    public class ZlecenieCieciaTowarViewModel_newTests
        : TestBaseGeneric<ZlecenieCieciaTowarViewModel>
    {
        private Mock<ITblProdukcjaZlecenieTowarRepository> tblProdukcjaZlecenieTowar;

        public override void SetUp()
        {
            base.SetUp();

            tblProdukcjaZlecenieTowar = new Mock<ITblProdukcjaZlecenieTowarRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaZlecenieTowar).Returns(tblProdukcjaZlecenieTowar.Object);

            CreateSut();
        }
        public override void CreateSut()
        {
            sut = new ZlecenieCieciaTowarViewModel(ViewModelService.Object);
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
        public void GdyPrzeslanoTowar_GdyTowarNull_NieDodajeDoListy()
        {
            MessengerSend((ProdukcjaZlecenieDodajTowarMessage)null);

            Assert.IsEmpty(sut.ListOfVMEntities);
        }

        #region Dodaj
        [Test]
        public void GdyPrzeslanoTowar_GdyTowarIdJestZero_DodajeDoListy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar { IDProdukcjaZlecenieTowar = 0, IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji }
            });

            Assert.IsNotEmpty(sut.ListOfVMEntities);

        }
        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_Dodaj_GdyTowarIdJestWiekszeOdZera_EdytujObiektNaLiscie()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar 
                { 
                    IDProdukcjaZlecenieTowar = 1, 
                    Szerokosc_m = 2, 
                    Dlugosc_m = 2,
                    IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                }
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            }
            );

            var towar = sut.ListOfVMEntities.SingleOrDefault(s => s.IDProdukcjaZlecenieTowar == 1);
            Assert.AreEqual(2, towar.Szerokosc_m);
            Assert.AreEqual(2, towar.Dlugosc_m);
        }
        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_Dodaj_GdyTowarIdJestWiekszeOdZera_TowaruNieMaNaLiscie_NieEdytujObiektNaLiscie()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar 
                { 
                    IDProdukcjaZlecenieTowar = 3, 
                    Szerokosc_m = 2, 
                    Dlugosc_m = 2,
                    IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji
                }
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1, Szerokosc_m=1, Dlugosc_m=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            var towar = sut.ListOfVMEntities.SingleOrDefault(s => s.IDProdukcjaZlecenieTowar == 3);
            Assert.AreEqual(3, sut.ListOfVMEntities.Count());
        }
        #endregion

        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_Usun_GdyTowarIdJestZero_UsunZListyNieZBazy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Usun,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar 
                { 
                    IDProdukcjaZlecenieTowar = 0,
                    IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji

                },
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            Assert.AreEqual(2, sut.ListOfVMEntities.Count());

            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()), Times.Never);
            UnitOfWork.Verify(v => v.SaveAsync(), Times.Never);
        }
        [Test]
        public void GdyPrzeslanoZlecenieCieciaTowar_Usun_GdyTowarIdJestWiekszyOdZera_UsunZListyIZBazy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Usun,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                ZlecenieTowar = new tblProdukcjaZlecenieTowar 
                { 
                    IDProdukcjaZlecenieTowar = 1,
                    IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji

                },
            },
            () => sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieTowar=2}
            });

            Assert.AreEqual(2, sut.ListOfVMEntities.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.Remove(It.IsAny<tblProdukcjaZlecenieTowar>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion

        #endregion

        #region DodajTowarCommand
        [Test]
        public void AddCommandExecute_PokazujeOknoDodawaniaTowaru()
        {
            sut.AddCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        #endregion

        #region EdytujTowarCommand

        private void EditCommandCanExecute_True()
        {
            sut.SelectedVMEntity = new tblProdukcjaZlecenieTowar();
        }
        [Test]
        public void EdytujTowarCommand_OtwieraOknoDodajTowar()
        {
            EditCommandCanExecute_True();
            
            sut.EditCommand.Execute(null);

            ViewService.Verify(v => v.Show<ZlecenieDodajTowarViewModel>());
        }
        [Test]
        public void EdytujTowarCommand_WysylaMessagePoOtwarciuOkna()
        {
            EditCommandCanExecute_True();

            sut.EditCommand.Execute(null);

            Messenger.Verify(v => v.Send<ProdukcjaZlecenieDodajTowarMessage, ZlecenieDodajTowarViewModel>(It.Is<ProdukcjaZlecenieDodajTowarMessage>(t => t.ZlecenieTowar == sut.SelectedVMEntity
                                                                                                                                                      && t.DodajUsunEdytujEnum == DodajUsunEdytujEnum.Edytuj
                                                                                                                                                      && t.GniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaDoKonfekcji)));
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

            Assert.IsNotNull(sut.ListOfVMEntities.First().Gramatura);
            Assert.IsNotNull(sut.ListOfVMEntities.First().Surowiec);
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
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=1, IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=2, IDProdukcjaZlecenieTowar=0},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=4, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()));
        }
        [Test]
        public void SaveAsync_BrakPozycjiZIdZlecenieCieciaTowarRownymZero_NieDodawajDoBazy()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=4},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            tblProdukcjaZlecenieTowar.Verify(v => v.AddRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()), Times.Never);
        }

        [Test]
        public void SaveAsync_GdyPrzeslanoIdWiekszeOdZera_ZmianaIdZlecenieCieciaNaPrzeslaneIdDlaWszystkichElementowListy()
        {
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=4},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenie=3, IDProdukcjaZlecenieTowar=2}
            };

            sut.SaveAsync(1);

            Assert.AreEqual(4, sut.ListOfVMEntities.Where(t => t.IDProdukcjaZlecenie == 1).Count());
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
            sut.ListOfVMEntities = new ObservableCollection<tblProdukcjaZlecenieTowar>
            {
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=1, IDProdukcjaZlecenieTowar=1},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=1, IDProdukcjaZlecenieTowar=2},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=2, IDProdukcjaZlecenieTowar=3},
                new tblProdukcjaZlecenieTowar{IDProdukcjaZlecenieCiecia=2, IDProdukcjaZlecenieTowar=4},
            };

            sut.DeleteAsync(1);

            Assert.AreEqual(2, sut.ListOfVMEntities.Count());
            tblProdukcjaZlecenieTowar.Verify(v => v.RemoveRange(It.IsAny<IEnumerable<tblProdukcjaZlecenieTowar>>()));
            UnitOfWork.Verify(v => v.SaveAsync());
        }
        #endregion

    }
}
