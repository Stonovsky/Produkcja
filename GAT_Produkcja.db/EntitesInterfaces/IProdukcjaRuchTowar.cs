using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IProdukcjaRuchTowar
    {

        /// <summary>
        /// Status ruchu, jako ruchu magazynowego, np RW, PW, MM etc. <see cref="StatusRuchuTowarowEnum"/>
        /// </summary>
        int IDRuchStatus { get;  }
        /// <summary>
        /// Id statusu rozliczeniowego, czyli czy rolke rozliczono czy nie rozliczono.
        /// <see cref="ProdukcjaRozliczenieStatusEnum"/>
        /// </summary>
        int IDProdukcjaRozliczenieStatus { get;  }
        /// <summary>
        /// ID enum <see cref="ProdukcjaRuchTowarStatusEnum"/>
        /// </summary>
        int? IDProdukcjaRuchTowarStatus { get;  }
        /// <summary>
        /// Id Gniazdo produkcyjnego <see cref="GniazdaProdukcyjneEnum"/>
        /// </summary>
        int? IDProdukcjaGniazdoProdukcyjne { get; }
        /// <summary>
        /// Id zlecenia produkcyujnego, na ktorego podstawie prowadzone sa rozliczenia produkcji. Cena mieszanki jest generowana z przez ID zlecenia produkcyjnego
        /// </summary>
        int? IDProdukcjaZlecenieProdukcyjne { get; }
        /// <summary>
        /// Id z tabeli MsAccess do identyfikacji rolki
        /// </summary>
        int? IDMsAccess { get; }
        /// <summary>
        /// Kod kreskowy rolki wyprodukowanej
        /// </summary>
        string KodKreskowy { get;  }
        /// <summary>
        /// Nr rolki wyprodukowanej
        /// </summary>
        int NrRolki { get;  }
        /// <summary>
        /// Nr pelny rolki wraz z prefixami i sufixami wskazujacymi na gniazdo produkcyjne
        /// </summary>
        string NrRolkiPelny { get;  }

        #region RolkaBazowa
        /// <summary>
        /// ID rolki bazowej, czyli tej, ktora powstala na linii wloknin
        /// </summary>
        int? IDRolkaBazowa { get;  }
        /// <summary>
        /// Nr rolki kotra powstala na linii wloknin
        /// </summary>
        string NrRolkiBazowej { get;  }
        string NazwaRolkiBazowej { get; }
        string SymbolRolkiBazowej { get; }
        /// <summary>
        /// Kod kreskowy rolki ktora powstala na linii wloknin
        /// </summary>
        string KodKreskowyRolkiBazowej { get;  }

        #endregion

        /// <summary>
        /// Nazwa towaru w MsAccess
        /// </summary>
        string TowarNazwaMsAccess { get; }
        /// <summary>
        /// Nazwa towaru w subiekcie
        /// </summary>
        string TowarNazwaSubiekt { get; }
        
        /// <summary>
        /// Symbol towaru w subiekcie
        /// </summary>
        string TowarSymbolSubiekt{ get; }
        /// <summary>
        /// ID gramatury <see cref="tblTowarGeowlokninaParametryGramatura"/>
        /// </summary>
        int IDGramatura { get;  }
        /// <summary>
        /// Gramatura
        /// </summary>
        int Gramatura { get;  }
        /// <summary>
        /// ID surowca <see cref="tblTowarGeowlokninaParametrySurowiec"/>
        /// </summary>
        int IDTowarGeowlokninaParametrySurowiec { get;  }
        /// <summary>
        /// Skrot surowca
        /// </summary>
        string SurowiecSkrot { get; }
        /// <summary>
        /// Szerokosc w metrach
        /// </summary>
        decimal Szerokosc_m { get;  }
        /// <summary>
        /// Dlugosc w metrach
        /// </summary>
        decimal Dlugosc_m { get;  }
        /// <summary>
        /// Waga w kg
        /// </summary>
        decimal Waga_kg { get;  }
        /// <summary>
        /// Ilosc w m2
        /// </summary>
        decimal Ilosc_m2 { get;  }
        /// <summary>
        /// Odpad w kg
        /// </summary>
        decimal WagaOdpad_kg { get;  }
        /// <summary>
        /// Cena zl/kg
        /// </summary>
        decimal Cena_kg { get;  }
        /// <summary>
        /// Cena zl/m2
        /// </summary>
        decimal Cena_m2 { get;  }
        /// <summary>
        /// Wartosc w zl => zwykle <see cref="Waga_kg"/> * <see cref="Cena_kg"/>
        /// </summary>
        decimal Wartosc { get;  }

        /// <summary>
        /// Flaga: Czy rolka kalandrowana, dla konfekcji jest TRUE
        /// </summary>
        bool CzyKalandrowana { get;  }
        /// <summary>
        /// Flaga: Czy rolka siedzi w tolerancjach parametrow 
        /// </summary>
        bool CzyParametryZgodne { get;  }
        /// <summary>
        /// Nr zlecenia produkcyjnego lub zlecenia ciecia
        /// </summary>
        int NrZlecenia { get;  }
        /// <summary>
        /// Nazwa zlecenia, gdy zlecenie jest pobierane z MsAccess lub przypisane NrZlecenia
        /// </summary>
        string ZlecenieNazwa { get; }
        /// <summary>
        /// Data dodania do bazy danych
        /// </summary>
        DateTime DataDodania { get;  }
        
        /// <summary>
        /// Kierunek skad rolka przyszla na dane gniazdo => Linia, Magazyn
        /// </summary>
        string KierunekPrzychodu { get; }

        /// <summary>
        /// Nr dokumentu dot biezacego ruchu towaru
        /// </summary>
        string NrDokumentu { get; }

        int? IDZleceniePodstawowe { get;  }
        int? NrZleceniaPodstawowego { get; }

    }
}
