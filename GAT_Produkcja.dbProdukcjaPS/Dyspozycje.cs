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
    
    public partial class Dyspozycje
    {
        public int ID { get; set; }
        public System.DateTime Data { get; set; }
        public string Nr_zlecenia { get; set; }
        public string Surowiec { get; set; }
        public string Artykuł { get; set; }
        public Nullable<int> Ilość_mb { get; set; }
        public bool Zakończ { get; set; }
    }
}
