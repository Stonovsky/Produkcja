using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieProdukcjaHelper : IPodsumowanieProdukcjaHelper
    {
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private readonly IRozliczenieMsAccesHelper rozliczenieMsAccesHelper;
        private readonly IUnitOfWork unitOfWork;
        private List<PodsFinans_ProdukcjaModel> podsumowanieLista;

        public PodsFinans_ProdukcjaModel Podsumowanie
        {
            get => new PodsFinans_ProdukcjaModel
            {
                Ilosc_kg = podsumowanieLista.Sum(s => s.Ilosc_kg),
                Ilosc_m2 = podsumowanieLista.Sum(s => s.Ilosc_m2),
                Wartosc = podsumowanieLista.Sum(s => s.Wartosc),
            };
        }
        public bool IsButtonActive { get; set; } = true;

        public PodsumowanieProdukcjaHelper(IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                           IRozliczenieMsAccesHelper rozliczenieMsAccesHelper,
                                           IUnitOfWork unitOfWork
                                           )
        {
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
            this.rozliczenieMsAccesHelper = rozliczenieMsAccesHelper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<PodsFinans_ProdukcjaModel>> PobierzPodsumowanieProdukcjiWDatach(DateTime dataOd, DateTime dataDo)
        {
            IsButtonActive = false;

            podsumowanieLista = new List<PodsFinans_ProdukcjaModel>();

            await PodsumowanieProdukcjiGeowloknin(dataOd, dataDo);
            await PodsumowanieProdukcjiGeokomorekWgDat(dataOd, dataDo);

            IsButtonActive = true;
            return podsumowanieLista;
        }

        private async Task PodsumowanieProdukcjiGeokomorek(DateTime dataOd, DateTime dataDo)
        {
            //var przerobGeokomorki = await unitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob.SingleOrDefaultAsync(g => g.DataDo == dataDo);
            var dataPoczatkowa = dataDo.AddDays(-14);
            var przerobyZOstatnichDwochTygodni = await unitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob.WhereAsync(g => g.DataOd >= dataPoczatkowa
                                                                                                             && g.DataDo <= dataDo);
            przerobyZOstatnichDwochTygodni = przerobyZOstatnichDwochTygodni.OrderByDescending(e => e.DataDo).ToList();

            if (!przerobyZOstatnichDwochTygodni.Any())
            {
                //podsumowanieLista.Add(new PodsFinans_ProdukcjaModel());
                return;
            } 

            var przerobGeokomorki = przerobyZOstatnichDwochTygodni.First();

            var podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel();

            if (przerobGeokomorki != null)
            {
                podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel
                {
                    RodzajProdukcji = $"Geokomórka od {przerobGeokomorki.DataOd.ToString("dd/MM/yyyy")} do {przerobGeokomorki.DataDo.ToString("dd/MM/yyyy")}",
                    Ilosc_kg = przerobGeokomorki.Ilosc_kg,
                    Ilosc_m2 = przerobGeokomorki.Ilosc_m2,
                    Wartosc = przerobGeokomorki.Wartosc,
                };
            }
            else
            {
                podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel
                {
                    RodzajProdukcji = "Geokomórka",
                };
            }
            podsumowanieLista.Add(podsumowanieGeokomorka);
        }

        private async Task PodsumowanieProdukcjiGeokomorekWgDat(DateTime dataOd, DateTime dataDo)
        {
            //var przerobGeokomorki = await unitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob.SingleOrDefaultAsync(g => g.DataDo == dataDo);
            var dataPoczatkowa = dataDo.AddDays(-14);
            var przerobyWgDat = await unitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob.WhereAsync(g => g.DataOd >= dataOd
                                                                                                                         && g.DataDo <= dataDo);
            przerobyWgDat = przerobyWgDat.OrderByDescending(e => e.DataDo).ToList();

            if (!przerobyWgDat.Any())
            {
                //podsumowanieLista.Add(new PodsFinans_ProdukcjaModel());
                return;
            }

            var przerobGeokomorki = przerobyWgDat.First();

            var podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel();

            if (przerobGeokomorki != null)
            {
                podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel
                {
                    RodzajProdukcji = $"Geokomórka od {przerobyWgDat.Last().DataOd.ToString("dd/MM/yyyy")} do {przerobyWgDat.First().DataDo.ToString("dd/MM/yyyy")}",
                    Ilosc_kg = przerobyWgDat.Sum(s=>s.Ilosc_kg),
                    Ilosc_m2 = przerobyWgDat.Sum(s=>s.Ilosc_m2),
                    Wartosc = przerobyWgDat.Sum(s=>s.Wartosc),
                };
            }
            else
            {
                podsumowanieGeokomorka = new PodsFinans_ProdukcjaModel
                {
                    RodzajProdukcji = "Geokomórka",
                };
            }
            podsumowanieLista.Add(podsumowanieGeokomorka);
        }

        private async Task PodsumowanieProdukcjiGeowloknin(DateTime dataOd, DateTime dataDo)
        {
            var produkcjaWDatach = await unitOfWorkMsAccess.Produkcja.GetByDateAsync(dataOd, dataDo);
            var listaZgrupowanaPW = await rozliczenieMsAccesHelper.GenerujZgrupowanaListePoNazwieZObliczeniemKosztow(produkcjaWDatach);

            var podsumowanieGeowloknin = new PodsFinans_ProdukcjaModel
            {
                RodzajProdukcji = $"Geowłóknina od {dataOd.ToString("dd/MM/yyyy")} do {dataDo.ToString("dd/MM/yyyy")}",
                Ilosc_kg = listaZgrupowanaPW.Sum(s => s.Ilosc_kg),
                Ilosc_m2 = listaZgrupowanaPW.Sum(s => s.Ilosc),
                Wartosc = listaZgrupowanaPW.Sum(s => s.Wartosc),
            };

            podsumowanieLista.Add(podsumowanieGeowloknin);
        }

        public async Task LoadAsync()
        {
            await rozliczenieMsAccesHelper.LoadAsync();
        }

    }
}
