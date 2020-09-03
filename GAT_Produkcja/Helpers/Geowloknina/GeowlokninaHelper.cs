using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.Helpers.Geowloknina
{
    public class GeowlokninaHelper : IGeowlokninaHelper
    {
        private readonly IUnitOfWork unitOfWork;

        public List<tblTowarGeowlokninaParametryRodzaj> ListaRodzajow { get; set; }
        public List<tblTowarGeowlokninaParametryGramatura> ListaGramatur { get; set; }

        public GeowlokninaHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            ListaRodzajow = new List<tblTowarGeowlokninaParametryRodzaj>();
            ListaGramatur = new List<tblTowarGeowlokninaParametryGramatura>();

            Task.Run(() => PobierzListy());
        }
        public async Task PobierzListy()
        {
            ListaRodzajow = await unitOfWork.tblTowarGeowlokninaParametryRodzaj.GetAllAsync() as List<tblTowarGeowlokninaParametryRodzaj>;
            ListaGramatur = await unitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync() as List<tblTowarGeowlokninaParametryGramatura>;
            await Task.Delay(100);
        }

        public int PobierzGramatureZNazwy(string nazwa)
        {

            string[] nazwaSplited = null;
            string[] nazwaToSplit = null;

            if (nazwa.Contains("0("))
            {
                nazwaToSplit = nazwa.Split('(');
                nazwaSplited = nazwaToSplit[0].Split(' ');
            }
            else if (nazwa.ToLower().Contains("0g"))
            {
                nazwaToSplit = nazwa.ToLower().Split('g');
                nazwaSplited = nazwaToSplit[1].Split(' ');
            }
            else if (nazwa.ToUpper().Contains("0UV"))
            {
                nazwaToSplit = nazwa.ToUpper().Split('U');
                nazwaSplited = nazwaToSplit[0].Split(' ');
            }
            else
            {
                nazwaSplited = nazwa.Split(' ');
            }


            foreach (var slowo in nazwaSplited)
            {
                var isNumeric = int.TryParse(slowo, out int gramatura);
                if (isNumeric)
                    return gramatura;
            }


            return 0;
        }

        public tblTowarGeowlokninaParametryRodzaj PobierzRodzajZNazwy(string nazwa)
        {
            var fragmentNazwy = "ALTEX AT PP";
            if (nazwa.Contains(fragmentNazwy))
            {
                return ListaRodzajow.FirstOrDefault(s => s.Rodzaj.Contains(fragmentNazwy));
            }

            fragmentNazwy = "ALTEX AT PES";
            if (nazwa.Contains(fragmentNazwy))
            {
                return ListaRodzajow.FirstOrDefault(s => s.Rodzaj.Contains(fragmentNazwy));
            }

            fragmentNazwy = "ALTEX AT";
            if (nazwa.Contains(fragmentNazwy))
            {
                return ListaRodzajow.FirstOrDefault(s => s.Rodzaj.Contains(fragmentNazwy));
            }
            fragmentNazwy = "ALTEX PES";
            if (nazwa.Contains(fragmentNazwy))
            {
                return ListaRodzajow.FirstOrDefault(s => s.Rodzaj.Contains(fragmentNazwy));
            }

            return null;
        }

        public decimal ObliczWageZNazwy(string nazwa, decimal ilosc)
        {
            var gramatura = PobierzGramatureZNazwy(nazwa);

            if (gramatura == 0)
                return 0;

            var r = ((decimal)gramatura / 1000) * ilosc;
            return (decimal)gramatura / 1000 * ilosc;
        }

        public decimal PobierzSzerokoscNawojuZNazwy_m(string nazwa)
        {
            var splt_mx = nazwa.Split(new Char[] { 'm', 'x' });
            var splt_nawias = splt_mx[0].Split('(');

            var szerokosc = Convert.ToDecimal(splt_nawias[1]);

            return szerokosc;
        }

        public decimal PobierzDlugoscNawojuZNazwy_m(string nazwa)
        {
            var split_x = nazwa.Split('x');
            var split_m = split_x[1].Split('m');

            var dlugosc = Convert.ToDecimal(split_m[0]);

            return dlugosc;
        }
        public async Task<tblTowar> PobierzTowarZGramaturyIRodzajuSurowca(int gramatura, string rodzajSurowca)
        {
            var surowiec = string.Empty;

            if (rodzajSurowca.ToLower().Contains("pp"))
                surowiec = "altex at pp";
            else if (rodzajSurowca.ToLower().Contains("pes"))
                surowiec = "altex at pes";
            else
                return null;

            return await unitOfWork.tblTowar.SingleOrDefaultAsync(t => t.Nazwa.ToLower().Contains(surowiec) &&
                                                                       t.Nazwa.ToLower().Contains(gramatura.ToString()));

        }
    }
}
