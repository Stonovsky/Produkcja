using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GAT_Produkcja.ViewModel.MainMenu;
using NUnit.Framework;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Convention
{
    [TestFixture]
    public class ViewModelConventionTests
    {
        [Test]
        public void EachViewModel_Implements_AddINotifyPropertyChangedInterfaceAttribuite()
        {
            var types = typeof(MainMenuViewModel).Assembly.GetTypes();
            var viewModelClasses = types.Where(t=>t.BaseType!=null)
                                        .Where(t => t.BaseType.Name.Contains("ViewModel"))
                                        .Where(t => t.IsClass)
                                        .ToList();

            Assert.IsNotEmpty(viewModelClasses);

            var listWithoutImplementedIntefaces = viewModelClasses.Where(c => c.GetInterfaces().Length == 0);

            Assert.IsEmpty(listWithoutImplementedIntefaces);
        }

    }
}
