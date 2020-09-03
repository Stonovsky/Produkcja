using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.MsAccessImport
{
    public class MsAccessImportGniazdDoSql
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private IEnumerable<tblProdukcjaZlecenie> zleceniaProdukcyjne;
        private IEnumerable<Dyspozycje> zleceniaMsAccess;
        #endregion

        #region CTOR
        public MsAccessImportGniazdDoSql(IUnitOfWork unitOfWork,
                                          IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;

            PobierzZleceniaProdukcyjne();
        }

        private void PobierzZleceniaProdukcyjne()
        {
            zleceniaProdukcyjne = unitOfWork.tblProdukcjaZlecenie.GetAll();
        }
        #endregion

        #region Linia wloknin
        public async Task ImportLiniaWloknin()
        {
            var ruchNaglowek = StworzNaglowek(GniazdaProdukcyjneEnum.LiniaWloknin);

            var liniaWlokninMsAccess = await unitOfWorkMsAccess.Produkcja.GetAllAsync();
            var liniaWlokninMsSQL = await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin);

            var pozycjeDoImportuMsAccess = liniaWlokninMsAccess.Where(e => !liniaWlokninMsSQL.Any(es => e.Id == es.IDMsAccess));
            var pozycjeDoImportuSQL = GenerujPozycjeDoImportuDoSQL(pozycjeDoImportuMsAccess);

            if (pozycjeDoImportuSQL != null && pozycjeDoImportuMsAccess.Any())
            {
                pozycjeDoImportuSQL.Where(t => t.IsValid == false).ToList().ForEach(t => t.Waga_kg = 1);
                ruchNaglowek.Towary = pozycjeDoImportuSQL.ToList();
                unitOfWork.tblProdukcjaRuchNaglowek.Add(ruchNaglowek);
                await unitOfWork.SaveAsync();
            }
        }
        private tblProdukcjaRuchNaglowek StworzNaglowek(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            return new tblProdukcjaRuchNaglowek
            {
                DataDodania = DateTime.Now.Date,
                IDMagazyn = (int)MagazynyEnum.ProdukcjaGeowlokniny_PRGW,
                IDPracownikGAT = 7,
                IDPracownikGAT1 = 7,
                IDProdukcjaGniazdoProdukcyjne = (int)gniazdaProdukcyjneEnum,
                Uwagi = "Import z MsAccess",
            };
        }
        private IEnumerable<tblProdukcjaRuchTowar> GenerujPozycjeDoImportuDoSQL(IEnumerable<dbMsAccess.Models.Produkcja> pozycjeDoImportuMsAccess)
        {
            if (!pozycjeDoImportuMsAccess.Any()) return null;

            List<tblProdukcjaRuchTowar> pozycjeSQLdoImportu = new List<tblProdukcjaRuchTowar>();
            foreach (var poz in pozycjeDoImportuMsAccess)
            {
                var pozycjaDoImportuSQL = new ProdukcjaAdapter(poz).Generuj();
                pozycjaDoImportuSQL.IDProdukcjaZlecenieProdukcyjne = PobierzIdZlecenia(poz);
                pozycjaDoImportuSQL.tblProdukcjaRuchTowarBadania = GenerujBadania(poz);
                pozycjeSQLdoImportu.Add(pozycjaDoImportuSQL);
            }
            return pozycjeSQLdoImportu;
        }

        private List<tblProdukcjaRuchTowarBadania> GenerujBadania(dbMsAccess.Models.Produkcja poz)
        {
            return new List<tblProdukcjaRuchTowarBadania>
            {
                     new tblProdukcjaRuchTowarBadania
                     {
                         Gramatura_1=poz.Gramatura1,
                         Gramatura_2=poz.Gramatura2,
                         Gramatura_3=poz.Gramatura3,
                         GramaturaSrednia=(poz.Gramatura1+poz.Gramatura2+poz.Gramatura3)/3,
                         UwagiGramatura="Importowano z MsAccess",
                     }
                };
        }


        #endregion

        #region Kalander

        public async Task ImportLiniaKalandra()
        {
            var ruchNaglowek = StworzNaglowek(GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            var encjeMsAccess = await unitOfWorkMsAccess.Kalander.GetAllAsync();
            var encjeMsSQL = await unitOfWork.tblProdukcjaRuchTowar
                                    .WhereAsync(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania);

            var pozycjeDoImportuMsAccess = encjeMsAccess.Where(e => !encjeMsSQL.Any(es => e.Id == es.IDMsAccess));
            var pozycjeDoImportuSQL = GenerujPozycjeDoImportuDoSQL(pozycjeDoImportuMsAccess);


            if (pozycjeDoImportuSQL != null && pozycjeDoImportuMsAccess.Any())
            {

                pozycjeDoImportuSQL.Where(t => t.IsValid == false).ToList().ForEach(t => t.Waga_kg = 1);
                var p = pozycjeDoImportuSQL.Where(t => t.IsValid == false);
                ruchNaglowek.Towary = pozycjeDoImportuSQL.ToList();
                unitOfWork.tblProdukcjaRuchNaglowek.Add(ruchNaglowek);
                await unitOfWork.SaveAsync();
            }
        }

        private IEnumerable<tblProdukcjaRuchTowar> GenerujPozycjeDoImportuDoSQL(IEnumerable<Kalander> pozycjeDoImportuMsAccess)
        {
            if (!pozycjeDoImportuMsAccess.Any()) return null;

            List<tblProdukcjaRuchTowar> pozycjeSQLdoImportu = new List<tblProdukcjaRuchTowar>();
            foreach (var poz in pozycjeDoImportuMsAccess)
            {
                if (poz.Waga == 0 && poz.Dlugosc == 0 || poz.Szerokosc == 0) continue;
                if (poz.ZlecenieID == 0) continue;

                var pozycjaDoImportuSQL = new KalanderAdapter(poz).Generuj();
                pozycjaDoImportuSQL.IDProdukcjaZlecenieProdukcyjne = PobierzIdZlecenia(poz);
                pozycjeSQLdoImportu.Add(pozycjaDoImportuSQL);
            }
            return pozycjeSQLdoImportu;
        }


        #endregion

        #region Konfekcja

        public async Task ImportLiniaKonfekcji()
        {
            var ruchNaglowek = StworzNaglowek(GniazdaProdukcyjneEnum.LiniaDoKonfekcji);

            var encjeMsAccess = await unitOfWorkMsAccess.Konfekcja.GetAllAsync();
            var encjeMsSQL = await unitOfWork.tblProdukcjaRuchTowar
                                    .WhereAsync(t => t.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji);
            zleceniaMsAccess = await unitOfWorkMsAccess.Dyspozycje.GetAllAsync();


            var pozycjeDoImportuMsAccess = encjeMsAccess.Where(e => !encjeMsSQL.Any(es => e.Id == es.IDMsAccess));
            var pozycjeDoImportuSQL = GenerujPozycjeDoImportuDoSQL(pozycjeDoImportuMsAccess);

            if (pozycjeDoImportuSQL != null && pozycjeDoImportuMsAccess.Any())
            {
                pozycjeDoImportuSQL.Where(t => t.IsValid == false).ToList().ForEach(t => t.Waga_kg = 1);
                var p = pozycjeDoImportuSQL.Where(t => t.IsValid == false);
                ruchNaglowek.Towary = pozycjeDoImportuSQL.ToList();
                unitOfWork.tblProdukcjaRuchNaglowek.Add(ruchNaglowek);
                await unitOfWork.SaveAsync();
            }
        }


        private IEnumerable<tblProdukcjaRuchTowar> GenerujPozycjeDoImportuDoSQL(IEnumerable<dbMsAccess.Models.Konfekcja> pozycjeDoImportuMsAccess)
        {
            if (!pozycjeDoImportuMsAccess.Any()) return null;

            List<tblProdukcjaRuchTowar> pozycjeSQLdoImportu = new List<tblProdukcjaRuchTowar>();
            var zleceniaBledne = pozycjeDoImportuMsAccess.Where(e => e.ZlecenieID > 93);
            SkorygujZleceniaBledne(zleceniaBledne);

            zleceniaBledne = pozycjeDoImportuMsAccess.Where(e => e.ZlecenieID > 93);

            foreach (var poz in pozycjeDoImportuMsAccess)
            {
                if (poz.Waga == 0 && poz.Dlugosc == 0 || poz.Szerokosc == 0) continue;
                if (poz.ZlecenieID == 0) continue;

                var pozycjaDoImportuSQL = new KonfekcjaAdapter(poz).Generuj();
                pozycjaDoImportuSQL.IDProdukcjaZlecenieProdukcyjne = PobierzIdZlecenia(poz);
                pozycjeSQLdoImportu.Add(pozycjaDoImportuSQL);
            }
            return pozycjeSQLdoImportu;
        }

        private void SkorygujZleceniaBledne(IEnumerable<dbMsAccess.Models.Konfekcja> zleceniaBledne)
        {
            foreach (var zlecenie in zleceniaBledne)
            {
                var idBledne = zlecenie.ZlecenieID;
                zlecenie.ZlecenieID = zleceniaMsAccess.SingleOrDefault(z => z.NrZlecenia == zlecenie.ZlecenieID.ToString()).Id;
            }
        }
        #endregion        
        
        #region Wspolne

        private int? PobierzIdZlecenia(IGniazdoProdukcyjne poz)
        {
            return zleceniaProdukcyjne.SingleOrDefault(s => s.IDMsAccess == poz.ZlecenieID).IDProdukcjaZlecenie;
        }

        public async Task ImportRuchuTowarowZMsAccess(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            var ruchNaglowek = StworzNaglowek(gniazdaProdukcyjneEnum);

            switch (gniazdaProdukcyjneEnum)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    break;
                default:
                    break;
            }
            var encjeMsAccess = await unitOfWorkMsAccess.Kalander.GetAllAsync();
            var encjeMsSQL = await unitOfWork.tblProdukcjaRuchTowar
                                    .WhereAsync(t => t.IDProdukcjaGniazdoProdukcyjne == (int)gniazdaProdukcyjneEnum);

            var pozycjeDoImportuMsAccess = encjeMsAccess.Where(e => !encjeMsSQL.Any(es => e.Id == es.IDMsAccess));
            var pozycjeDoImportuSQL = GenerujPozycjeDoImportuDoSQL(pozycjeDoImportuMsAccess);


            if (pozycjeDoImportuSQL != null && pozycjeDoImportuMsAccess.Any())
            {

                pozycjeDoImportuSQL.Where(t => t.IsValid == false).ToList().ForEach(t => t.Waga_kg = 1);
                var p = pozycjeDoImportuSQL.Where(t => t.IsValid == false);
                ruchNaglowek.Towary = pozycjeDoImportuSQL.ToList();
                unitOfWork.tblProdukcjaRuchNaglowek.Add(ruchNaglowek);
                await unitOfWork.SaveAsync();
            }
        }

        #endregion
    }
}