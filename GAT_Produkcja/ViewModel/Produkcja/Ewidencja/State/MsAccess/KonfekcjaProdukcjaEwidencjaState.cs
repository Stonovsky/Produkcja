using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
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
    public class KonfekcjaProdukcjaEwidencjaState : ProdukcjaEwidencjaStateBase, IKonfekcjaProdukcjaEwidencjaState
    {
        public KonfekcjaProdukcjaEwidencjaState(IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                                IProdukcjaEwidencjaHelper helper)
            : base(unitOfWorkMsAccess, helper)
        {
        }


        public async override Task PobierzListeRolekZMsAccess()
        {
            //if (ListaPozycjiKalandra is null)
            ListaPozycjiKonfekcji = new ObservableCollection<IGniazdoProdukcyjne>(
                await UnitOfWorkMsAccess.Konfekcja.GetByDateAsync(DataPoczatek, DataKoniec));
        }
        public override async Task GrupujTowary()
        {
            listaZgrupowanaBazowa = await GrupujListeRolekWgNazwyTowaru(ListaPozycjiKonfekcji);
            ListaZgrupowanychPozycjiKonfekcji = FiltrujTowar(listaZgrupowanaBazowa);
        }

        public override void PodsumujListe()
        {
            Podsumowanie = GenerujPodsumowanieListy(ListaZgrupowanychPozycjiKonfekcji);
        }
    }
}
