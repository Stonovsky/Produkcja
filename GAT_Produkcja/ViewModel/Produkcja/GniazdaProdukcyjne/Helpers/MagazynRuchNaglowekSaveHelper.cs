using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Magazyn.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers
{
    public class MagazynRuchNaglowekSaveHelper : IMagazynRuchNaglowekSaveHelper
    {
        private readonly ITblRuchNaglowekHelper tblRuchNaglowekHelper;
        private readonly IUnitOfWork unitOfWork;
        private tblRuchNaglowek ruchNaglowek;

        public MagazynRuchNaglowekSaveHelper(ITblRuchNaglowekHelper tblRuchNaglowekHelper,
                                      IUnitOfWork unitOfWork)
        {
            this.tblRuchNaglowekHelper = tblRuchNaglowekHelper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<tblRuchNaglowek> ZapiszRekordDoTblRuchNaglowek(int idZlecenieProdukcyjne,
                                                        GniazdaProdukcyjneEnum gniazdoProdukcyjne,
                                                        StatusRuchuTowarowEnum statusRuchuTowarowEnum,
                                                        FirmaEnum firmaZ,
                                                        FirmaEnum firmaDo,
                                                        MagazynyEnum magazynZ,
                                                        MagazynyEnum magazynDo)
        {
            ruchNaglowek = await StworzRuchNaglowek(idZlecenieProdukcyjne, gniazdoProdukcyjne, statusRuchuTowarowEnum, firmaZ, firmaDo, magazynZ, magazynDo);
            unitOfWork.tblRuchNaglowek.Add(ruchNaglowek);
            await unitOfWork.SaveAsync();

            return ruchNaglowek;
        }

        private async Task<tblRuchNaglowek> StworzRuchNaglowek(int idZlecenieProdukcyjne,
                                                               GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum,
                                                               StatusRuchuTowarowEnum statusRuchuTowarowEnum,
                                                               FirmaEnum firmaZ,
                                                               FirmaEnum firmaDo,
                                                               MagazynyEnum magazynZ,
                                                               MagazynyEnum magazynDo)
        {
            var nrDokumentu = await tblRuchNaglowekHelper.NrDokumentuGenerator(StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW);

            return new tblRuchNaglowek
            {
                ID_PracownikGAT = UzytkownikZalogowany.Uzytkownik == null ? 7 : UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT,
                DataPrzyjecia = new DateTime(),
                IDFirmaZ = (int)firmaZ,
                IDFirmaDo = (int)firmaDo,
                IDKontrahent = null,
                IDMagazynZ = (int)magazynZ,
                IDMagazynDo = (int)magazynDo,
                IDProdukcjaGniazdaProdukcyjne = (int)gniazdaProdukcyjneEnum,
                IDRuchStatus = (int)statusRuchuTowarowEnum,
                IDProdukcjaZlecenieProdukcyjne = idZlecenieProdukcyjne,
                NrDokumentu = nrDokumentu.NrDokumentu,
                NrDokumentuPelny = nrDokumentu.PelnyNrDokumentu
            };
        }
    }
}
