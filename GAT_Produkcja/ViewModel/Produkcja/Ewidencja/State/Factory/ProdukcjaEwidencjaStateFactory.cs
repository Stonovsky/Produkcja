using Autofac;
using GAT_Produkcja.Startup;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory
{
    public class ProdukcjaEwidencjaStateFactory : IProdukcjaEwidencjaStateFactory
    {
        private IProdukcjaEwidencjaState liniaWlokninState;
        private IProdukcjaEwidencjaState kalanderState;
        private IProdukcjaEwidencjaState konfekcjaState;

        public ProdukcjaEwidencjaStateFactory()
        {
            liniaWlokninState = IoC.Container.Resolve<ILiniaWlokninProdukcjaEwidencjaState>();
            kalanderState = IoC.Container.Resolve<IKalanderProdukcjaEwidencjaState>();
            konfekcjaState = IoC.Container.Resolve<IKonfekcjaProdukcjaEwidencjaState>();
        }
        public IProdukcjaEwidencjaState PobierzStan(int zaznaczonyTabItem)
        {
            switch (zaznaczonyTabItem)
            {
                case 0:
                    return liniaWlokninState;
                case 1:
                    return kalanderState;
                case 2:
                    return konfekcjaState;
                default:
                    return null;
            }
        }
    }
}
