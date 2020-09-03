using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess
{
    public class KalanderProdukcjaEwidencjaState : ProdukcjaEwidencjaStateBase, IKalanderProdukcjaEwidencjaState
    {
        public KalanderProdukcjaEwidencjaState(IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                               IProdukcjaEwidencjaHelper helper)
            : base(unitOfWorkMsAccess, helper)
        {
        }


        public override async Task PobierzListeRolekZMsAccess()
        {
            //if (ListaPozycjiKalandra is null)
            ListaPozycjiKalandra = new ObservableCollection<IGniazdoProdukcyjne>(
                    await UnitOfWorkMsAccess.Kalander.GetByDateAsync(DataPoczatek, DataKoniec));
        }
        public override async Task GrupujTowary()
        {
            listaZgrupowanaBazowa = await GrupujListeRolekWgNazwyTowaru(ListaPozycjiKalandra);
            ListaZgrupowanychPozycjiKalandra = FiltrujTowar(listaZgrupowanaBazowa);
        }

        public override void PodsumujListe()
        {
            Podsumowanie = GenerujPodsumowanieListy(ListaZgrupowanychPozycjiKalandra);
        }
    }
}
