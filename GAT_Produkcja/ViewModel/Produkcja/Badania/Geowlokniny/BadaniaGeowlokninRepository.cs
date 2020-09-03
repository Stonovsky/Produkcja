using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny
{
    public class BadaniaGeowlokninRepository : IBadaniaGeowlokninRepository
    {
        private readonly GAT_ProdukcjaModel ctx;

        public BadaniaGeowlokninRepository(GAT_ProdukcjaModel ctx)
        {
            this.ctx = ctx;
        }

        public async Task DodajBadaniaDoBazyAsync(List<tblWynikiBadanGeowloknin> listaBadan)
        {
            ctx.tblWynikiBadanGeowloknin.AddRange(listaBadan);
            await ctx.SaveChangesAsync();
        }

        public async Task<ObservableCollection<tblWynikiBadanGeowloknin>> PobierzListeBadan()
        {
            var lista = await ctx.tblWynikiBadanGeowloknin.ToListAsync();
            return new ObservableCollection<tblWynikiBadanGeowloknin>(lista);
        }
        public async Task<tblWynikiBadanGeowloknin> PobierzWynikiBadaniaZBazyAsync(string sciezkaPliku)
        {
            return await ctx.tblWynikiBadanGeowloknin.Where(p=>p.SciezkaPliku==sciezkaPliku).SingleOrDefaultAsync();
        }

        public async Task UaktualnijWynikiBadanWBazieAsync()
        {
            await ctx.SaveChangesAsync();
        }
    }
}
