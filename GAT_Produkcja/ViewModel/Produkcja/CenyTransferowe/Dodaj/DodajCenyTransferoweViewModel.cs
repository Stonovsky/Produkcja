using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Dodaj
{
    public class DodajCenyTransferoweViewModel : SaveCommandGenericViewModelBase
    {
        #region Fields
        private readonly IXlsService xlsService;
        #endregion

        #region Properties
        public List<tblProdukcjaRozliczenie_CenyTransferowe> ListaCenTransferowych { get; set; } = new List<tblProdukcjaRozliczenie_CenyTransferowe>();
        public List<tblProdukcjaRozliczenie_CenyTransferowe> ListaZmienionychCen { get; set; } = new List<tblProdukcjaRozliczenie_CenyTransferowe>();
        public tblProdukcjaRozliczenie_CenyTransferowe WybranaZmienionaCena { get; set; }
        public IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> ListaZmienionychCenOrg { get; set; }
        public DateTime DataDodania { get; set; } = DateTime.Now.Date;
        public int SumaIlosciPozycji { get; set; }
        #endregion


        #region Commands
        public RelayCommand DodajCenyTransferoweZPlikuXls { get; set; }
        public RelayCommand ZmienDateCommand { get; set; }
        public override bool IsChanged => !ListaZmienionychCenOrg.Compare(ListaZmienionychCen);

        public override bool IsValid => throw new NotImplementedException();
        #endregion

        #region CTOR
        public DodajCenyTransferoweViewModel(IViewModelService viewModelService,
                                             IXlsService xlsService)
            : base(viewModelService)
        {
            this.xlsService = xlsService;

            DodajCenyTransferoweZPlikuXls = new RelayCommand(DodajCenyTransferoweZPlikuXlsExecute);
            ZmienDateCommand = new RelayCommand(ZmienDateCommandExecute, ZmienDateCommandCanExecute);

            ListaZmienionychCenOrg = ListaZmienionychCen.DeepClone();
        }

        private void ZmienDateCommandExecute()
        {
            ListaZmienionychCen.ToList().ForEach(c => c.DataDodania = DataDodania);
        }

        private bool ZmienDateCommandCanExecute()
        {
            if (ListaZmienionychCen is null) return false;
            if (!ListaZmienionychCen.Any()) return false;

            return ListaZmienionychCen.First().DataDodania != DataDodania;
        }
        #endregion

        #region Commands
        /// <summary>
        /// Metoda generujaca <see cref="ListaCenTransferowych"/> z pliku excel
        /// </summary>
        private void DodajCenyTransferoweZPlikuXlsExecute()
        {
            var ListaNowychCenTransferowych = xlsService.GetTranferPrices("CenyTransferowe");

            ListaZmienionychCen = new List<tblProdukcjaRozliczenie_CenyTransferowe>(PobierzListeZmienionychCen(ListaNowychCenTransferowych));

            ZmienStareCenyNaNieaktualne(ListaZmienionychCen);
            UzupelnijNoweCeny(ListaZmienionychCen);
            Podsumuj();
        }

        private void Podsumuj()
        {
            SumaIlosciPozycji = ListaZmienionychCen.Count();
        }

        /// <summary>
        /// Generuje liste obiektow, ktorych ceny roznia sie od cen w bazie
        /// </summary>
        /// <param name="listaNowychCenTransferowych"></param>
        /// <returns></returns>
        private IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> PobierzListeZmienionychCen(IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaNowychCenTransferowych)
        {
            foreach (var nowaCena in listaNowychCenTransferowych)
            {
                if (CzyCenyRozne(nowaCena))
                    yield return nowaCena;
            }
        }

        /// <summary>
        /// Uzupelnia ceny o date dodania oraz flage CzyAktualna
        /// </summary>
        /// <param name="listaZmienionychCen"></param>
        private void UzupelnijNoweCeny(IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaZmienionychCen)
        {
            listaZmienionychCen.ToList()
                               .ForEach(c =>
                               {
                                   c.CzyAktualna = true;
                                   c.DataDodania = DateTime.Now.Date;
                                   c.IDTowarGrupa = PobierzGrupe(c);
                               });
        }

        private int PobierzGrupe(tblProdukcjaRozliczenie_CenyTransferowe c)
        {
            if (c.TowarNazwa.ToUpper().Contains("ALTEX AT"))
                return (int)TowarGrupaEnum.Geowlokniny;
            if (c.TowarNazwa.ToUpper().Contains("AT CELL"))
                return (int)TowarGrupaEnum.Geokomorki;
            if (c.TowarNazwa.ToUpper().Contains("AT BORD"))
                return (int)TowarGrupaEnum.Obrzeze;

            return -1;
        }

        /// <summary>
        /// Zmienia ceny bedace w bazie na nieaktualne dla tych cen, ktore maja nowe odpowiedniki
        /// </summary>
        /// <param name="listaZmienionychCen"></param>
        private void ZmienStareCenyNaNieaktualne(IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaZmienionychCen)
        {
            var listaCenZmienionychWCenachTransferowych = ListaCenTransferowych.Where(c => listaZmienionychCen.Any(z => z.TowarNazwa == c.TowarNazwa));
            listaCenZmienionychWCenachTransferowych.ToList().ForEach(c => c.CzyAktualna = false);
        }

        #endregion

        /// <summary>
        /// Sprawdza czy ceny sa rozne, gdy tak jest zwraca TRUE
        /// </summary>
        /// <param name="nowaCena"></param>
        /// <returns></returns>
        private bool CzyCenyRozne(tblProdukcjaRozliczenie_CenyTransferowe nowaCena)
        {
            var staraCena = ListaCenTransferowych.SingleOrDefault(s => s.TowarNazwa == nowaCena.TowarNazwa);
            if (staraCena is null) return true;

            return staraCena.CenaTransferowa != nowaCena.CenaTransferowa
                || staraCena.CenaHurtowa != nowaCena.CenaHurtowa;
        }

        protected override async void LoadCommandExecute()
        {
            ListaCenTransferowych = new List<tblProdukcjaRozliczenie_CenyTransferowe>(
                await UnitOfWork.tblProdukcjaRozliczenie_CenyTransferowe.WhereAsync(c=>c.CzyAktualna==true));
        }

        protected override void DeleteCommandExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteCommandCanExecute()
        {

            throw new NotImplementedException();
        }

        protected override async void SaveCommandExecute()
        {
            var listaNowychCen = ListaZmienionychCen.Where(c => c.IDProdukcjaRozliczenie_CenyTransferowe == 0);

            if (listaNowychCen.Any())
                UnitOfWork.tblProdukcjaRozliczenie_CenyTransferowe.AddRange(listaNowychCen);

            await UnitOfWork.SaveAsync();

            DialogService.ShowInfo_BtnOK("Ceny zostały zapisane w bazie danych");
            IsChanged_False();
            Messenger.Send(new ZmianaCenTrasferowychMessage());
            ViewService.Close(this.GetType().Name);
        }

        protected override bool SaveCommandCanExecute()
        {
            if (ListaZmienionychCen is null)
                return false;

            if (!ListaZmienionychCen.Any())
                return false;
            if (!IsChanged)
                return false;

            return true;
        }

        public override void IsChanged_False()
        {
            ListaZmienionychCenOrg = ListaZmienionychCen.DeepClone();
        }
    }
}
