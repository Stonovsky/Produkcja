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
    public partial class tblRuchTowar
    {
        [NotMapped]
        [Required(ErrorMessage = "Pole wymagane")]
        public string TowarNazwa { get; set; }

        [NotMapped]
        public string Jm { get; set; }
    }
}
