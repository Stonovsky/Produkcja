using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.MsAccessImport
{
    public class MsAccessImportZlecenProdukcyjnychDoSql
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private IEnumerable<Surowiec> surowceMsAccess;
        #region CTOR
        public MsAccessImportZlecenProdukcyjnychDoSql(IUnitOfWork unitOfWork,
                                                      IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
        }
        #endregion
        public async Task ImportZleceniaProdukcyjne()
        {
            var zleceniaMsAccess = await unitOfWorkMsAccess.Dyspozycje.GetAllAsync();
            var mieszankiMsAccess = await unitOfWorkMsAccess.NormyZuzycia.GetAllAsync();
            surowceMsAccess = await unitOfWorkMsAccess.Surowiec.GetAllAsync();

            List<tblProdukcjaZlecenie> zleceniaProdukcyjneDoDodania = await GenerujZleceniaProdukcyjneDoDodania(zleceniaMsAccess, mieszankiMsAccess);

            if (zleceniaProdukcyjneDoDodania.Any())
            {
                unitOfWork.tblProdukcjaZlecenie.AddRange(zleceniaProdukcyjneDoDodania);
                await unitOfWork.SaveAsync();
            }
        }

        private async Task<List<tblProdukcjaZlecenie>> GenerujZleceniaProdukcyjneDoDodania(IEnumerable<Dyspozycje> zleceniaMsAccess,
                                                                               IEnumerable<NormyZuzycia> mieszankiMsAccess)
        {
            IEnumerable<Dyspozycje> zleceniaDoImportuZMsAccess = await PobierzZleceniaDoImportuZMsAccess(zleceniaMsAccess);

            List<tblProdukcjaZlecenie> zleceniaProdukcyjneDoDodaniaDoSQL = new List<tblProdukcjaZlecenie>();
            foreach (var zlecenie in zleceniaDoImportuZMsAccess)
            {
                //dodawanie zlecenia prod
                var zlecProd = new Dyspozycje_tblProdukcjaZlecenieAdapter(zlecenie).Generuj();
                var mieszankaDlaZlecProdMsAccess = mieszankiMsAccess.Where(m => m.ZlecenieID == zlecenie.Id);

                zlecProd.MieszankaZlecenia = GenerujMieszankeDlaZlecenia(zlecenie, mieszankaDlaZlecProdMsAccess);
                zlecProd.TowaryZlecenia = GenerujTowarDlaZlecenia(zlecenie, mieszankaDlaZlecProdMsAccess);

                zleceniaProdukcyjneDoDodaniaDoSQL.Add(zlecProd);
            }

            return zleceniaProdukcyjneDoDodaniaDoSQL;
        }

        private async Task<IEnumerable<Dyspozycje>> PobierzZleceniaDoImportuZMsAccess(IEnumerable<Dyspozycje> zleceniaMsAccess)
        {
            var zleceniaWBazieSQL = await unitOfWork.tblProdukcjaZlecenie.WhereAsync(z => z.IDMsAccess != null && z.IDMsAccess > 0);

            var zleceniaDoImportuZMsAccess = zleceniaMsAccess.Where(z => !zleceniaWBazieSQL.Any(zlecsql => zlecsql.IDMsAccess == z.Id));
            return zleceniaDoImportuZMsAccess;
        }

        private ICollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka> GenerujMieszankeDlaZlecenia(Dyspozycje zlecenie,
                                                                                                   IEnumerable<NormyZuzycia> mieszankiMsAccess)
        {
            if (zlecenie is null) return null;
            if (mieszankiMsAccess is null) return null;

            var zlecMieszanka = mieszankiMsAccess.Select(e => new NormyZuzycia_tblProdukcjaZlecenie_MieszankaAdapter(e,surowceMsAccess).Generuj());
            return zlecMieszanka.ToList();
        }

        private ICollection<tblProdukcjaZlecenieTowar> GenerujTowarDlaZlecenia(Dyspozycje zlecenie, IEnumerable<NormyZuzycia> mieszankiMsAccess)
        {
            if (zlecenie is null) return null;
            if (mieszankiMsAccess is null || !mieszankiMsAccess.Any()) return null;

            var zlecenieTowar = new NormyZuzycia_tblProdukcjaZlecenieTowarAdapter(mieszankiMsAccess, zlecenie, surowceMsAccess).Generuj();
            ICollection<tblProdukcjaZlecenieTowar> towaryZeZlecenia = new List<tblProdukcjaZlecenieTowar> { zlecenieTowar };
            return towaryZeZlecenia;
        }

    }
}
