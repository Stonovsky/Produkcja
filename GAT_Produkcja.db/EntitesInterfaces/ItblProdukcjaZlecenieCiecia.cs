using System;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface ItblProdukcjaZlecenieCiecia
    {
        DateTime? DataRozliczenia { get; set; }
        DateTime DataWykonania { get; set; }
        DateTime DataZlecenia { get; set; }
        int IDKontrahent { get; set; }
        int? IDProdukcjaRozliczenieStatus { get; set; }
        int IDProdukcjaZlecenieCiecia { get; set; }
        int IDProdukcjaZlecenieStatus { get; set; }
        int IDWykonujacy { get; set; }
        int IDZlecajacy { get; set; }
        string KodKreskowy { get; set; }
        string NrDokumentu { get; set; }
        int NrZleceniaCiecia { get; set; }
        string RodzajPakowania { get; set; }
        tblKontrahent tblKontrahent { get; set; }
        tblPracownikGAT tblPracownikGAT_Wykonujacy { get; set; }
        tblPracownikGAT tblPracownikGAT_Zlecajacy { get; set; }
        tblProdukcjaRozliczenieStatus tblProdukcjaRozliczenieStatus { get; set; }
        tblProdukcjaZlecenieStatus tblProdukcjaZlecenieStatus { get; set; }
        string Uwagi { get; set; }
        decimal Zaawansowanie { get; set; }
    }
}