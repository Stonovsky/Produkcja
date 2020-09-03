using GAT_Produkcja.db;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.ExtensionMethods
{
    [TestFixture]
    public class CompareExtensionTests
    {

        [Test]
        public void CompareList_GdyListyTeSame_ZwracaTrue()
        {

            var lista1 = new List<tblTowar> { new tblTowar { IDTowar = 1 } };
            var lista2 = new List<tblTowar> { new tblTowar { IDTowar = 1 } };

            var result = lista1.CompareWithList(lista2);

            Assert.IsTrue(result);
        }

        [Test]
        public void CompareList_GdyListyMajaTaSamaIloscItemowAleRozneDane_ZwracaFalse()
        {

            var lista1 = new List<tblTowar> { new tblTowar { IDTowar = 2 } };
            var lista2 = new List<tblTowar> { new tblTowar { IDTowar = 1 } };

            var result = lista1.CompareWithList(lista2);

            Assert.IsFalse(result);
        }

        [Test]
        public void CompareList_GdyListyMajaRoznaIloscItemow_ZwracaFalse()
        {

            var lista1 = new List<tblTowar> 
            { 
                new tblTowar { IDTowar = 1 }, 
                new tblTowar { IDTowar = 1 }, 
            };
            var lista2 = new List<tblTowar> { new tblTowar { IDTowar = 1 } };

            var result = lista1.CompareWithList(lista2);

            Assert.IsFalse(result);
        }
    }

    
}
