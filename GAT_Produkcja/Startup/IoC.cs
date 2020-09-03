using Autofac;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbComarch.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.UI.Utilities.WebScraper;
using GAT_Produkcja.Utilities.ExcelReportGenerator;
using GAT_Produkcja.Utilities.FilesManipulations;
using GAT_Produkcja.Utilities.Logger;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.Utilities.MailSenders.SmtpClientWrapper;
using GAT_Produkcja.Utilities.ZebraPrinter;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls.ImportZPliku;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.WeryfikacjaWynikowBadan;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Animation;

namespace GAT_Produkcja.Startup
{
    public static class IoC
    {
        /// <summary>
        /// Dependecy Container
        /// </summary>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// Wire all pieces together
        /// </summary>
        public static void Setup()
        {
            var builder = new ContainerBuilder();

            #region GAT_db
            builder.RegisterType<GAT_ProdukcjaModel>();
            #endregion

            #region UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();//.InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();

            //MsAccess
            builder.RegisterType<UnitOfWorkMsAccess>().As<IUnitOfWorkMsAccess>();
            //SqlComarch
            builder.RegisterType<UnitOfWorkComarch>().As<IUnitOfWorkComarch>();
            #endregion

            #region Services
            builder.RegisterAssemblyTypes(typeof(IoC).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();

            #endregion

            #region Strategies
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                   .Where(t => t.Name.EndsWith("Strategy"))
                   .AsImplementedInterfaces();

            //builder.RegisterType<SurowiecSubiektDictionaryMsAccessStrategy>().As<ISurowiecSubiektDictionaryMsAccessStrategy>();
            //builder.RegisterType<SurowiecSubiektZNazwyMsAccessStrategy>().As<ISurowiecSubiektZNazwyMsAccessStrategy>();
            //builder.RegisterAssemblyTypes(typeof(ISurowiecSubiektStrategy).Assembly)
            //         .Where(t => typeof(ISurowiecSubiektStrategy).IsAssignableFrom(t))
            //         .AsSelf();
            //         //.AsImplementedInterfaces();

            builder.Register<Func<SurowiecSubiektFactoryEnum, ISurowiecSubiektStrategy>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return (SurowiecSubiektFactoryEnum) =>
                {
                    switch (SurowiecSubiektFactoryEnum)
                    {
                        case SurowiecSubiektFactoryEnum.ZDictionary:
                            return cc.Resolve<SurowiecSubiektDictionaryMsAccessStrategy>();
                        case SurowiecSubiektFactoryEnum.ZNazwy:
                            return cc.Resolve<SurowiecSubiektZNazwyMsAccessStrategy>();
                        default:
                            throw new ArgumentException();
                    }
                };
            });



            #endregion

            #region States
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                   .Where(t => t.Name.EndsWith("State"))
                   .AsImplementedInterfaces();
            #endregion
            builder.RegisterType<LiniaWlokninProdukcjaEwidencjaState>().As<ILiniaWlokninProdukcjaEwidencjaState>();

            #region Factories
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                     .Where(t => t.Name.EndsWith("Factory"))
                     .AsImplementedInterfaces();

            //builder.Register<Func<SurowiecSubiektFactoryEnum, ISurowiecSubiektStrategy>>(c =>
            //{
            //    return (type) =>
            //    {
            //        switch (type)
            //        {
            //            case SurowiecSubiektFactoryEnum.ZDictionary:
            //                {
            //                    return new ProcessType1Xml();
            //                }
            //            case XmlType.Type2:
            //                {
            //                    return new ProcessType2Xml();
            //                }
            //            case XmlType.Type3:
            //                {
            //                    return new ProcessType3Xml();
            //                }
            //            default:
            //                {
            //                    return new ProcessType1Xml();
            //                }
            //        }
            //    };
            //});

            #endregion

            #region Helpers
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                     .Where(t => t.Name.EndsWith("Helper"))
                     .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(Dyspozycje).Assembly)
                     .Where(t => t.Name.EndsWith("Helper"))
                     .AsImplementedInterfaces();
            #endregion

            #region Handlers
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                                     .Where(t => t.Name.EndsWith("Handler"))
                                     .AsImplementedInterfaces();
            #endregion

            #region Readers
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                         .Where(t => t.Name.EndsWith("Reader"))
                         .AsImplementedInterfaces();
            #endregion

            #region Generators
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                    .Where(t => t.Name.EndsWith("Generator"))
                    .AsImplementedInterfaces();
            #endregion

            #region MVVMLight
            builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();
            #endregion

            #region Utilities

            builder.RegisterType<FilesManipulation>().As<IFilesManipulation>();

            // MailSenders
            builder.RegisterType<SmtpClientWrapper>().AsImplementedInterfaces();
            builder.RegisterType<GmailSender>().As<IGmailSender>();

            builder.RegisterType<OutlookMailSender>().As<IOutlookMailSender>();
            builder.RegisterType<PlikiCRUD>().As<IPlikiCRUD>();

            builder.RegisterType<PobierzDaneKontrahentaZGUS>().As<IPobierzDaneKontrahentaZGUS>();
            builder.RegisterType<DaneKontrahentaZGUSWebScraper>().As<IDaneKontrahentaZGUSWebScraper>();

            builder.RegisterType<ImportBadanZRaportuZXls>().As<IImportBadanZRaportuZXls>();
            builder.RegisterType<BadaniaGeowlokninWynikiZPliku>().As<IBadaniaGeowlokninWynikiZPliku>();
            builder.RegisterType<BadaniaGeowloknin>().As<IBadaniaGeowloknin>();

            builder.RegisterType<WeryfikacjaWynikowBadan>().As<IWeryfikacjaWynikowBadan>();

            builder.RegisterType<ZebraLabelPrinter>().As<IZebraLabelPrinter>();

            builder.RegisterType<ActivityLogger>().As<IActivityLogger>();
            #endregion

            #region Repositories
            builder.RegisterType<BadaniaGeowlokninRepository>().As<IBadaniaGeowlokninRepository>();
            #endregion

            #region ViewModels

            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                    .Where(t => t.Name.EndsWith("ViewModel"))
                    .AsSelf()
                    .AsImplementedInterfaces();
            #endregion

            #region Views

            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                    .Where(t => t.Name.EndsWith("View"))
                    .AsSelf();

            #endregion

            #region Wrappers
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                   .Where(t => t.Name.EndsWith("Wrapper"))
                   .AsImplementedInterfaces();
            #endregion

            #region AdapterPattern
            //builder.RegisterAdapter<IProdukcjaRuchTowar, Konfekcja>(k=> new Konfekcja());
            #endregion

            #region Builders
            builder.RegisterAssemblyTypes(typeof(IoC).Assembly)
                           .Where(t => t.Name.EndsWith("Builder"))
                           .AsImplementedInterfaces();
            #endregion

            #region Printers
            builder.RegisterAssemblyTypes(typeof(ViewModelLocator).Assembly)
                   .Where(t => t.Name.EndsWith("Printer"))
                   .AsImplementedInterfaces();

            #endregion

            Container = builder.Build();

        }
    }
}
