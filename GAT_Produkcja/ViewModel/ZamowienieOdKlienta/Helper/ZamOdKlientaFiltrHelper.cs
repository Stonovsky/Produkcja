using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper
{
    public class ZamOdKlientaFiltrHelper : IZamOdKlientaFiltrHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<vwZamOdKlientaAGG> listaZamowien;
        private ObservableCollection<vwZamOdKlientaAGG> listaZamowienOdKlientow;


        public ZamOdKlientaFiltrHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<vwZamOdKlientaAGG>> FiltrujAsync(ZK_Filtr filtr)
        {
            listaZamowien = await unitOfWork.vwZamOdKlientaAGG.GetAllAsync();
            listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>(listaZamowien);

            if (!string.IsNullOrEmpty(filtr?.NazwaTowaru))
            {
                var listaFiltrNazwa = listaZamowienOdKlientow.Where(z => z.TowarNazwa.ToLower().Contains(filtr.NazwaTowaru.ToLower()));
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>(listaFiltrNazwa);
            }

            FiltrujDatePrzyjecia(filtr?.DataOd, filtr?.DataDo);
            FiltrujTerminRealizacji(filtr?.TerminRealizacjiOd, filtr?.TerminRealizacjiDo);
            FiltrujGrupe(filtr?.Grupa);
            FiltrujStatus(filtr?.Status);

            return listaZamowienOdKlientow; 
        }

        private void FiltrujGrupe(string nazwaGrupy)
        {
            if (nazwaGrupy != null)
            {
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                                (listaZamowienOdKlientow.Where(g => g.Grupa == nazwaGrupy));
            }
        }

        private void FiltrujStatus(string status)
        {
            if (status is null 
                || status.ToLower().Contains("nie"))
            {
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                    (listaZamowienOdKlientow.Where(z => z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane
                                                     || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_bezRezerwacji
                                                     || z.Status == (int)ZamOdKlientaAGG_Status_Enum.Niezrealizowane_zRezewacja));
            }
            else
            {
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                    (listaZamowienOdKlientow.Where(z => z.Status == (int)ZamOdKlientaAGG_Status_Enum.Zrealizowane));
            }
        }

        private void FiltrujTerminRealizacji(DateTime? terminOd, DateTime? terminDo)
        {
            if (terminOd != null)
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                                                (listaZamowienOdKlientow.Where(z => z.TerminRealizacji >= terminOd));
            if (terminDo != null)
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                                                (listaZamowienOdKlientow.Where(z => z.TerminRealizacji <= terminDo));
        }

        private void FiltrujDatePrzyjecia(DateTime? dataOd, DateTime? dataDo)
        {
            if (dataOd != null)
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                                                (listaZamowienOdKlientow.Where(z => z.DataWyst >= dataOd));
            if (dataDo != null)
                listaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                                                (listaZamowienOdKlientow.Where(z => z.DataWyst <= dataDo));
        }
    }
}
