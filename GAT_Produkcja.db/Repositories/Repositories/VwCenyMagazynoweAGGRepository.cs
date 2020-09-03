using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwCenyMagazynoweAGGRepository : IGenericRepository<vwCenyMagazynoweAGG>
    {
        Task<decimal> GetPriceFromPRoductName(string productName);

    }

    public class VwCenyMagazynoweAGGRepository : GenericRepository<vwCenyMagazynoweAGG, GAT_ProdukcjaModel>, IVwCenyMagazynoweAGGRepository
    {
        public VwCenyMagazynoweAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<decimal> GetPriceFromPRoductName(string productName)
        {
            Regex pattern = new Regex(@"(?<Surowiec>PES|PP)\s*(?<Gramatura>\d+)\s*(?<UV>UV)*");
            Match match = pattern.Match(productName);
            var surowiec = match.Groups["Surowiec"].Value;
            var gramatura = match.Groups["Gramatura"].Value;
            var uv = match.Groups["UV"].Value;

            var cenaModels = await Context.vwCenyMagazynoweAGG
                                        .Where(c => c.Nazwa.Contains(surowiec))
                                        .Where(c => c.Nazwa.Contains(gramatura))
                                        .ToListAsync();
            if (string.IsNullOrEmpty(uv))
                cenaModels = cenaModels.Where(c => !c.Nazwa.ToLower().Contains("uv")).ToList();
            else
                cenaModels = cenaModels.Where(c => c.Nazwa.ToLower().Contains("uv")).ToList();

            if (cenaModels.Count() > 0)
            {
                var cenaModel = cenaModels.First();

                if (cenaModel != null)
                    return cenaModel.CenaMagazynowa;
            }
            return 0;
        }
    }
}
