using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("AGG.vwZamOdKlientaAGG")]
    public class vwZamOdKlientaAGG
    {
        [Key]
        public int Id { get; set; }
        public int Typ { get; set; }
        public string NrPelny { get; set; }
        public string MscWyst { get; set; }
        public DateTime DataWyst { get; set; }
        public DateTime DataMag { get; set; }
        public DateTime TerminRealizacji { get; set; }
        public int Status { get; set; }
        public int StatusEx { get; set; }
        public int OdbiorcaId { get; set; }
        public string KontrahentNazwa { get; set; }
        public int TowId { get; set; }
        public string TowarNazwa { get; set; }
        public decimal Ilosc { get; set; }
        public string Jm { get; set; }
        public decimal CenaMag { get; set; }
        public decimal CenaNetto { get; set; }
        public decimal WartNetto { get; set; }
        public decimal WartBrutto { get; set; }
        public string Uwagi { get; set; }
        public int ObiektGT { get; set; }
        public int PersonelId { get; set; }
        public string Wystawil { get; set; }
        public string Grupa { get; set; }
    }
}
