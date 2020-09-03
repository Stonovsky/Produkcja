using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("AGG.vwZestSprzedazyAGG")]

    public class vwZestSprzedazyAGG
    {
        [Key]
        public long Id { get; set; }
        public string IdFirma { get; set; }
        public string Firma { get; set; }
        public int DokId { get; set; }
        public DateTime DataSprzedazy { get; set; }
        public int Rok { get; set; }
        public string NazwaKontrahenta { get; set; }
        public string AdresKontrahenta { get; set; }
        public string MiejscowoscKontrahenta { get; set; }
        public string Kodocztowy { get; set; }
        public string NIP { get; set; }
        public string Towar { get; set; }
        public string NrDokSprzedazy { get; set; }
        public int DokTyp { get; set; }
        public string TytulDok { get; set; }
        public string Podtytul { get; set; }
        public decimal Ilosc { get; set; }
        public string Jm { get; set; }
        public decimal CenaJedn { get; set; }
        public decimal Netto { get; set; }
        public decimal Brutto { get; set; }
        public decimal Koszt { get; set; }
        public decimal Zysk { get; set; }
        public decimal Marza { get; set; }
        public string Grupa { get; set; }
        public string Handlowiec { get; set; }
        public string Nroferty { get; set; }
    }
}
