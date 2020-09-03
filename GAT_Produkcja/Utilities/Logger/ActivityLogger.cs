using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Logger
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly IUnitOfWork unitOfWork;

        public ActivityLogger(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public tblLog logEntity { get; set; }

        public async Task LogUserActivityAsync([CallerMemberName] string aktywnosc = "")
        {
            if (string.IsNullOrEmpty(aktywnosc))
                return;
            if (UzytkownikZalogowany.Uzytkownik == null ||
                UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT == 0)
                return;

            aktywnosc = aktywnosc.Replace("CommandExecute", "")
                                 .Replace("Execute", "")
                                 .Replace("Otworz", "");

            logEntity = new tblLog()
            {
                Data = DateTime.Now,
                ID_Log = Guid.NewGuid(),
                Aktywnosc = aktywnosc,
                Uzytkownik = UzytkownikZalogowany.Uzytkownik.ImieINazwiskoGAT
            };

            unitOfWork.tblLog.Add(logEntity);
            await unitOfWork.SaveAsync();
        }

    }
}
