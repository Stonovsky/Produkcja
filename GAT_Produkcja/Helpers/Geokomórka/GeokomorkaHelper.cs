using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;

namespace GAT_Produkcja.Helpers.Geokomórka
{
    public class GeokomorkaHelper : IGeokomorkaHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private List<tblTowarGeokomorkaParametryGeometryczne> listaParametrowGeometrycznych;
        private List<tblTowarGeokomorkaParametryTyp> listaTypow;
        private List<tblTowarGeokomorkaParametryRodzaj> listaRodzajow;
        private List<tblTowarGeokomorkaParametryZgrzew> listaZgrzewow;

        public List<tblTowarGeokomorkaParametryTyp> ListaTypow { get => listaTypow; set => listaTypow = value; }
        public List<tblTowarGeokomorkaParametryZgrzew> ListaZgrzewow { get => listaZgrzewow; set => listaZgrzewow = value; }
        public List<tblTowarGeokomorkaParametryRodzaj> ListaRodzajow { get => listaRodzajow; set => listaRodzajow = value; }
        public List<tblTowarGeokomorkaParametryGeometryczne> ListaParametrowGeometrycznych { get => listaParametrowGeometrycznych; set => listaParametrowGeometrycznych = value; }

        #region CTOR
        public GeokomorkaHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            listaTypow = new List<tblTowarGeokomorkaParametryTyp>();
            listaParametrowGeometrycznych = new List<tblTowarGeokomorkaParametryGeometryczne>();
            listaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>();
            listaTypow = new List<tblTowarGeokomorkaParametryTyp>();
            listaZgrzewow = new List<tblTowarGeokomorkaParametryZgrzew>();

