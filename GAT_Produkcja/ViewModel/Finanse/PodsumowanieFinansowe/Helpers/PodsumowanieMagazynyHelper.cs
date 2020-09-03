using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieMagazynyHelper : IPodsumowanieMagazynyHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private PodsFinans_MagazynyModel podsumowanie;
        private DateTime dataDo;
        private IEnumerable<vwMagazynRuchGTX2> magazynGTX2;
        private IEnumerable<vwMagazynRuchGTX> magazynGTX;
        private IEnumerable<vwMagazynRuchAGG> magazynAGG;

        public PodsFinans_MagazynyModel Podsumowanie
        {
            get => new PodsFinans_MagazynyModel
            {
                Ilosc = magazynAGG.Sum(s => s.Ilosc) + magazynGTX.Sum(s => s.Ilosc) + magazynGTX2.Sum(s => s.Ilosc),
                Wartosc = magazynAGG.Sum(s => s.Wartosc) + magazynGTX.Sum(s => s.Wartosc) + magazynGTX2.Sum(s => s.Wartosc),

            }; 
            set => podsumowanie = value;
        }
        public bool IsButtonActive { get; set; } = true;

        #region CTOR
        public PodsumowanieMagazynyHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion
        public async Task<IEnumerable<PodsFinans_MagazynyModel>> PobierzPodsumowanieMagazynuDoDaty<T>(T TMagazyn, DateTime dataDo)
        {
            IsButtonActive = false;
            this.dataDo = dataDo;

            var magazyn = await PobierzMagazyn(TMagazyn, dataDo);
            IEnumerable<PodsFinans_MagazynyModel> listaPodsumowaniaMagazynow = GrupujMagazynyWgNazwy(magazyn);

            IsButtonActive = true;
            return listaPodsumowaniaMagazynow;
        }
        private async Task<IEnumerable<IMagazynRuchSubiekt>> PobierzMagazyn<T>(T TMagazyn, DateTime dataDo)
        {
            var nazwaTypu = TMagazyn.GetType().Name;

            if (nazwaTypu.ToLower().Contains("gtx2"))
            {
                magazynGTX2 = await unitOfWork.vwMagazynRuchGTX2.WhereAsync(r => r.Data <= dataDo);
                return magazynGTX2;
            }
            else if (nazwaTypu.ToLower().Contains("gtx"))
            {
                magazynGTX = await unitOfWork.vwMagazynRuchGTX.WhereAsync(r => r.Data <= dataDo);
                return magazynGTX;
            }
            else
            {
                magazynAGG = await unitOfWork.vwMagazynRuchAGG.WhereAsync(r => r.Data <= dataDo);
                return magazynAGG;
            }
        }

        private IEnumerable<PodsFinans_MagazynyModel> GrupujMagazynyWgNazwy(IEnumerable<IMagazynRuchSubiekt> magazyn)
        {
            return magazyn
                .GroupBy(g => g.IdMagazyn)
                .Select(s => new PodsFinans_MagazynyModel
                {
                    Lokalizacja = GenerujLokalizacje(s.First().MagazynNazwa),
                    NazwaMagazynu = s.First().MagazynNazwa,
                    Ilosc = s.Sum(p => p.Pozostalo),
                    Jm = s.First().Jm,
                    Wartosc = s.Sum(w => w.Wartosc)
                });
        }

        private string GenerujLokalizacje(string magazynNazwa)
        {
            if (magazynNazwa.ToLower().Contains("budown")
                || magazynNazwa.ToLower().Contains("eurofol"))
            {
                return "Magazyn regionalny";
            }
            else
                return "Magazyn Studzienice";
        }
    }
}
