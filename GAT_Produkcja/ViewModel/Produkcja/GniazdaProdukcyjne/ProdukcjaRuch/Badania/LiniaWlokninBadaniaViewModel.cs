using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Badania
{
    [AddINotifyPropertyChangedInterface]

    public class LiniaWlokninBadaniaViewModel : SaveDeleteMethodViewModelBase, ILiniaWlokninBadaniaViewModel
    {
        private readonly IWeryfikacjaTolerancji weryfikacjaTolerancji;

        public tblProdukcjaGniazdoWlokninaBadania Badanie { get; set; } = new tblProdukcjaGniazdoWlokninaBadania();
        public tblProdukcjaGniazdoWlokninaBadania BadanieOrg { get; set; }


        public int? GramaturaProbkaLewa
        {
            get { return Badanie?.Gramatura_1; }
            set
            {
                Badanie.Gramatura_1 = value;
                ObliczGramatureSrednia();
            }
        }


        public int? GramaturaProbkaSrodek
        {
            get { return Badanie?.Gramatura_2; }
            set
            {
                Badanie.Gramatura_2 = value;
                ObliczGramatureSrednia();
            }
        }


        public int? GramaturaProbkaPrawa
        {
            get { return Badanie?.Gramatura_3; }
            set
            {
                Badanie.Gramatura_3 = value;
                ObliczGramatureSrednia();
                
            }
        }
        private async void ObliczGramatureSrednia()
        {
            if (CzyGramaturyWypelnione())
            {
                Badanie.GramaturaSrednia = (decimal)((Badanie.Gramatura_1.GetValueOrDefault()
                                + Badanie.Gramatura_2.GetValueOrDefault()
                                + Badanie.Gramatura_3.GetValueOrDefault()) / 3);

                await CzySredniaGramaturaWTolerancjach();
            }
        }


        public decimal GramaturaSrednia
        {
            get { return Badanie.GramaturaSrednia; }
            set { Badanie.GramaturaSrednia = value; }
        }

        public override bool IsChanged
        {
            get { return !Badanie.Compare(BadanieOrg); }
        }
        public override bool IsValid { get { return Badanie.IsValid; } }

        public tblProdukcjaZlecenie AktywneZlecenieProdukcyjne { get; set; }

        private bool CzyGramaturyWypelnione()
        {
            if (Badanie.Gramatura_1 == null
                || Badanie.Gramatura_2 == null
                || Badanie.Gramatura_3 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region CTOR
        public LiniaWlokninBadaniaViewModel(IViewModelService viewModelService,
                                            IWeryfikacjaTolerancji weryfikacjaTolerancji)
                                            : base(viewModelService)
        {
            this.weryfikacjaTolerancji = weryfikacjaTolerancji;

            Badanie = new tblProdukcjaGniazdoWlokninaBadania();
            BadanieOrg = Badanie.DeepClone();

            Messenger.Register<tblProdukcjaZlecenie>(this, GdyPrzeslanoAktywneZlecenieProdykcyjne);
        }

        private void GdyPrzeslanoAktywneZlecenieProdykcyjne(tblProdukcjaZlecenie obj)
        {
            AktywneZlecenieProdukcyjne = obj;
        }


        #endregion

        public override async Task SaveAsync(int? idProdukcjaGniazdoWloknin)
        {
            if (idProdukcjaGniazdoWloknin != null)
            {
                //Badanie.IDProdukcjaGniazdoWloknina = idProdukcjaGniazdoWloknin.Value;
                UnitOfWork.tblProdukcjaGniazdoWlokninaBadania.Add(Badanie);
                await UnitOfWork.SaveAsync();
                IsChanged_False();
            }
        }

        public override async Task DeleteAsync(int id)
        {
            UnitOfWork.tblProdukcjaGniazdoWlokninaBadania.Remove(Badanie);
            await UnitOfWork.SaveAsync();
        }

        public override async Task LoadAsync(int? id)
        {
            if (id.HasValue && id!=0)
            {
                Badanie = await UnitOfWork.tblProdukcjaGniazdoWlokninaBadania.SingleOrDefaultAsync(b => b.IDProdukcjaGniazdoWlokninaBadania == id);
                IsChanged_False();
            }
        }

        public override void IsChanged_False()
        {
            BadanieOrg = Badanie.DeepClone();
        }

        private async Task CzySredniaGramaturaWTolerancjach()
        {
            //if (!CzyGramaturyWypelnione())
            //    return;

            //if (AktywneZlecenieProdukcyjne == null
            //    || AktywneZlecenieProdukcyjne.IDTowar==null)
            //    return;

            //var result = await weryfikacjaTolerancji.CzyParametrZgodny(AktywneZlecenieProdukcyjne.IDTowar.Value,
            //                                                           GeowlokninaParametryEnum.Gramatura,
            //                                                           (int)Badanie.GramaturaSrednia);

            //if (result == null)
            //    return;

            //Badanie.CzySrenidaGramaturaWTolerancjach = result.CzyParametrZgodnyZTolerancja;
            //Badanie.Uwagi = result.Uwagi;
        }
    }
}
