using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    public partial class tblProdukcjaZlecenieProdukcyjneMieszanka
    {
        [NotMapped]
        public string NazwaTowaru { get; set; }
        [NotMapped]
        public string JmNazwa { get; set; }

    }
}
