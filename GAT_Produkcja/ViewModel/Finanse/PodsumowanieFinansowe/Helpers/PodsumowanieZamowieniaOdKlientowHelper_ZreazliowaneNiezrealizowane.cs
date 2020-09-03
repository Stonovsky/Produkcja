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
    public class PodsumowanieZamowieniaOdKlientowHelper_ZreazliowaneNiezrealizowane : IPodsumowanieZamowieniaOdKlientowHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<vwZamOdKlientaAGG> listaZlecen;
        private DateTime dataOd;
        private DateTime dataDo;
        private PodsFinans_ZamowienieOdKlientowModel zamowieniaNieZrealizowanePodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
        private PodsFinans_ZamowienieOdKlientowModel zamowieniaZrealizowanePodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
        private List<PodsFinans_ZamowienieOdKlientowModel> podsumowanieLista;
        private PodsFinans_ZamowienieOdKlientowModel podsumowanie;

        public PodsFinans_ZamowienieOdKlientowModel Podsumowanie
        {
            get => new PodsFinans_ZamowienieOdKlientowModel
            {
                IloscCalkowita = zamowieniaNieZrealizowanePodsumowanie.IloscCalkowita,
                WartoscCalkowita = zamowieniaNieZrealizowanePodsumowanie.WartoscCalkowita,
            };
            set => podsumowanie = value;
        }
        public bool IsButtonActive { get; set; } = true;

        public PodsumowanieZamowieniaOdKlientowHelper_ZreazliowaneNiezrealizowane(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

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


            GenerujZamowieniaNiezrealizowane();
            GenerujZamowieniaZrealizowane();


            IsButtonActive = true;
            return podsumowanieLista;
        }


        private void ResetowanieZmiennych()
        {
            zamowieniaNieZrealizowanePodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
            zamowieniaZrealizowanePodsumowanie = new PodsFinans_ZamowienieOdKlientowModel();
            podsumowanieLista = new List<PodsFinans_ZamowienieOdKlientowModel>();
        }

        #region Zamowienia zrealizowane

        private void GenerujZamowieniaZrealizowane()
        {
            ZamowienieZrealizowaneAll(listaZlecen);
            ZamowienieZrealizowaneWBiezacymTygodniu(listaZlecen);

            podsumowanieLista.Add(zamowieniaZrealizowanePodsumowanie);
        }

        private void ZamowienieZrealizowaneWBiezacymTygodniu(IEnumerable<vwZamOdKlientaAGG> listaZlecen)
        {

            var zamowieniaZrealizowane = listaZlecen
                                        .Where(z => z.StatusEx != (int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane)
                                        .Where(z => z.DataWyst >= dataOd && z.DataWyst <= dataDo)
                                        .ToList();
            //var zamowieniaZrealizowane = listaZlecen
            //                    .Where(z => z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane
            //                                         && z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji
            //                                         && z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_zRezewacja)
            //                                .Where(z => z.DataWyst >= dataOd
            //                                         && z.DataWyst <= dataDo)
            //                                .ToList();

            zamowieniaZrealizowanePodsumowanie.CzyZrealizowano = true;
            zamowieniaZrealizowanePodsumowanie.IloscTK = zamowieniaZrealizowane.Sum(z => z.Ilosc);
            zamowieniaZrealizowanePodsumowanie.WartoscTK = zamowieniaZrealizowane.Sum(z => z.WartNetto);
        }

        private void ZamowienieZrealizowaneAll(IEnumerable<vwZamOdKlientaAGG> listaZlecen)
        {
            var zamowieniaZrealizowane = listaZlecen
                .Where(z => z.StatusEx != (int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane)
                .ToList();
            //.Where(z => z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane
            //         && z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji
            //         && z.Status != (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_zRezewacja)
            //.ToList();

            zamowieniaZrealizowanePodsumowanie.CzyZrealizowano = true;
            zamowieniaZrealizowanePodsumowanie.IloscCalkowita = zamowieniaZrealizowane.Sum(z => z.Ilosc);
            zamowieniaZrealizowanePodsumowanie.WartoscCalkowita = zamowieniaZrealizowane.Sum(z => z.WartNetto);
        }


        #endregion

        #region Zamowienia niezrealizowane

        private void GenerujZamowieniaNiezrealizowane()
        {
            ZamowienieNiezrealizowaneAll(listaZlecen);
            ZamowienieNiezrealizowaneWBiezacymTygodniu(listaZlecen);
            podsumowanieLista.Add(zamowieniaNieZrealizowanePodsumowanie);
        }

        private void ZamowienieNiezrealizowaneAll(IEnumerable<vwZamOdKlientaAGG> listaZamowienOdKlientow)
        {
            var zamowieniaNieZrealizowane = listaZamowienOdKlientow
                .Where(z => z.StatusEx == (int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane)
                .ToList();
            //.Where(z => z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane
            //    || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji
            //    || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_zRezewacja)
            //.ToList();

            zamowieniaNieZrealizowanePodsumowanie.CzyZrealizowano = false;
            zamowieniaNieZrealizowanePodsumowanie.IloscCalkowita = zamowieniaNieZrealizowane.Sum(z => z.Ilosc);
            zamowieniaNieZrealizowanePodsumowanie.WartoscCalkowita = zamowieniaNieZrealizowane.Sum(z => z.WartNetto);
        }

        private void ZamowienieNiezrealizowaneWBiezacymTygodniu(IEnumerable<vwZamOdKlientaAGG> listaZamowienOdKlientow)
        {
            var zamowieniaNieZrealizowane = listaZamowienOdKlientow
                            .Where(z => z.StatusEx == (int)ZamOdKlientaAGG_StatusEx_Enum.NieZrealizowane)
                            .Where(z => z.DataWyst >= dataOd
                                                         && z.DataWyst <= dataDo)
                            .ToList();
            //var zamowieniaNieZrealizowane = listaZamowienOdKlientow
            //                                    .Where(z => z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane
            //                                        || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji
            //                                        || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_zRezewacja)
            //                                    .Where(z => z.DataWyst >= dataOd
            //                                             && z.DataWyst <= dataDo)
            //                                    .ToList();

            zamowieniaNieZrealizowanePodsumowanie.CzyZrealizowano = false;
            zamowieniaNieZrealizowanePodsumowanie.IloscTK = zamowieniaNieZrealizowane.Sum(z => z.Ilosc);
            zamowieniaNieZrealizowanePodsumowanie.WartoscTK = zamowieniaNieZrealizowane.Sum(z => z.WartNetto);
        }
        #endregion
    }
}
