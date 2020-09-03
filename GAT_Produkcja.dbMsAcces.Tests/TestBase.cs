using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAcces.Tests
{
    [TestFixture]
    public abstract class TestBase<TType>
        where TType: class
    {
        protected TType sut { get; set; }

        [SetUp]
        public abstract void SetUp();

        public abstract void CreateSut();
    }
}
