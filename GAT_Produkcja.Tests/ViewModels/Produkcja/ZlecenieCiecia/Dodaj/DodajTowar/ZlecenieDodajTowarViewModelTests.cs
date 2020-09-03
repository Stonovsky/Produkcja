using AutoFixture;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Tests.BaseClasses;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Messages;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieDodajTowar.Messages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.ZlecenieCiecia.Dodaj.DodajTowar
{
    [TestFixture]
    public class ZlecenieDodajTowarViewModelTests : TestBase
    {

        private Fixture fixture;
        private Mock<ITblTowarGeowlokninaParametryGramaturaRepository> tblTowarGeowlokninaParametryGramatura;
        private Mock<ITblTowarGeowlokninaParametrySurowiecRepository> tblTowarGeowlokninaParametrySurowiec;
        private Mock<ITblProdukcjaGniazdoProdukcyjneRepository> tblProdukcjaGniazdoProdukcyjne;
        private ZlecenieDodajTowarViewModel sut;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fixture = new Fixture();

            tblTowarGeowlokninaParametryGramatura = new Mock<ITblTowarGeowlokninaParametryGramaturaRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametryGramatura).Returns(tblTowarGeowlokninaParametryGramatura.Object);

            tblTowarGeowlokninaParametrySurowiec = new Mock<ITblTowarGeowlokninaParametrySurowiecRepository>();
            UnitOfWork.Setup(s => s.tblTowarGeowlokninaParametrySurowiec).Returns(tblTowarGeowlokninaParametrySurowiec.Object);

            tblProdukcjaGniazdoProdukcyjne = new Mock<ITblProdukcjaGniazdoProdukcyjneRepository>();
            UnitOfWork.Setup(s => s.tblProdukcjaGniazdoProdukcyjne).Returns(tblProdukcjaGniazdoProdukcyjne.Object);

            CreateSut();
        }

        public override void CreateSut()
        {
            sut = new ZlecenieDodajTowarViewModel(ViewModelService.Object);
        }


        #region Messengers
        #region Rejestracja
        [Test]
        public void RejestracjaMessengerow()
        {
            Messenger.Verify(v => v.Register(sut, It.IsAny<Action<ProdukcjaZlecenieDodajTowarMessage>>(), It.IsAny<bool>()));
        }
        #endregion

        #region GdyPrzeslanoTowar
        [Test]
        public void GdyPrzeslanoTowar_GdyGniazdoEnumNieZero_PobierzGniazdoZBazy()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaWloknin
            });

            sut.LoadCommand.Execute(null);

            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }
        
        [Test]
        public void GdyPrzeslanoTowar_GdyGniazdoProdukcyjneEnumJestZero_NiePobierajGniazdaZBazy()
        {

            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                DodajUsunEdytujEnum = DodajUsunEdytujEnum.Dodaj,
            });

            tblProdukcjaGniazdoProdukcyjne.Verify(v => v.GetByIdAsync(It.IsAny<int>()),Times.Never);
        }
        #endregion
        #endregion

        #region LoadCommand

        [Test]
        public void LoadCommandExecute_PobieraZBazyDane()
        {
            sut.LoadCommand.Execute(null);

            tblTowarGeowlokninaParametrySurowiec.Verify(v => v.GetAllAsync());
            tblTowarGeowlokninaParametryGramatura.Verify(v => v.GetAllAsync());
        }

        [Test]
        public void LoadCommandExecute_GdyPrzeslanoGniazdo_OkreslaCzyWlokninaKalandrowanaCzyNie()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage
            {
                GniazdaProdukcyjneEnum = GniazdaProdukcyjneEnum.LiniaDoKalandowania,
            }) ;

            sut.LoadCommand.Execute(null);

            Assert.IsTrue(sut.VMEntity.CzyKalandrowana);
        }

        #endregion

        #region SaveCommand
        private void Towar_IsValid()
        {
            //sut.TowarDoPrzeslania = fixture.Build<tblProdukcjaZlecenieTowar>()
            //    .Without(w => w.tblProdukcjaZlecenieCiecia)
            //    .Without(w => w.tblProdukcjaZlcecenieProdukcyjne)
            //    .Without(w => w.tblProdukcjaGniazdoProdukcyjne)
            //    .Without(w => w.tblProdukcjaZlecenieStatus)
            //    .Without(w => w.tblTowar)
            //    .Without(w => w.tblTowarGeowlokninaParametryGramatura)
            //    .Without(w => w.tblTowarGeowlokninaParametrySurowiec)
            //    .Create();

            sut.VMEntity = new tblProdukcjaZlecenieTowar
            {
                IDTowar = 1,
                IDTowarGeowlokninaParametryGramatura = 1,
                IDTowarGeowlokninaParametrySurowiec = 1,
                Szerokosc_m = 1,
                Dlugosc_m = 1,
                Ilosc_szt = 1,
                Ilosc_m2 = 1,
            };
        }
        
        #region CanExecute
        [Test]
        public void SaveCanExecute_TowraJestZwalidowany_True()
        {
            Towar_IsValid();

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsTrue(actual);
        }

        [Test]
        public void SaveCanExecute_TowraNieJestZwalidowany_False()
        {

            var actual = sut.SaveCommand.CanExecute(null);

            Assert.IsFalse(actual);
        }
        #endregion

        #region Execute
        [Test]
        public void SaveCommandExecute_GdyStatusEdytuj_NieWysylaMessage()
        {
            MessengerSend(new ProdukcjaZlecenieDodajTowarMessage { DodajUsunEdytujEnum = DodajUsunEdytujEnum.Edytuj });
            Towar_IsValid();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<ProdukcjaZlecenieDodajTowarMessage>()),Times.Never);
        }
        [Test]
        public void SaveCommandExecute_WysylaMessage()
        {
            Towar_IsValid();

            sut.SaveCommand.Execute(null);

            Messenger.Verify(v => v.Send(It.IsAny<ProdukcjaZlecenieDodajTowarMessage>()));

        }

        [Test]
        public void SaveCommandExecute_ZamykaOkno()
        {
            Towar_IsValid();

            sut.SaveCommand.Execute(null);

            ViewService.Verify(v => v.Close(sut.GetType().Name));
        }
        #endregion

        #endregion

        #region ObliczWage
        [Test]
        public void ObliczIlosc_m2_kg_GdyGramaturaNieWybrana_ZwracaZero()
        {
            sut.SaveCommand.CanExecute(null);

            Assert.AreEqual(0, sut.VMEntity.Ilosc_kg);
        }

        [Test]
        public void ObliczWage_GdyGramaturaWybranaIIlosc_m2WiekszaOdZera_LiczyKg()
        {
            sut.WybranaGramatura = new tblTowarGeowlokninaParametryGramatura { Gramatura = 100 };
            sut.VMEntity.Ilosc_m2 = 10;

            sut.SaveCommand.CanExecute(null);

            Assert.AreEqual(1, sut.VMEntity.Ilosc_kg);
        }
        #endregion

        #region ObliczIloscSzt
        [Test]
        public void ObliczIloscSzt_GdyDaneOk_LiczyIlosc()
        {
            sut.VMEntity = new tblProdukcjaZlecenieTowar
            {
                Ilosc_m2 = 10,
                Szerokosc_m = 1,
                Dlugosc_m = 1
            };

            sut.SaveCommand.CanExecute(null);

            Assert.AreEqual(10, sut.VMEntity.Ilosc_szt);
        }

        [Test]
        public void ObliczIloscSzt_GdyBrakujeDanych_Zwraca_Zero()
        {
            sut.VMEntity = new tblProdukcjaZlecenieTowar
            {
                Ilosc_m2 = 10,
                Szerokosc_m = 0,
                Dlugosc_m = 1
            };

            sut.SaveCommand.CanExecute(null);

            Assert.AreEqual(0, sut.VMEntity.Ilosc_szt);
        }

        #endregion
    }
}
