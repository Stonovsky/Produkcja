using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.Repositories;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Badania;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.GniazdaProdukcyjne.LiniaWloknin.Badania
{
    //[TestFixture, Ignore("Do poprawy")]
    //public class LiniaWlokninBadaniaViewModelTests
    //{

    //    private Mock<IUnitOfWork> unitOfWork;
    //    private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
    //    private Mock<IViewService> viewService;
    //    private Mock<IDialogService> dialogService;
    //    private Mock<IMessenger> messenger;
    //    private Messenger messengerOrg;
    //    private Mock<IWeryfikacjaTolerancji> weryfikacjaTolerancji;
    //    private Mock<ITblProdukcjaGniazdoWlokninaBadaniaRepository> tblProdukcjaGniazdoWlokninaBadania;
    //    private Mock<ITblProdukcjaGniazdoWlokninaRepository> tblProdukcjaGniazdoWloknina;
    //    private Mock<ITblProdukcjaZlcecenieProdukcyjneRepository> tblProdukcjaZlcecenieProdukcyjne;
    //    private LiniaWlokninBadaniaViewModel sut;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        unitOfWork = new Mock<IUnitOfWork>();
    //        unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
    //        viewService = new Mock<IViewService>();
    //        dialogService = new Mock<IDialogService>();
    //        messenger = new Mock<IMessenger>();
    //        messengerOrg = new Messenger();
    //        weryfikacjaTolerancji = new Mock<IWeryfikacjaTolerancji>();

    //        tblProdukcjaGniazdoWlokninaBadania = new Mock<ITblProdukcjaGniazdoWlokninaBadaniaRepository>();
    //        unitOfWork.Setup(s => s.tblProdukcjaGniazdoWlokninaBadania).Returns(tblProdukcjaGniazdoWlokninaBadania.Object);

    //        tblProdukcjaGniazdoWloknina = new Mock<ITblProdukcjaGniazdoWlokninaRepository>();
    //        unitOfWork.Setup(s => s.tblProdukcjaGniazdoWloknina).Returns(tblProdukcjaGniazdoWloknina.Object);

    //        tblProdukcjaZlcecenieProdukcyjne = new Mock<ITblProdukcjaZlcecenieProdukcyjneRepository>();
    //        unitOfWork.Setup(s => s.tblProdukcjaZlcecenieProdukcyjne).Returns(tblProdukcjaZlcecenieProdukcyjne.Object);

    //        sut = CreateSut(messenger.Object);
    //    }

    //    private LiniaWlokninBadaniaViewModel CreateSut(IMessenger messenger)
    //    {
    //        return new LiniaWlokninBadaniaViewModel(unitOfWork.Object, messenger, weryfikacjaTolerancji.Object);
    //    }


    //    #region Messengery
    //    [Test]
    //    public void RejestracjaMessengerow()
    //    {
    //        messenger.Verify(v => v.Register<tblProdukcjaZlcecenieProdukcyjne>(sut, It.IsAny<Action<tblProdukcjaZlcecenieProdukcyjne>>(), It.IsAny<bool>()));
    //    }
    //    #endregion

    //    #region LoadAsync
    //    [Test]
    //    public async Task LoadAsync_WhenIdIsNull_UoWisNotCalled()
    //    {
    //        await sut.LoadAsync(null);

    //        tblProdukcjaGniazdoWlokninaBadania.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaGniazdoWlokninaBadania, bool>>>()), Times.Never);
    //    }
    //    [Test]
    //    public async Task LoadAsync_WhenIdIsZero_UoWisNotCalled()
    //    {
    //        await sut.LoadAsync(0);

    //        tblProdukcjaGniazdoWlokninaBadania.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaGniazdoWlokninaBadania, bool>>>()), Times.Never);
    //    }
    //    [Test]
    //    public async Task LoadAsync_WhenIdIsNotNull_UoWGetByIdAsyncShouldBeCalled()
    //    {
    //        await sut.LoadAsync(1);

    //        tblProdukcjaGniazdoWlokninaBadania.Verify(v => v.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaGniazdoWlokninaBadania, bool>>>()));
    //    }

    //    [Test]
    //    public void LoadAsync_AfterLoad_IsChanged_False()
    //    {

    //        tblProdukcjaGniazdoWlokninaBadania.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<tblProdukcjaGniazdoWlokninaBadania, bool>>>()))
    //                                          .ReturnsAsync(new tblProdukcjaGniazdoWlokninaBadania { IDProdukcjaGniazdoWlokninaBadania = 2, Gramatura_1 = 1 });

    //        sut.LoadAsync(1);

    //        Assert.IsFalse(sut.IsChanged);
    //    }

    //    #endregion

    //    #region SaveAsync
    //    [Test]
    //    public void SaveAsync_IDisNull_DoNothnig()
    //    {
    //        sut.SaveAsync(null);

    //        unitOfWork.Verify(v => v.SaveAsync(), Times.Never);
    //    }

    //    [Test]
    //    public void SaveAsync_IDisNotNull_AddIdToModelBadanie()
    //    {
    //        sut.SaveAsync(1);

    //        Assert.AreEqual(1, sut.Badanie.IDProdukcjaGniazdoWloknina);
    //    }

    //    [Test]
    //    public void SaveAsync_IDisNotNull_AddToDb()
    //    {
    //        sut.SaveAsync(1);

    //        tblProdukcjaGniazdoWlokninaBadania.Verify(v => v.Add(It.IsAny<tblProdukcjaGniazdoWlokninaBadania>()));
    //        unitOfWork.Verify(v => v.SaveAsync());
    //    }

    //    [Test]
    //    public void SaveAsync_IsChange_False()
    //    {
    //        sut.SaveAsync(1);

    //        Assert.IsFalse(sut.IsChanged);
    //    }
    //    #endregion

    //    #region DeleteAsync
    //    [Test]
    //    public void DeleteAsync_UoWRemoveIsCalled()
    //    {
    //        sut.DeleteAsync(1);

    //        tblProdukcjaGniazdoWlokninaBadania.Verify(v => v.Remove(It.IsAny<tblProdukcjaGniazdoWlokninaBadania>()));
    //        unitOfWork.Verify(v => v.SaveAsync());
    //    }


    //    #endregion

    //    #region CzySrenidaGramaturaWTolerancjach
    //    [Test]
    //    public void CzySredniaGramaturaWTolerancjach_GdyWszystkieGramaturyNieSaWypelnione_NicNieRob()
    //    {
    //        sut.Badanie.IDProdukcjaGniazdoWloknina = 1;
            
    //        sut.GramaturaProbkaLewa = 100;

    //        weryfikacjaTolerancji.Verify(v => v.CzyParametrZgodny(It.IsAny<int>(), GeowlokninaParametryEnum.Gramatura, It.IsAny<int>()), Times.Never);
    //    }

    //    [Test]
    //    public void CzySredniaGramaturaWTolerancjach_WszytkieGramaturyWypelnione_SprawdzTolerancje()
    //    {

    //        sut.AktywneZlecenieProdukcyjne = new tblProdukcjaZlcecenieProdukcyjne { IDProdukcjaZlecenieProdukcyjne = 1, IDTowar=1 };

    //        sut.Badanie = new tblProdukcjaGniazdoWlokninaBadania
    //        {
    //            IDProdukcjaGniazdoWloknina = 1,
    //            Gramatura_1 = 100,
    //            Gramatura_2 = 100,
    //            Gramatura_3 = 100

    //        };

    //        sut.GramaturaProbkaLewa = 110;

    //        weryfikacjaTolerancji.Verify(v => v.CzyParametrZgodny(It.IsAny<int>(), It.IsAny<GeowlokninaParametryEnum>(), It.IsAny<int>()));
    //    }

    //    [Test]
    //    public void CzySredniaGramaturaWTolerancjach_GdyAktywneZlecenieNieNull_SprawdzTolerancje()
    //    {

    //        sut.Badanie = new tblProdukcjaGniazdoWlokninaBadania
    //        {
    //            IDProdukcjaGniazdoWloknina = 1,
    //            Gramatura_1 = 100,
    //            Gramatura_2 = 100,
    //            Gramatura_3 = 100

    //        };

    //        sut.GramaturaProbkaLewa = 110;

    //        weryfikacjaTolerancji.Verify(v => v.CzyParametrZgodny(It.IsAny<int>(), GeowlokninaParametryEnum.Gramatura, It.IsAny<int>()), Times.Never);
    //    }


    //    [Test]
    //    public void GramaturaSredniaProperty_WhenGramaturaIsNull_DoNotCalculateGramaturaSrednia()
    //    {
    //        sut.GramaturaProbkaLewa = 100;

    //        var actual = sut.GramaturaSrednia;

    //        Assert.AreEqual(0, actual);
    //    }

    //    [Test]
    //    public void GramaturaSredniaProperty_WhenGramaturyAreNotNull_CalculateGramaturaSrednia()
    //    {
    //        sut.GramaturaProbkaLewa = 100;
    //        sut.GramaturaProbkaSrodek = 100;
    //        sut.GramaturaProbkaPrawa = 100;

    //        var actual = sut.GramaturaSrednia;

    //        Assert.AreEqual(100, actual);
    //    }
    //    #endregion

    //    #region PrzeslanoAktywneZlecenieProdukcyjne
    //    [Test]
    //    public void GdyPrzeslanoAktywneZlecenieProdukcyjne_WhenSent_ZlecenieProdukcyjneShouldNotBeNull()
    //    {
    //        sut = CreateSut(messengerOrg);

    //        messengerOrg.Send(new tblProdukcjaZlcecenieProdukcyjne { IDProdukcjaZlecenieProdukcyjne = 1 });

    //        Assert.IsNotNull(sut.AktywneZlecenieProdukcyjne);
    //    }
    //    [Test]
    //    public void GdyPrzeslanoAktywneZlecenieProdukcyjne_WhenZlecenieIsNull_ZlecenieProdukcyjneIsNull()
    //    {
    //        sut = CreateSut(messengerOrg);

    //        messengerOrg.Send((tblProdukcjaZlcecenieProdukcyjne)null);

    //        Assert.IsNull(sut.AktywneZlecenieProdukcyjne);
    //    }

    //    #endregion


    //}

}
