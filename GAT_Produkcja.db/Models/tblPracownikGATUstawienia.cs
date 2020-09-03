using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    // TODO Klasa nie dodana do bazy - do dodanie w momencie gdy opracuje sie dodawanie pracownika z poziomu formularza zeby tworzyc od razu kilka rekordow w kilku tabelach
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblPracownikGATUstawienia")]
    public partial class tblPracownikGATUstawienia
    {
        [Key]
        public int IDPracownikGATUstawienia { get; set; }

        [ForeignKey(nameof(tblPracownikGAT))]
        public int ID_PracownikGAT { get; set; }
        
        [ForeignKey(nameof(tblPracownikGATDostep))]
        public int ID_Dostep { get; set; }
        
        [DefaultValue(1)]
        public int MotywKoloruAplikacji { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
        public virtual tblPracownikGATDostep tblPracownikGATDostep { get; set; }
    }
}
