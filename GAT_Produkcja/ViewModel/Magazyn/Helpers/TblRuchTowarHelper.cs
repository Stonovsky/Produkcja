using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Helpers
{
    public class TblRuchTowarHelper : ITblRuchTowarHelper
    {

        private IUnitOfWork unitOfWork;
        private const bool dodajemyIlosc = false;
        private const bool odejmujemyIlosc = true;

        public tblRuchTowar Towar { get; set; }

        public TblRuchTowarHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            Towar = new tblRuchTowar();
        }

        public async Task DodajDoBazyDanych(tblRuchTowar towar,
                                        tblRuchStatus statusRuchu,
                                        tblRuchNaglowek naglowek)
        {
            Towar = towar;
            Towar.IDRuchNaglowek = naglowek.IDRuchNaglowek;

            if (Towar.IDRuchTowar == 0)
            {
                switch (statusRuchu.IDRuchStatus)
                {
                    case (int)StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ:
                        await ZapiszWBazie(naglowek.IDMagazynDo.Value, DokumentTypEnum.PrzyjęcieZewnętrzne_PZ, dodajemyIlosc);
                        break;

                    case (int)StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM:
                        await ZapiszWBazie(naglowek.IDMagazynDo.Value, DokumentTypEnum.PrzesuniecieMiedzymagazynowe_MM, dodajemyIlosc);
                        await ZapiszWBazie(naglowek.IDMagazynZ.Value, DokumentTypEnum.PrzesuniecieMiedzymagazynowe_MM, odejmujemyIlosc);
                        break;

                    case (int)StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ:
                        await ZapiszWBazie(naglowek.IDMagazynZ.Value, DokumentTypEnum.WydanieZewnetrzne_WZ, odejmujemyIlosc);
                        break;

                    case (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW:
                        await ZapiszWBazie(naglowek.IDMagazynDo.Value, DokumentTypEnum.PrzyjecieWewnetrzne_PW, dodajemyIlosc);
                        break;

                    case (int)StatusRuchuTowarowEnum.RozchodWewnetrzny_RW:
                        await ZapiszWBazie(naglowek.IDMagazynZ.Value, DokumentTypEnum.RozchodWewnetrzny_RW, odejmujemyIlosc);
                        break;

                    default:
                        break;
                }
            }
        }

        private async Task ZapiszWBazie(int idMagazyn, DokumentTypEnum dokumentTypEnum, bool czyOdejmujemyZMagazynu)
        {
            Towar.IDDokumentTyp = (int)dokumentTypEnum;
            Towar.IDMagazyn = idMagazyn;
            await OkreslIlosciPoDodaniuDlaTowaru(idMagazyn, czyOdejmujemyZMagazynu);
            unitOfWork.tblRuchTowar.Add(Towar);
            await unitOfWork.SaveAsync();
        }

        private async Task OkreslIlosciPoDodaniuDlaTowaru(int idMagazynu, bool czyOdejmujemyZMagazynu)
        {
            var TowarRuch = await unitOfWork.tblRuchTowar.WhereAsync(t => t.IDTowar == Towar.IDTowar && t.IDMagazyn == idMagazynu)
                                                        .ConfigureAwait(false);

            Towar.IloscPrzed = TowarRuch.Sum(s => s.Ilosc);
            if (czyOdejmujemyZMagazynu)
            {
                if (Towar.Ilosc > 0)
                    Towar.Ilosc = decimal.Negate(Towar.Ilosc);
            }

            Towar.IloscPo = Towar.IloscPrzed + Towar.Ilosc;
        }
    }
}
