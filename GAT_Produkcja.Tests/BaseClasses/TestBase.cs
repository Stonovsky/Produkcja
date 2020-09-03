using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.BaseClasses
{
    [TestFixture]
    public abstract class TestBase
    {

        public Mock<IUnitOfWork> UnitOfWork { get; private set; }
        public Mock<IUnitOfWorkFactory> UnitOfWorkFactory { get; private set; }
        public Mock<IViewService> ViewService { get; private set; }
        public Mock<IDialogService> DialogService { get; private set; }
        public Mock<IMessenger> Messenger { get; private set; }
        public Messenger MessengerOrg { get; private set; }
        public Mock<IViewModelService> ViewModelService { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            UnitOfWork = new Mock<IUnitOfWork>();
            UnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            ViewService = new Mock<IViewService>();
            DialogService = new Mock<IDialogService>();
            Messenger = new Mock<IMessenger>();
            MessengerOrg = new Messenger();

            ViewModelService = new Mock<IViewModelService>();
            ViewModelService.Setup(s => s.UnitOfWork).Returns(UnitOfWork.Object);
            ViewModelService.Setup(s => s.UnitOfWorkFactory).Returns(UnitOfWorkFactory.Object);
            ViewModelService.Setup(s => s.ViewService).Returns(ViewService.Object);
            ViewModelService.Setup(s => s.DialogService).Returns(DialogService.Object);
            ViewModelService.Setup(s => s.Messenger).Returns(Messenger.Object);
        }

        public abstract void CreateSut();

        #region MessengerOrg
        protected virtual void CreateSutWithOrgMessenger()
        {
            ViewModelService.Setup(s => s.Messenger).Returns(MessengerOrg);
            CreateSut();
        }
        #endregion

        #region MessengerSend
        /// <summary>
        /// Metoda uproszczajaca wysylanie wiadomosci mokowanym messengerem. 
        /// </summary>
        /// <typeparam name="T">Typ generyczny argumentu fukncji delegata</typeparam>
        /// <param name="TEntity">argument funkcji ktora po otrzymaniu wiadomosci bedzie wykonywac logike</param>
        /// <param name="messenger">obiekt messengera jako Mock</param>
        /// <param name="actionSut">delegat dla funkcji tworzacej sut -> zwykle jest to CreateSut()</param>
        protected virtual void MessengerSend<T>(T TEntity,
                                        Mock<IMessenger> messenger,
                                        Action actionSut)
        {
            Action<T> callback = null;

            // konfiguracja rejestracji messengera
            messenger.Setup(s => s.Register(It.IsAny<object>(), It.IsAny<Action<T>>(), It.IsAny<bool>()))
                     .Callback((object obj, Action<T> action, bool s) => callback = action);

            actionSut(); // delegat uruchamiajacy stworzenie nowego sut

            callback(TEntity); // delegat callback podczas rejestracji otrzumuje funkcje na ktora powyzszy messenger zostal zarejestrowany
        }

        /// <summary>
        /// Metoda uproszczajaca wysylanie wiadomosci mokowanym messengerem. 
        /// </summary>
        /// <typeparam name="T">Typ generyczny argumentu fukncji delegata</typeparam>
        /// <param name="TEntity">argument metody/delegata ktora po otrzymaniu wiadomosci bedzie uruchomiona</param>
        /// <param name="actionSut">delegat dla funkcji tworzacej sut -> zwykle jest to CreateSut()</param>
        //protected virtual void MessengerSend<T>(T TEntity,
        //                                        Action actionSut)
        //{
        //    Action<T> callback = null;

        //    // konfiguracja rejestracji messengera
        //    Messenger.Setup(s => s.Register(It.IsAny<object>(), It.IsAny<Action<T>>(), It.IsAny<bool>()))
        //             .Callback((object obj, Action<T> action, bool s) => callback = action);

        //    actionSut(); // delegat uruchamiajacy stworzenie nowego sut

        //    callback(TEntity); // delegat callback podczas rejestracji otrzumuje funkcje na ktora powyzszy messenger zostal zarejestrowany
        //}


        /// <summary>
        /// Metoda uproszczajaca wysylanie wiadomosci mokowanym messengerem. 
        /// </summary>
        /// <typeparam name="T">Typ generyczny argumentu fukncji delegata</typeparam>
        /// <param name="TEntity">argument metody/delegata ktora po otrzymaniu wiadomosci bedzie uruchomiona</param>
        protected virtual void MessengerSend<T>(T TEntity, Action methodToInvokeBeforeCallback = null, Action methodToInvokeAftereCallback = null)
            where T : class
        {
            Action<T> callback = null;

            // konfiguracja rejestracji messengera
            Messenger.Setup(s => s.Register(It.IsAny<object>(), It.IsAny<Action<T>>(), It.IsAny<bool>()))
                     .Callback((object obj, Action<T> action, bool s) => callback = action);

            CreateSut();

            methodToInvokeBeforeCallback?.Invoke();

            if (TEntity != null)
            {
                if (callback is null) throw new NotImplementedException("There is no method with given argument in the sut");
                callback(TEntity); // delegat callback podczas rejestracji otrzumuje funkcje na ktora powyzszy messenger zostal zarejestrowany
            }

            methodToInvokeAftereCallback?.Invoke();

        }

        //protected virtual void MessengerSend<T,C>(T TEntity,C ClassEntity = default,  Action methodToInvokeBeforeCallback = null)
        //            where T : class
        //            where C : class
        //{
        //    Action<T> callback = null;

        //    // konfiguracja rejestracji messengera
        //    Messenger.Setup(s => s.Register(It.IsAny<ClassEntity.GetType().>(), It.IsAny<Action<T>>(), It.IsAny<bool>()))
        //             .Callback((object obj, Action<T> action, bool s) => callback = action);

        //    CreateSut();

        //    methodToInvokeBeforeCallback?.Invoke();

        //    if (TEntity != null)
        //        callback(TEntity); // delegat callback podczas rejestracji otrzumuje funkcje na ktora powyzszy messenger zostal zarejestrowany
        //}

        #endregion    
    }
}
