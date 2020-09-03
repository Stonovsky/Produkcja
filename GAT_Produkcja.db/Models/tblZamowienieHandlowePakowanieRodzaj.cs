using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Zamowienia.tblZamowienieHandlowePakowanieRodzaj")]
    public partial class tblZamowienieHandlowePakowanieRodzaj
    {
        [Key]
        [Column("IDZamowienieHandlowePakowanieRodzaj")]
        public int IDZamowienieHandlowePakowanieRodzaj { get; set; }

        public string RodzajPakowania { get; set; }
    }
}
