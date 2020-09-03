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
    public class PodsumowanieNaleznosciIZobowiazaniaHelper : IPodsumowanieNaleznosciIZobowiazaniaHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<vwFinanseNalZobGTX2> nalZobGTX2;
        private IEnumerable<vwFinanseNalZobGTX> nalZobGTX;
        private IEnumerable<vwFinanseNalZobAGG> nalZobAGG;
        private DateTime dataDo;
        private PodsFinans_NaleznosciIZobowiazaniaModel podsumowanie;

        public PodsFinans_NaleznosciIZobowiazaniaModel Podsumowanie
        {
            get => new PodsFinans_NaleznosciIZobowiazaniaModel
            {
                NaleznosciAll = nalZobAGG.Sum(s => s.Naleznosc.GetValueOrDefault()) + nalZobGTX.Sum(s => s.Naleznosc.GetValueOrDefault()) + nalZobGTX2.Sum(s => s.Naleznosc.GetValueOrDefault()),
                ZobowiazaniaAll = nalZobAGG.Sum(s => s.Zobowiazanie.GetValueOrDefault()) + nalZobGTX.Sum(s => s.Zobowiazanie.GetValueOrDefault()) + nalZobGTX2.Sum(s => s.Zobowiazanie.GetValueOrDefault()),
            }; 
            set => podsumowanie = value;
        }
        public bool IsButtonActive { get; set; } = true;

        public PodsumowanieNaleznosciIZobowiazaniaHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PodsFinans_NaleznosciIZobowiazaniaModel>> PobierzPodsumowanieNalzenosciIZobowiazan(DateTime dataDo)
        {
            IsButtonActive = false;
            this.dataDo = dataDo;
            await PobierzNaleznosciIZobowiazaniaDlaFirm(dataDo);

            var podsumowanie = new List<PodsFinans_NaleznosciIZobowiazaniaModel>();

            podsumowanie.Add(PodsumujNaleznosciIZobowiazaniaDla(nalZobAGG));
            podsumowanie.Add(PodsumujNaleznosciIZobowiazaniaDla(nalZobGTX));
            podsumowanie.Add(PodsumujNaleznosciIZobowiazaniaDla(nalZobGTX2));

            IsButtonActive = true;
            return podsumowanie;
        }

        private async Task PobierzNaleznosciIZobowiazaniaDlaFirm(DateTime dataDo)
        {
            nalZobGTX2 = await unitOfWork.vwFinanseNalZobGTX2.GetAllAsync();
            nalZobGTX = await unitOfWork.vwFinanseNalZobGTX.GetAllAsync();
            nalZobAGG = await unitOfWork.vwFinanseNalZobAGG.GetAllAsync();

            //nalZobGTX2 = await unitOfWork.vwFinanseNalZobGTX2.WhereAsync(r => r.TerminPlatnosci <= dataDo);
            //nalZobGTX = await unitOfWork.vwFinanseNalZobGTX.WhereAsync(r => r.TerminPlatnosci <= dataDo);
            //nalZobAGG = await unitOfWork.vwFinanseNalZobAGG.WhereAsync(r => r.TerminPlatnosci <= dataDo);
        }

        private PodsFinans_NaleznosciIZobowiazaniaModel PodsumujNaleznosciIZobowiazaniaDla(IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazan)
        {
            var podsumowanieNiZ = new PodsFinans_NaleznosciIZobowiazaniaModel();

            PodsumujNaleznosciIZobowiazaniaAll(listaNaleznosciIZobowiazan, podsumowanieNiZ);
            PodsumujNaleznosciIZobowiazaniaDoDaty(listaNaleznosciIZobowiazan, podsumowanieNiZ);

            return podsumowanieNiZ;
            //string firma = PobierzFirme(listaNaleznosciIZobowiazan.First().GetType());
            //return new PodsWDatach_NaleznosciIZobowiazaniaModel
            //{
            //    Firma = firma,
            //    NaleznosciDoDaty = listaNaleznosciIZobowiazan.Sum(s => s.Naleznosc).GetValueOrDefault(),
            //    ZobowiazaniaDoDaty = listaNaleznosciIZobowiazan.Sum(s => s.Zobowiazanie).GetValueOrDefault(),
            //};
        }

        private void PodsumujNaleznosciIZobowiazaniaAll(IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazan, PodsFinans_NaleznosciIZobowiazaniaModel podsumowanieNiZ)
        {
            var lista = listaNaleznosciIZobowiazan;

            string firma = PobierzFirme(lista.First().GetType());

            podsumowanieNiZ.Firma = firma;
            podsumowanieNiZ.NaleznosciAll = lista.Sum(s => s.Naleznosc).GetValueOrDefault();
            podsumowanieNiZ.ZobowiazaniaAll = lista.Sum(s => s.Zobowiazanie).GetValueOrDefault();

        }

        private void PodsumujNaleznosciIZobowiazaniaDoDaty(IEnumerable<IFinanseNaleznosciZobowiazania> listaNaleznosciIZobowiazan, PodsFinans_NaleznosciIZobowiazaniaModel podsumowanieNiZ)
        {
            var lista = listaNaleznosciIZobowiazan.Where(s => s.TerminPlatnosci <= dataDo);

            string firma = PobierzFirme(lista.First().GetType());

            podsumowanieNiZ.Firma = firma;
            podsumowanieNiZ.NaleznosciDoDaty = lista.Sum(s => s.Naleznosc).GetValueOrDefault();
            podsumowanieNiZ.ZobowiazaniaDoDaty = lista.Sum(s => s.Zobowiazanie).GetValueOrDefault();
        }

        private string PobierzFirme(Type type)
        {
            switch (type.Name)
            {
                case string n when n.ToLower().Contains("gtx2"):
                    return "GTEX2 sp. z o.o.";
                case string n when n.ToLower().Contains("gtx"):
                    return "GTEX sp. z o.o.";
                default:
                    return "AG Geosynthetics sp. z o.o. sp. k.";
            }
        }
    }
}
