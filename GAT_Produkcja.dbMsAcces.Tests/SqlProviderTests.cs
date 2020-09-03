using GAT_Produkcja.dbComarch.Repositories;
using Microsoft.Win32;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAcces.Tests
{
    [TestFixture]
    public class SqlProviderTests
    {

        [Test]
        public void CheckProviderNameAndVersion()
        {
            string AccessDBAsValue = string.Empty;
            RegistryKey rkACDBKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes");
            if (rkACDBKey != null)
            {
                //int lnSubKeyCount = 0;
                //lnSubKeyCount =rkACDBKey.SubKeyCount; 
                foreach (string subKeyName in rkACDBKey.GetSubKeyNames())
                {
                    if (subKeyName.Contains("Microsoft.ACE.OLEDB"))
                    {
                        Debug.WriteLine(subKeyName);
                        // do something what you want do
                    }
                }
            }
        }
    }
}
  