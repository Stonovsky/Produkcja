//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAT_PRodukcja.dbProdukcjaPS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kalander
    {
        public Nullable<int> Nr_zlecenia { get; set; }
        public string Nr_sztuki { get; set; }
        public string Szer_igł { get; set; }
        public Nullable<int> Waga_igł { get; set; }
        public Nullable<int> Długość_igł { get; set; }
        public System.DateTime Data { get; set; }
        public Nullable<System.DateTime> Godz { get; set; }
        public string Artykuł { get; set; }
        public Nullable<int> Operator { get; set; }
        public Nullable<int> Pomoc_operatora { get; set; }
        public string Szerokość { get; set; }
        public Nullable<int> Długość { get; set; }
        public Nullable<double> Waga { get; set; }
        public Nullable<double> Waga_krajki { get; set; }
        public bool Odpad { get; set; }
        public string Kod_odpadu { get; set; }
        public Nullable<int> Waga_odpad { get; set; }
        public Nullable<int> Konfekcja { get; set; }
        public Nullable<double> M2 { get; set; }
    }
}
