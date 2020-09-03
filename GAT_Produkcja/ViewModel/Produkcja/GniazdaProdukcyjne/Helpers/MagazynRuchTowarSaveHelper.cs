using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Magazyn.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers
{
    public class MagazynRuchTowarSaveHelper : IMagazynRuchTowarSaveHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITblRuchTowarHelper tblRuchTowarHelper;
        private tblRuchTowar ruchTowar;

        public MagazynRuchTowarSaveHelper(IUnitOfWork unitOfWork,
                                   ITblRuchTowarHelper tblRuchTowarHelper)
        {
            this.unitOfWork = unitOfWork;
            this.tblRuchTowarHelper = tblRuchTowarHelper;
        }

        public async Task<tblRuchTowar> ZapiszRekordDoTblRuchTowaru(JmEnum jmEnum,
                                                                     StatusRuchuTowarowEnum statusRuchuTowarowEnum,
                                                                     DokumentTypEnum dokumentTypEnum,
                                                                     MagazynyEnum naMagazynEnum,
                                                                     VatEnum vatEnum,
                                                                     int idTowar,
                                                                     int idRuchNaglowek,
                                                                     string nrRolki,
                                                                     string nrPartii)
        {

            ruchTowar = StworzRuchTowaru(jmEnum,
                                         dokumentTypEnum,
                                         naMagazynEnum,
                                         vatEnum,
                                         idTowar,
                                         idRuchNaglowek,
                                         nrRolki,
                                         nrPartii);


            var ruchStatus = await unitOfWork.tblRuchStatus.GetByIdAsync((int)statusRuchuTowarowEnum);
            var ruchNaglowek = await unitOfWork.tblRuchNaglowek.GetByIdAsync(idRuchNaglowek);

            await tblRuchTowarHelper.DodajDoBazyDanych(ruchTowar, ruchStatus, ruchNaglowek);

            return ruchTowar;
        }

        private tblRuchTowar StworzRuchTowaru(JmEnum jmEnum,
                                                DokumentTypEnum dokumentTypEnum,
                                                MagazynyEnum naMagazynEnum,
                                                VatEnum vatEnum,
                                                int idTowar,
                                                int idRuchNaglowek,
                                                string nrRolki,
                                                string nrPartii)
        {
            return new tblRuchTowar
            {
                IDJm = (int)jmEnum,
                IDDokumentTyp = (int)dokumentTypEnum,
                IDMagazyn = (int)naMagazynEnum,
                IDVat = (int)vatEnum,
                IDTowar = idTowar,
                IDRuchNaglowek = idRuchNaglowek,
                NrRolki = nrRolki,
                NrParti = nrPartii,


            };
        }



    }
}
