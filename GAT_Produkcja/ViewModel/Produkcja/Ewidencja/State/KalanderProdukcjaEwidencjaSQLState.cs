using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State
{
    public class KalanderProdukcjaEwidencjaSQLState : ProdukcjaEwidencjaSQLStateBase, IKalanderProdukcjaEwidencjaSQLState
    {
        public KalanderProdukcjaEwidencjaSQLState(IUnitOfWork unitOfWork,
                                                  IRozliczenieSQLHelper rozliczenieSQLHelper,
                                                  IProdukcjaEwidencjaHelper helper) : base(unitOfWork, rozliczenieSQLHelper, helper)
        {
        }

        public override async Task PobierzListeRolek()
        {
            ListaPozycjiKalandra = new ObservableCollection<tblProdukcjaRuchTowar>
                    (await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(e => e.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania
                                                                         && e.DataDodania >= DataPoczatek
                                                                         && e.DataDodania <= DataKoniec));
        }
        public override async Task GrupujTowary()
        {
            ListaZgrupowanychPozycjiKalandra = new List<tblProdukcjaRozliczenie_PW>();

            listaZgrupowanaBazowa = await GrupujListeRolekWgNazwyTowaru(ListaPozycjiKalandra);
            var iloscszt = listaZgrupowanaBazowa.Sum(s => s.Ilosc_szt);
            ListaZgrupowanychPozycjiKalandra = FiltrujTowar(listaZgrupowanaBazowa);
        }


        public override void PodsumujListe()
        {
            Podsumowanie = GenerujPodsumowanieListy(ListaZgrupowanychPozycjiKalandra);
        }
    }
}