            Task.Run(() => PobierzListy());
        }
        #endregion

        public async Task PobierzListy()
        {
            listaParametrowGeometrycznych = await unitOfWork.tblTowarGeokomorkaParametryGeometryczne.GetAllAsync().ConfigureAwait(false) as List<tblTowarGeokomorkaParametryGeometryczne>;
            listaTypow = await unitOfWork.tblTowarGeokomorkaParametryTyp.GetAllAsync().ConfigureAwait(false) as List<tblTowarGeokomorkaParametryTyp>;
            listaRodzajow = await unitOfWork.tblTowarGeokomorkaParametryRodzaj.GetAllAsync().ConfigureAwait(false) as List<tblTowarGeokomorkaParametryRodzaj>;
            listaZgrzewow = await unitOfWork.tblTowarGeokomorkaParametryZgrzew.GetAllAsync().ConfigureAwait(false) as List<tblTowarGeokomorkaParametryZgrzew>;
        }

        public decimal ObliczIloscM2ZgodnaZPowierzchniaSekcji(string kodZgrzewu, decimal szerokoscSekcji_m, decimal dlugoscSekcji_m, decimal ilosc_m2)
        {
            if (ilosc_m2 == 0 ||
                String.IsNullOrEmpty(kodZgrzewu) ||
                szerokoscSekcji_m == 0 ||
                dlugoscSekcji_m == 0)
                return 0;

            var iloscPodana = ilosc_m2;
            var powierzchniaSekcji = dlugoscSekcji_m * szerokoscSekcji_m;
            var iloscWlasciwa = Math.Ceiling(iloscPodana / powierzchniaSekcji) * powierzchniaSekcji;

            return iloscWlasciwa;
        }
        public decimal ObliczIloscM2ZgodnaZPowierzchniaSekcji(string nazwa, decimal ilosc_m2)
        {
            var zgrzew = PobierzZgrzewZNazwy(nazwa);
            var dlugoscSekcji_m = PobierzStandardowaDlugoscSekcjiZNazwy_m(nazwa);
            var szerokoscSekcji_m = PobierzStandardowaSzerokoscSekcjiZNazwy_m(nazwa);


            if (ilosc_m2 == 0 ||
                String.IsNullOrEmpty(zgrzew.KodZgrzewu) ||
                szerokoscSekcji_m == 0 ||
                dlugoscSekcji_m == 0)
                return 0;

            var iloscPodana = ilosc_m2;
            var powierzchniaSekcji = dlugoscSekcji_m * szerokoscSekcji_m;
            var iloscWlasciwa = Math.Ceiling(iloscPodana / powierzchniaSekcji) * powierzchniaSekcji;

            return iloscWlasciwa;
        }

        public int ObliczIloscSekcji(string kodZgrzewu, decimal szerokoscSekcji_m, decimal dlugoscSekcji_m, decimal ilosc_m2)
        {
            if (ilosc_m2 == 0 ||
                String.IsNullOrEmpty(kodZgrzewu) ||
                szerokoscSekcji_m == 0 ||
                dlugoscSekcji_m == 0)
                return 0;

            //gdy przeslano dl i szer w mm
            if (dlugoscSekcji_m > 50)
                dlugoscSekcji_m = dlugoscSekcji_m / 1000;
            if (szerokoscSekcji_m > 50)
                szerokoscSekcji_m = szerokoscSekcji_m / 1000;


            var iloscPodana = ilosc_m2;
            var powierzchniaSekcji = dlugoscSekcji_m * szerokoscSekcji_m;
            var iloscWlasciwa = Math.Ceiling(iloscPodana / powierzchniaSekcji) * powierzchniaSekcji;

            return (int)(iloscWlasciwa / powierzchniaSekcji);
        }
        public int ObliczIloscSekcji(string nazwa, decimal ilosc_m)
        {
            var zgrzew = PobierzZgrzewZNazwy(nazwa);
            if (zgrzew == null)
                return 0;

            var parametryGeometryczne = ListaParametrowGeometrycznych.SingleOrDefault(s => s.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew);
            if (parametryGeometryczne == null)
                return 0;

            var iloscPodana = ilosc_m;
            var powierzchniaSekcji = parametryGeometryczne.DlugoscStandardowaSekcji_m * parametryGeometryczne.SzerokoscStandardowaSekcji_m;
            var iloscWlasciwa = Math.Ceiling(iloscPodana / powierzchniaSekcji) * powierzchniaSekcji;

            return (int)(iloscWlasciwa / powierzchniaSekcji);
        }

        public tblTowarGeokomorkaParametryTyp PobierzTypZNazwy(string nazwa)
        {
            if (String.IsNullOrEmpty(nazwa))
                return null;

            string[] typyArray = listaTypow.Select(s => s.Typ.ToUpper()).ToArray();
            Array.Sort(typyArray, SortujNazwyZUwagiNaPLUS);
            string typGeokomorki = null;


            switch (typyArray.FirstOrDefault<string>(s => nazwa.ToUpper().Contains(s.ToUpper())))
            {
                case "STANDARD PLUS":
                    typGeokomorki = "STANDARD PLUS";
                    break;
                case "STANDARD":
                    typGeokomorki = "STANDARD";
                    break;
                case "COMFORT PLUS":
                    typGeokomorki = "COMFORT PLUS";
                    break;
                case "COMFORT":
                    typGeokomorki = "COMFORT";
                    break;
                case "PREMIUM PLUS":
                    typGeokomorki = "PREMIUM PLUS";
                    break;
                case "PREMIUM":
                    typGeokomorki = "PREMIUM";
                    break;
            }

            return listaTypow.FirstOrDefault(t => t.Typ.ToUpper() == typGeokomorki);
        }
        private static int SortujNazwyZUwagiNaPLUS(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;

                return -1;
            }

            if (y == null)
                return 1;

            if (x.ToUpper().Contains("PLUS") &&
                        !y.ToUpper().Contains("PLUS"))
                return 1;

            if (!x.ToUpper().Contains("PLUS") &&
                    y.ToUpper().Contains("PLUS"))
                return 1;

            return 1;
        }

        public tblTowarGeokomorkaParametryZgrzew PobierzZgrzewZNazwy(string nazwa)
        {
            if (String.IsNullOrEmpty(nazwa))
                return null;

            var podzial1 = nazwa.Split('.');
            var podzial2 = podzial1[0].Split(' ');

            var kodZgrzewu = podzial2[podzial2.Length - 1];

            return listaZgrzewow.FirstOrDefault(z => z.KodZgrzewu == kodZgrzewu);
        }

        public tblTowarGeokomorkaParametryRodzaj PobierzRodzajZNazwy(string nazwa)
        {
            if (String.IsNullOrEmpty(nazwa))
                return null;

            string rodzaj = null;

            if (nazwa.ToUpper().Contains("AT CELL"))
            {
                rodzaj = "AT CELL";
            }
            else if (nazwa.ToUpper().Contains("PINEMA"))
            {
                rodzaj = "PINEMA";
            }

            return listaRodzajow.FirstOrDefault(z => z.Rodzaj.ToUpper() == rodzaj);
        }
        public decimal PobierzWysokoscZNazwy(string nazwa)
        {
            if (String.IsNullOrEmpty(nazwa))
                return 0;

            if (!nazwa.Contains("."))
                return 0;

            if (nazwa.IndexOf(".") == nazwa.Length - 1)
                return 0;

            var i = nazwa.IndexOf('.');

            var podzial1 = nazwa.Split('.');
            var podzial2 = podzial1[1].Split(' ');

            if (podzial2 == null)
                return 0;

            var wysokosc = podzial2[0];

            return Convert.ToDecimal(wysokosc);
        }

        public decimal ObliczWage(string nazwa, decimal ilosc_m2)
        {
            var zgrzew = PobierzZgrzewZNazwy(nazwa);
            var typ = PobierzTypZNazwy(nazwa);
            var wysokosc = PobierzWysokoscZNazwy(nazwa);

            if (zgrzew == null ||
                typ == null ||
                wysokosc == 0)
                return 0;

            var wagaKomorki = (typ.GruboscPasa - 0.44M) * wysokosc * (zgrzew.Zgrzew + 16) * 0.96M / 1000000;
            var parametryGeometryczneDlaDanejGCE = listaParametrowGeometrycznych.SingleOrDefault(s => s.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew);
            var iloscKomorekNaM2 = 1 / (parametryGeometryczneDlaDanejGCE.PowierzchniaKomorki_cm2 / 10000);
            var wagaM2 = iloscKomorekNaM2 * wagaKomorki;

            return ilosc_m2 * wagaM2;
        }

        public decimal ObliczWage(int zgrzew, string typ, int wysokosc, decimal ilosc_m2)
        {
            if (zgrzew == 0 ||
                typ == null ||
                wysokosc == 0)
                return 0;

            var zgrzewEntity = ListaZgrzewow.SingleOrDefault(s => s.Zgrzew == zgrzew);
            if (zgrzewEntity == null)
                return 0;

            var typEntity = ListaTypow.SingleOrDefault(s => s.Typ == typ);
            if (typEntity == null)
                return 0;

            var gruboscPasa = ListaTypow.Where(s => s.Typ == typ).Select(s => s.GruboscPasa).FirstOrDefault();
            var wagaKomorki = (gruboscPasa - 0.44M) * wysokosc * (zgrzew + 16) * 0.96M / 1000000;
            var parametryGeometryczneDlaDanejGCE = listaParametrowGeometrycznych.SingleOrDefault(s => s.IDTowarParametryGeokomorkaZgrzew == zgrzewEntity.IDTowarGeokomorkaParametryZgrzew);
            var iloscKomorekNaM2 = 1 / (parametryGeometryczneDlaDanejGCE.PowierzchniaKomorki_cm2 / 10000);
            var wagaM2 = iloscKomorekNaM2 * wagaKomorki;

            return ilosc_m2 * wagaM2;
        }


        public string GenerujNazwePelna(string typ,
                                        string rodzaj,
                                        string kodZgrzewu,
                                        int zgrzew,
                                        int wysokosc,
                                        int szerokoscSekcji,
                                        int dlugoscSekcji,
                                        decimal ilosc_m2)
        {
            var iloscSekcji = ilosc_m2 / (szerokoscSekcji / 1000 * dlugoscSekcji / 1000);
            iloscSekcji = Math.Round(iloscSekcji);
            return String.Format($"Geokomórka {typ} {rodzaj} {kodZgrzewu}.{wysokosc} ({szerokoscSekcji}x{dlugoscSekcji}mm) [zgrzew: {zgrzew}], Ilość [m2]: {ilosc_m2}, Ilość sekcji [szt.]: {iloscSekcji}");
        }

        public string GenerujNazwePelna(string nazwa, decimal ilosc_m2)
        {

            var typ = PobierzTypZNazwy(nazwa);
            var zgrzew = PobierzZgrzewZNazwy(nazwa);
            var wysokosc = PobierzWysokoscZNazwy(nazwa);
            var rodzaj = PobierzRodzajZNazwy(nazwa);
            var szerokoscSekcji_mm = Math.Round(PobierzStandardowaSzerokoscSekcjiZNazwy_m(nazwa) * 1000);
            var dlugoscSekcjii_mm = Math.Round(PobierzStandardowaDlugoscSekcjiZNazwy_m(nazwa) * 1000);
            var iloscWlasciwa = Math.Round(ObliczIloscM2ZgodnaZPowierzchniaSekcji(nazwa, ilosc_m2), 2);
            var iloscSekcjiWlasciwa = ObliczIloscSekcji(nazwa, ilosc_m2);

            return String.Format($"Geokomórka {rodzaj.Rodzaj} {typ.Typ} {zgrzew.KodZgrzewu}.{wysokosc} ({dlugoscSekcjii_mm}x{szerokoscSekcji_mm}mm) [zgrzew: {zgrzew.Zgrzew}], Ilość [m2]: {iloscWlasciwa}, Ilość sekcji [szt.]: {iloscSekcjiWlasciwa}");
        }

        public decimal PobierzStandardowaSzerokoscSekcjiZNazwy_m(string nazwa)
        {
            var zgrzew = PobierzZgrzewZNazwy(nazwa);

            var szerokoscSekcji = ListaParametrowGeometrycznych
                                    .Where(p => p.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew)
                                    .Select(p => p.SzerokoscStandardowaSekcji_m)
                                    .SingleOrDefault();

            return szerokoscSekcji;
        }

        public decimal PobierzStandardowaDlugoscSekcjiZNazwy_m(string nazwa)
        {
            var zgrzew = PobierzZgrzewZNazwy(nazwa);

            var dlugoscSekcji = ListaParametrowGeometrycznych
                                    .Where(p => p.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew)
                                    .Select(p => p.DlugoscStandardowaSekcji_m)
                                    .SingleOrDefault();

            return dlugoscSekcji;
        }

        public tblTowarGeokomorkaParametryGeometryczne PobierzParametryGeometryczneZeZgrzewu(string kodZgrzewu)
        {
            var zgrzew = ListaZgrzewow.Where(z => z.KodZgrzewu == kodZgrzewu).SingleOrDefault();

            if (zgrzew == null)
                return null;

            return ListaParametrowGeometrycznych
                        .Where(p => p.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew)
                        .SingleOrDefault();
        }

        public async Task<tblTowar> PobierzTowarAsync(tblTowarGeokomorkaParametryRodzaj rodzaj,
                                                    tblTowarGeokomorkaParametryTyp typ,
                                                    tblTowarGeokomorkaParametryZgrzew zgrzew,
                                                    int wysokosc)
        {
            if (rodzaj == null || typ == null || zgrzew == null || wysokosc == 0)
                return null;

            var listaTowarow = await unitOfWork.tblTowar.WhereAsync(t =>
                                            t.Nazwa.Contains(rodzaj.Rodzaj) &&
                                            t.Nazwa.Contains(typ.Typ) &&
                                            t.Nazwa.Contains(zgrzew.KodZgrzewu) &&
                                            t.Nazwa.Contains("." + wysokosc.ToString()))
                                            .ConfigureAwait(false);
            if (listaTowarow.Count() > 1)
            {
                if (rodzaj.Rodzaj.ToUpper().Contains("PLUS"))
                    return listaTowarow.SingleOrDefault(r => r.Nazwa.ToUpper().Contains("PLUS"));

                return listaTowarow.SingleOrDefault(r => !r.Nazwa.ToUpper().Contains("PLUS"));
            }

            return listaTowarow.SingleOrDefault();
        }

        public decimal ObliczWageSekcji_kg(string nazwa)
        {
            if (nazwa == null)
                return 0;

            var zgrzew = PobierzZgrzewZNazwy(nazwa);
            if (zgrzew == null)
                return 0;

            var parametryGeometryczneDlaDanejGCE = listaParametrowGeometrycznych.SingleOrDefault(s => s.IDTowarParametryGeokomorkaZgrzew == zgrzew.IDTowarGeokomorkaParametryZgrzew);
            var powierzchniaSekcji = parametryGeometryczneDlaDanejGCE.PowierzchniaSekcji_m2;

            if (powierzchniaSekcji == 0)
                return 0;

            return Math.Ceiling(ObliczWage(nazwa, powierzchniaSekcji)*100)/100;
        }
    }
}

