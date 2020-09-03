using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess
{
    public class LiniaWlokninProdukcjaEwidencjaState : ProdukcjaEwidencjaStateBase, ILiniaWlokninProdukcjaEwidencjaState
    {
        public LiniaWlokninProdukcjaEwidencjaState(IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                                   IProdukcjaEwidencjaHelper helper)
            : base(unitOfWorkMsAccess, helper)
        {
        }


        public async override Task PobierzListeRolekZMsAccess()
        {
            ListaPozycjiWloknin = new ObservableCollection<IGniazdoProdukcyjne>();

            //if (ListaPozycjiWloknin is null)
            ListaPozycjiWloknin = new ObservableCollection<IGniazdoProdukcyjne>(
                await UnitOfWorkMsAccess.Produkcja.GetByDateAsync(DataPoczatek, DataKoniec));
        }
        public override async Task GrupujTowary()
        {
            ListaZgrupowanychPozycjiLiniWloknin = new List<tblProdukcjaRozliczenie_PW>();

            listaZgrupowanaBazowa = await GrupujListeRolekWgNazwyTowaru(ListaPozycjiWloknin);
            var iloscszt = listaZgrupowanaBazowa.Sum(s => s.Ilosc_szt);
            ListaZgrupowanychPozycjiLiniWloknin = FiltrujTowar(listaZgrupowanaBazowa);
        }

        public override void PodsumujListe()
        {
            Podsumowanie = GenerujPodsumowanieListy(ListaZgrupowanychPozycjiLiniWloknin);
        }
    }
}
