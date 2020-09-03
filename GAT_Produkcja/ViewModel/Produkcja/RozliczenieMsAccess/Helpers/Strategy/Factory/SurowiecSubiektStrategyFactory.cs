using Autofac;
using GAT_Produkcja.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory
{
    public class SurowiecSubiektStrategyFactory : ISurowiecSubiektStrategyFactory
    {
        public ISurowiecSubiektStrategy PobierzStrategie(SurowiecSubiektFactoryEnum enumSubiektFactory)
        {
            switch (enumSubiektFactory)
            {
                case SurowiecSubiektFactoryEnum.ZNazwy:
                    return IoC.Container.Resolve<ISurowiecSubiektZNazwyMsAccessStrategy>();
                case SurowiecSubiektFactoryEnum.ZDictionary:
                    return IoC.Container.Resolve<ISurowiecSubiektDictionaryMsAccessStrategy>();
                default:
                    return null;
            }
        }
    }
}
