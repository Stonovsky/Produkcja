using GAT_Produkcja.dbMsAccess;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAcces.Tests.Repositories
{
    [TestFixture, Ignore("Testy pobieraja dane z bazy - wlaczyc kiedy trzeba!")]
    public class ProdukcjaRepositoryTests
    {
        private ProdukcjaRepository sut;

        [SetUp]
        public void SetUp()
        {
            StaticStartUp.Initiall();

            sut = new ProdukcjaRepository();
        }

        [Test]
        public async Task GetAll_ListaNiepusta()
        {
            var lista = await sut.GetAllAsync();

            Assert.IsNotEmpty(lista);
        }
        [Test]
        public async Task  CzyPobieranieDziala()
        {
            var uow = new UnitOfWorkMsAccess();
            
            var produkcja = await uow.Produkcja.GetAllAsync();
            var artykuly= await uow.Artykuly.GetAllAsync();
            var kalander = await uow.Kalander.GetAllAsync();
            var konfekcja = await uow.Konfekcja.GetAllAsync();
            var surowiec = await uow.Surowiec.GetAllAsync();
            var normy = await uow.NormyZuzycia.GetAllAsync();

            Assert.IsNotEmpty(artykuly);
            Assert.IsNotEmpty(produkcja);
            Assert.IsNotEmpty(kalander);
            Assert.IsNotEmpty(konfekcja);
            Assert.IsNotEmpty(surowiec);
            Assert.IsNotEmpty(normy);

        }
        [Test]
        public void NormyZuzycia_Pobieranie()
        {
            var uow = new UnitOfWorkMsAccess();

            //var normy = 

        }
        [Test]
        public async Task DodawanieDoBazy()
        {
            var art = new Artykuly() { NazwaArtykulu = "test", Opis = "test" };

            //await new ArtykulyRepository().InsertAsync(art);

        }

        [Test]
        public void MethodName_Condition_Expectations()
        {
            System.Data.OleDb.OleDbConnection conn = new
                    System.Data.OleDb.OleDbConnection();
            // Modify the connection string and include any
            // additional required properties for your database.
            conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                @"Data source= \\192.168.34.57\gat\PRODUKCJA\" +
                @"Database.accdb";
            try
            {
                conn.Open();
                // Insert code to process data.
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
