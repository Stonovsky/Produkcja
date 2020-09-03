using GAT_Produkcja.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka.Helpers
{
    public class MieszankaKalkulatorCenyHelper
    {
        private IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki;


        private bool ListaIsValid(IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)
        {
            if (listaPozycjiMieszanki is null) return false;
            //if (!listaPozycjiMieszanki.Any()) return false;

            return true;
        }


        public IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> ObliczWartoscPozycji(
                                        IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)
        {
            if (!ListaIsValid(listaPozycjiMieszanki))
                throw new ArgumentException("Podany argument w postaci listy mieszanki jest nieprawidłowy.");

            foreach (var pozycja in listaPozycjiMieszanki)
                pozycja.Wartosc_kg = pozycja.Cena_kg * pozycja.IloscKg;

            return listaPozycjiMieszanki;
        }

        public decimal ObliczWartoscMieszanki(
                    IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)

        {
            if (!ListaIsValid(listaPozycjiMieszanki))
                throw new ArgumentException("Podany argument w postaci listy mieszanki jest nieprawidłowy.");

            listaPozycjiMieszanki = ObliczWartoscPozycji(listaPozycjiMieszanki);

            return listaPozycjiMieszanki.Sum(s => s.Wartosc_kg);
        }

        public decimal ObliczSredniaCeneMieszankiZaKg(IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)
        {
            var wartoscMieszanki = ObliczWartoscMieszanki(listaPozycjiMieszanki);
            var iloscMieszanki = listaPozycjiMieszanki.Sum(s => s.IloscKg);

            if (iloscMieszanki == 0) return 0;

            return wartoscMieszanki / iloscMieszanki;

        }

        public IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> DodajWartoscMieszankiDoPozycjiListy(
                                            IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka> listaPozycjiMieszanki)
        {
            if (!ListaIsValid(listaPozycjiMieszanki))
                throw new ArgumentException("Podany argument w postaci listy mieszanki jest nieprawidłowy.");

            listaPozycjiMieszanki = ObliczWartoscPozycji(listaPozycjiMieszanki);

            foreach (var pozycja in listaPozycjiMieszanki)
                pozycja.WartoscMieszanki = listaPozycjiMieszanki.Sum(s => s.Wartosc_kg);

            return listaPozycjiMieszanki;
        }

    }
}
