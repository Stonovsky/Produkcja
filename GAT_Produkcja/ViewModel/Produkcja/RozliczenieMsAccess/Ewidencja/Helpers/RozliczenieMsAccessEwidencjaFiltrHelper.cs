using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja.Helpers
{
    public class RozliczenieMsAccessEwidencjaFiltrHelper
    {
        private RozliczenieEwidencjaFiltrModel filtr;
        private IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie> listOfVMEntities;

        public IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie> FiltrujListe(IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie> lista,
                                                                                RozliczenieEwidencjaFiltrModel filtr)
        {
            if (lista is null)
                throw new ArgumentException("Brak listy do filtrowania");
            if (filtr == null) return lista;
            if (!lista.Any()) return lista;

            this.filtr = filtr;
            this.listOfVMEntities = lista;

            FiltrujDaty();
            FiltrujNazweTowaru();
            FiltrujJednostke();
            FiltrujRodzaj();

            return listOfVMEntities;
        }

        private void FiltrujRodzaj()
        {
            if (!string.IsNullOrEmpty(filtr.Rodzaj))
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.SymbolTowaruSubiekt.ToLower().Contains(filtr.Rodzaj.ToLower())));
        }

        private void FiltrujJednostke()
        {
            if (!string.IsNullOrEmpty(filtr.Jm))
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.Jm.ToLower().Contains(filtr.Jm.ToLower())));
        }

        private void FiltrujNazweTowaru()
        {
            if (!string.IsNullOrEmpty(filtr.Towar))
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.NazwaTowaruSubiekt.ToLower().Contains(filtr.Towar.ToLower())));
        }

        private void FiltrujDaty()
        {
            if (filtr.DataOd != null && filtr.DataDo!=null)
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.tblProdukcjaRozliczenie_Naglowek.DataDodania.Date >= filtr.DataOd.Date
                                               && e.tblProdukcjaRozliczenie_Naglowek.DataDodania.Date<=filtr.DataDo.Date));

            else if (filtr.DataDo != null)
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.tblProdukcjaRozliczenie_Naglowek.DataDodania <= filtr.DataDo));
           
            else if (filtr.DataOd != null)
                listOfVMEntities = new ObservableCollection<tblProdukcjaRozliczenie_PWPodsumowanie>
                     (listOfVMEntities.Where(e => e.tblProdukcjaRozliczenie_Naglowek.DataDodania >= filtr.DataOd));


        }

    }
}
