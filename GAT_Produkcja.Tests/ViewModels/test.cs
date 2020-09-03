using GAT_Produkcja.db;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels
{
    [TestFixture]
    public class test
    {
        private tblTowar sut;

        public void SetUp()
        {

            sut = new tblTowar();
        }

        [Test]
        public void MethodName_Condition_Expectations()
        {
            sut = new tblTowar();
            var t = sut.GetType();

            var type = sut.GetType();
            var count = type.GetCustomAttributes(typeof(SerializableAttribute), true).Count();
            Assert.AreEqual(1, count);
        }
    }
}
