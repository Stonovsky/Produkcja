using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BarCodeGenerator
{
    public static class BarCodeGenerator
    {
        //static private int _InternalCounter = 0;

        static public string GetUniqueId()
        {
            var now = DateTime.Now;

            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;
            var miliseconds = (int)DateTime.Now.Millisecond % 100 ;

            //var counter = _InternalCounter++ % 100;

            return days.ToString("00000") + seconds.ToString("00000") + miliseconds.ToString("00");
        }
    }
}