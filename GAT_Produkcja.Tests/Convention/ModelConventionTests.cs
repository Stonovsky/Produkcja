using NUnit.Framework;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GAT_Produkcja.Tests.Convention
{
    [TestFixture]
    public class ModelConventionTests
    {
        [SetUp]
        public void SetUp()
        {

        }


        [Test]
        public void WszystkieTableMuszaMiecAtrybut_SerializableAttribute()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.SingleOrDefault(a => a.FullName.Contains("db,")); //Assembly.LoadFrom("GAT_Produkcja.db");
            //var assembly = GetAssemply(assemblies);

            var classesInAssembly = assembly.GetTypes();

            var tables = classesInAssembly
                .Where(c => c.Namespace == "GAT_Produkcja.db")
                .Where(c => c.Name.Contains("tbl"));

            var tablesWithAttribute = new List<Type>();
            foreach (var table in tables)
            {

                if (table.GetCustomAttributes(typeof(SerializableAttribute), true).Count() == 1)
                    tablesWithAttribute.Add(table);
            }

            var tablesWithoutAttribute = tables.Except(tablesWithAttribute);

            Assert.IsTrue(tablesWithoutAttribute.Count() == 0);
        }

        private Assembly GetAssemply(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Regex pattern = new Regex(@"db$");
                Match match = pattern.Match(assembly.FullName);
                if (match.Success)
                    return assembly;
            }

            return null;
        }

        [Test]
        public void Test_Condition_Expectations()
        {

            var listaBezAtrybutow = ListaTabelBezAtrybutu(typeof(SerializableAttribute));
            Assert.IsTrue(listaBezAtrybutow.Count() == 0);
        }

        private IEnumerable<Type> ListaTabelBezAtrybutu<T>(T atrybut) where T : Type
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.SingleOrDefault(a => a.FullName.Contains("db,")); //Assembly.LoadFrom("GAT_Produkcja.db");
            var classesInAssembly = assembly.GetTypes();

            var tables = classesInAssembly
                .Where(c => c.Namespace == "GAT_Produkcja.db")
                .Where(c => c.Name.Contains("tbl"));

            var tablesWithAttribute = new List<Type>();
            foreach (var table in tables)
            {

                if (table.GetCustomAttributes(atrybut, true).Count() == 1)
                    tablesWithAttribute.Add(table);
            }

            return tables.Except(tablesWithAttribute);

        }

        [Test]
        [Ignore("Nie znalazlem sposobu na sprawdzenie cusotmatrybutow")]
        public void WszystkieTableMuszaMiecAtrybut_AddINotifyPropertyChangedInterfaceAttribute()
        {

            var listaBezAtrybutow = ListaTabelBezAtrybutu(typeof(AddINotifyPropertyChangedInterfaceAttribute));
            Assert.IsTrue(listaBezAtrybutow.Count() == 0);

        }

    }
}
