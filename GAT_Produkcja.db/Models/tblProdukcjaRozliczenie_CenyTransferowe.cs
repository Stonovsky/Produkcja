using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe")]

    public partial class tblProdukcjaRozliczenie_CenyTransferowe
    {
        [Key]
        public int IDProdukcjaRozliczenie_CenyTransferowe { get; set; }
        public string TowarNazwa { get; set; }

        [ForeignKey(nameof(tblTowarGrupa))]
        public int IDTowarGrupa { get; set; }
        public decimal CenaTransferowa { get; set; }
        public decimal CenaHurtowa { get; set; }
        public DateTime? DataDodania { get; set; }
        public bool? CzyAktualna { get; set; }

        public virtual  tblTowarGrupa tblTowarGrupa { get; set; }
        public override string ToString()
        {
            return TowarNazwa+"; " + CenaTransferowa.ToString();
        }
    }
}
