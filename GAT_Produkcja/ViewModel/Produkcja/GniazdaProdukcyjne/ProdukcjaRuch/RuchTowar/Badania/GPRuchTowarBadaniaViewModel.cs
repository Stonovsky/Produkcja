using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Badania
{
    public class GPRuchTowarBadaniaViewModel : SaveDeleteMethodGenericViewModelBase<tblProdukcjaRuchTowarBadania>, IGPRuchTowarBadaniaViewModel
    {
        #region Fields

        private int gramatura1;
        private int gramatura2;
        private int gramatura3;
        private tblTowarGeowlokninaParametryGramatura gramaturaPrzeslana;
        private readonly IWeryfikacjaGramaturyGeowlokninHelper weryfikacjaGramaturyHelper;

        #endregion

        #region Properties
        public int Gramatura1
        {
            get => gramatura1;
            set
            {
                gramatura1 = value;
                VMEntity.Gramatura_1 = value;
                PrzeliczGramatureSrednia();
            }
        }

        public int Gramatura2
        {
            get => gramatura2;
            set
            {
                gramatura2 = value;
                VMEntity.Gramatura_2 = value;
                PrzeliczGramatureSrednia();
            }
        }

        public int Gramatura3
        {
            get => gramatura3;
            set
            {
                gramatura3 = value;
                VMEntity.Gramatura_3 = value;
                PrzeliczGramatureSrednia();
            }
        }
        #endregion

        #region CTOR
        public GPRuchTowarBadaniaViewModel(IViewModelService viewModelService,
                                           IWeryfikacjaGramaturyGeowlokninHelper weryfikacjaGramaturyHelper)
            : base(viewModelService)
        {
            VMEntity = new tblProdukcjaRuchTowarBadania();
            this.weryfikacjaGramaturyHelper = weryfikacjaGramaturyHelper;
        }
        #endregion

        #region MessengerRegistration
        protected override void MessengerRegistration()
        {
            //Gdy przeslano gramature z karty Rejestracja z GPRTDodajVM
            Messenger.Register<tblTowarGeowlokninaParametryGramatura>(this, GdyPrzeslanoGramature);
        }

        private void GdyPrzeslanoGramature(tblTowarGeowlokninaParametryGramatura obj)
        {
            gramaturaPrzeslana = obj;
        }
        #endregion

        public override IGenericRepository<tblProdukcjaRuchTowarBadania> Repository => UnitOfWork.tblProdukcjaRuchTowarBadania;

        protected override Func<int> GetVMEntityId => () => VMEntity.IDProdukcjaRuchTowarBadania;

        #region LoadAsync

        protected override Func<int?, Task> LoadEntitiesAsync => async (id) =>
        {
            await weryfikacjaGramaturyHelper.LoadAsync();
             
            if (id is null || id == 0) return;
            VMEntity = await Repository.SingleOrDefaultAsync(e => e.IDProdukcjaRuchTowar == id);
        };
        #endregion

        #region SaveAsync
        public tblProdukcjaRuchTowarBadania Save(int? idTblProdukcjaRuchTowar)
        {
            if (idTblProdukcjaRuchTowar is null) return null;
           
            VMEntity.IDProdukcjaRuchTowar = idTblProdukcjaRuchTowar.Value;
            return VMEntity;

        }
        #endregion

        #region Gramatura srednia
        private void PrzeliczGramatureSrednia()
        {
            if (VMEntity.Gramatura_1 == 0 && VMEntity.Gramatura_2 == 0 && VMEntity.Gramatura_3 == 0) return;

            List<int> listaGramatur = new List<int>
            {
                VMEntity.Gramatura_1,
                VMEntity.Gramatura_2,
                VMEntity.Gramatura_3,
            };

            var sumaGramatur = listaGramatur.Sum();
            var iloscGramatur = listaGramatur.Where(e => e > 0).Count();

            if (iloscGramatur == 0) return;

            VMEntity.GramaturaSrednia = sumaGramatur / iloscGramatur;

            SprawdzCzyGramaturaZgodna(VMEntity.GramaturaSrednia);
        }

        private void SprawdzCzyGramaturaZgodna(decimal gramaturaSrednia)
        {
            if (gramaturaSrednia == 0) return;
            if (gramaturaPrzeslana is null || gramaturaPrzeslana.IDTowarGeowlokninaParametryGramatura==0) return;

            VMEntity.CzySredniaGramaturaWTolerancjach = weryfikacjaGramaturyHelper.CzyGramaturaZgodna(gramaturaSrednia, gramaturaPrzeslana.IDTowarGeowlokninaParametryGramatura);
        }
        #endregion
    }
}
