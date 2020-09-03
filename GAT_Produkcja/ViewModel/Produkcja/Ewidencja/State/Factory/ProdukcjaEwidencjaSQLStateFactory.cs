using Autofac;
using GAT_Produkcja.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory
{
    public class ProdukcjaEwidencjaSQLStateFactory : IProdukcjaEwidencjaSQLStateFactory
    {
        private IProdukcjaEwidencjaSQLState liniaWlokninState;
        private IProdukcjaEwidencjaSQLState kalanderState;
        private IProdukcjaEwidencjaSQLState konfekcjaState;

        #region CTOR
        public ProdukcjaEwidencjaSQLStateFactory()
        {
            liniaWlokninState = IoC.Container.Resolve<ILiniaWlokninProdukcjaEwidencjaSQLState>();
            kalanderState = IoC.Container.Resolve<IKalanderProdukcjaEwidencjaSQLState>();
            konfekcjaState = IoC.Container.Resolve<IKonfekcjaProdukcjaEwidencjaSQLState>();

        }
        #endregion

        public IProdukcjaEwidencjaSQLState PobierzStan(int zaznaczonyTabItem)
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
