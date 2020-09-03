using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieZamowieniaOdKlientowHelper : IPodsumowanieZamowieniaOdKlientowHelper
    {
        private PodsFinans_ZamowienieOdKlientowModel podsumowanie;
        private List<PodsFinans_ZamowienieOdKlientowModel> podsumowanieLista;


        private PodsFinans_ZamowienieOdKlientowModel zamowieniaOdKlientowPodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
        private readonly IUnitOfWork unitOfWork;

        public PodsFinans_ZamowienieOdKlientowModel Podsumowanie
        {
            get => new PodsFinans_ZamowienieOdKlientowModel
            {
                IloscCalkowita = zamowieniaOdKlientowPodsumowanie.IloscCalkowita,
                WartoscCalkowita = zamowieniaOdKlientowPodsumowanie.WartoscCalkowita,
            };
            set => podsumowanie = value;
        }


        public bool IsButtonActive { get; set; } = true;

        private DateTime dataOd;
        private DateTime dataDo;
        private IEnumerable<vwZamOdKlientaAGG> listaZlecen;

        #region CTOR
        public PodsumowanieZamowieniaOdKlientowHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<List<PodsFinans_ZamowienieOdKlientowModel>> PodsumujZamowieniaOdKlientow(DateTime dataOd, DateTime dataDo)
        {
            IsButtonActive = false;
            this.dataOd = dataOd;
            this.dataDo = dataDo;

            ResetowanieZmiennych();

            listaZlecen = await unitOfWork.vwZamOdKlientaAGG.GetAllAsync();

            if (listaZlecen is null ||
                !listaZlecen.Any())
                return new List<PodsFinans_ZamowienieOdKlientowModel>();

            GenerujZamowieniaZOkresu();
            GenerujZamowieniaNiezrealizowaneAll();

            podsumowanieLista.Add(zamowieniaOdKlientowPodsumowanie);
            
            IsButtonActive = true;

            return podsumowanieLista;
        }

        private void ResetowanieZmiennych()
        {
            zamowieniaOdKlientowPodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
            podsumowanieLista = new List<PodsFinans_ZamowienieOdKlientowModel>();
        }

        private void GenerujZamowieniaNiezrealizowaneAll()
        {
            var zamowieniaNiezrealizowane = listaZlecen
                            .Where(z => z.StatusEx == (int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane)
                            .ToList();

            zamowieniaOdKlientowPodsumowanie.IloscCalkowita = zamowieniaNiezrealizowane.Sum(z => z.Ilosc);
            zamowieniaOdKlientowPodsumowanie.WartoscCalkowita = zamowieniaNiezrealizowane.Sum(z => z.WartNetto);
        }

        private void GenerujZamowieniaZOkresu()
        {

            var zamowieniaZOkresu = listaZlecen
                                        .Where(z => z.DataWyst >= dataOd && z.DataWyst <= dataDo)
                                        .ToList();

            zamowieniaOdKlientowPodsumowanie.IloscTK = zamowieniaZOkresu.Sum(z => z.Ilosc);
            zamowieniaOdKlientowPodsumowanie.WartoscTK = zamowieniaZOkresu.Sum(z => z.WartNetto);
        }

    }
}
