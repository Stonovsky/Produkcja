using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje

{
    public class WeryfikacjaTolerancji : IWeryfikacjaTolerancji
    {
        private readonly IUnitOfWork unitOfWork;
        private WeryfikacjaTolerancjiResult rezultat;

        public WeryfikacjaTolerancji(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            rezultat = new WeryfikacjaTolerancjiResult();
        }



        public async Task<WeryfikacjaTolerancjiResult> CzyParametrZgodny(int idTowar, GeowlokninaParametryEnum parametr, int rzeczywistaWartoscParametru)
        {
            tblTowarGeowlokninaParametry parametryWymagane = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.IDTowar == idTowar);

            if (parametryWymagane == null)
                return null;

            rezultat = new WeryfikacjaTolerancjiResult
            {
                RodzajSprawdzanegoParametru = parametr,
                CzyParametrZgodnyZTolerancja = false,
                ParametrRzeczywisty = rzeczywistaWartoscParametru,
                ParametrWymagany = parametryWymagane.MasaPowierzchniowa
            };

            ZgodnoscParametrow(parametr, rzeczywistaWartoscParametru, parametryWymagane);

            await CzyMoznaPrzekwalifikowac(parametr, rzeczywistaWartoscParametru);

            return rezultat;
        }

        private void ZgodnoscParametrow(GeowlokninaParametryEnum parametr, int rzeczywistaWartoscParametru, tblTowarGeowlokninaParametry parametryWymagane)
        {
            switch (parametr)
            {
                case GeowlokninaParametryEnum.Gramatura:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.MasaPowierzchniowa_Minimum,
                                              parametryWymagane.MasaPowierzchniowa_Maksimum,
                                              "Gramatura");
                    break;

                case GeowlokninaParametryEnum.WytrzymaloscNaRozciaganie_MD:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.WytrzymaloscNaRozciaganie_MD_Minimum,
                                              parametryWymagane.WytrzymaloscNaRozciaganie_MD,
                                              "Wytrzymałość na rozciąganie MD");
                    break;

                case GeowlokninaParametryEnum.WytrzymaloscNaRozciaganie_CMD:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.WytrzymaloscNaRozciaganie_CMD_Minimum,
                                              parametryWymagane.WytrzymaloscNaRozciaganie_CMD,
                                              "Wytrzymałość na rozciąganie CMD");
                    break;

                case GeowlokninaParametryEnum.WyduzeniePrzyZerwaniu_MD:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.WydluzeniePrzyZerwaniu_MD_Minimum,
                                              parametryWymagane.WydluzeniePrzyZerwaniu_MD_Maksimum,
                                              "Wydłużenie przy zerwaniu MD");
                    break;

                case GeowlokninaParametryEnum.WyduzeniePrzyZerwaniu_CMD:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Minimum,
                                              parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Maksimum,
                                              "Wydłużenie przy zerwaniu CMD");
                    break;

                case GeowlokninaParametryEnum.OdpornoscNaPrzebicieStatyczne_CBR:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.OdpornoscNaPrzebicieStatyczne_CBR_Minimum,
                                              parametryWymagane.OdpornoscNaPrzebicieStatyczne_CBR,
                                              "Odporność na przebicie statyczne CBR");
                    break;

                case GeowlokninaParametryEnum.OdpornoscNaPrzebicieDynamiczne:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.OdpornoscNaPrzebicieDynamiczne,
                                              parametryWymagane.OdpornoscNaPrzebicieDynamiczne_Maksimum,
                                              "Odporność na przebicie dynamiczne");

                    break;
                case GeowlokninaParametryEnum.CharakterystycznaWielkoscPorow:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.CharakterystycznaWielkoscPorow_Minimum,
                                              parametryWymagane.CharakterystycznaWielkoscPorow_Maksimum,
                                              "Charaketrystyczna wielkość porów");
                    break;
                case GeowlokninaParametryEnum.WodoprzepuszczalnoscProsotpadla:
                    SprawdzZgodnoscParametrow(rzeczywistaWartoscParametru,
                                              parametryWymagane.WodoprzepuszczalnoscProsotpadla_Minimum,
                                              parametryWymagane.WodoprzepuszczalnoscProsotpadla,
                                              "Wodoprzepuszczalność prostopadła");
                    break;
                default:
                    break;
            }

        }

        private void SprawdzZgodnoscParametrow(decimal wartoscRzeczywista, decimal wartoscWymaganaMin, decimal wartoscWymaganaMax, string parametrNazwa)
        {
            if (wartoscRzeczywista > wartoscWymaganaMax ||
                wartoscRzeczywista < wartoscWymaganaMin)
            {
                string pozaTolerancja = GenerujUwage(wartoscRzeczywista,
                                                    wartoscWymaganaMin,
                                                    wartoscWymaganaMax,
                                                    parametrNazwa);

                rezultat.Uwagi = $"{parametrNazwa} nie mieści się w tolerancjach.\n{pozaTolerancja}";
                rezultat.CzyParametrZgodnyZTolerancja = false;
            }
            else
            {
                rezultat.CzyParametrZgodnyZTolerancja = true;
            }
        }

        private string GenerujUwage(decimal? parametrSprawdzany, decimal parametrMinimum, decimal parametrMaksimum, string nazwaParametru)
        {
            if (parametrSprawdzany < parametrMinimum)
            {
                return $"{nazwaParametru} < Minimum => {parametrSprawdzany - parametrMinimum}\n";
            }
            else if (parametrSprawdzany > parametrMaksimum)
            {
                return $"{nazwaParametru} > Maksimum => {parametrSprawdzany - parametrMaksimum}\n";
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task CzyMoznaPrzekwalifikowac(GeowlokninaParametryEnum parametr, decimal wartoscRzeczywistaParametru)
        {

            tblTowarGeowlokninaParametry nowyParametr = null;
            switch (parametr)
            {
                case GeowlokninaParametryEnum.Gramatura:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.MasaPowierzchniowa_Maksimum <= wartoscRzeczywistaParametru &&
                                                                                                                 p.MasaPowierzchniowa_Minimum >= wartoscRzeczywistaParametru);
                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.MasaPowierzchniowa;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.WytrzymaloscNaRozciaganie_MD:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.WytrzymaloscNaRozciaganie_MD <= wartoscRzeczywistaParametru &&
                                                                                                           p.WytrzymaloscNaRozciaganie_MD_Minimum >= wartoscRzeczywistaParametru);
                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.WytrzymaloscNaRozciaganie_MD;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;

                case GeowlokninaParametryEnum.WytrzymaloscNaRozciaganie_CMD:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.WytrzymaloscNaRozciaganie_CMD <= wartoscRzeczywistaParametru &&
                                                                                                           p.WytrzymaloscNaRozciaganie_CMD_Minimum >= wartoscRzeczywistaParametru);
                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.WytrzymaloscNaRozciaganie_CMD;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.WyduzeniePrzyZerwaniu_MD:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.WydluzeniePrzyZerwaniu_MD_Maksimum <= wartoscRzeczywistaParametru &&
                                                                                                           p.WydluzeniePrzyZerwaniu_MD_Minimum >= wartoscRzeczywistaParametru);
                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.WydluzeniePrzyZerwaniu_MD;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.WyduzeniePrzyZerwaniu_CMD:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.WydluzeniePrzyZerwaniu_CMD_Maksimum <= wartoscRzeczywistaParametru &&
                                                                                                           p.WydluzeniePrzyZerwaniu_CMD_Minimum >= wartoscRzeczywistaParametru);

                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.WytrzymaloscNaRozciaganie_CMD;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.OdpornoscNaPrzebicieStatyczne_CBR:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.OdpornoscNaPrzebicieStatyczne_CBR <= wartoscRzeczywistaParametru &&
                                                                                                           p.OdpornoscNaPrzebicieStatyczne_CBR_Minimum >= wartoscRzeczywistaParametru);
                    if (nowyParametr != null)
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.OdpornoscNaPrzebicieStatyczne_CBR;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.OdpornoscNaPrzebicieDynamiczne:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.OdpornoscNaPrzebicieDynamiczne_Maksimum <= wartoscRzeczywistaParametru &&
                                                                                                           p.OdpornoscNaPrzebicieDynamiczne >= wartoscRzeczywistaParametru);
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.OdpornoscNaPrzebicieDynamiczne;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.CharakterystycznaWielkoscPorow:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.CharakterystycznaWielkoscPorow_Maksimum <= wartoscRzeczywistaParametru &&
                                                                                                           p.CharakterystycznaWielkoscPorow_Minimum >= wartoscRzeczywistaParametru);
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.CharakterystycznaWielkoscPorow;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                case GeowlokninaParametryEnum.WodoprzepuszczalnoscProsotpadla:
                    nowyParametr = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync(p => p.WodoprzepuszczalnoscProsotpadla <= wartoscRzeczywistaParametru &&
                                                                                                           p.WodoprzepuszczalnoscProsotpadla_Minimum >= wartoscRzeczywistaParametru);
                    {
                        rezultat.MoznaPrzekwalifikowacNa = nowyParametr.WodoprzepuszczalnoscProsotpadla;
                        rezultat.CzyMoznaPrzekwalifikowac = true;
                    }
                    break;
                default:
                    break;
            }

        }

    }
}
