using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Dictionaries
{
    public class TowarGeowlokninaGramaturaDictionary
    {
        Dictionary<int, int> gramaturaDictonary = new Dictionary<int, int>();

        public TowarGeowlokninaGramaturaDictionary()
        {
            gramaturaDictonary.Add(90, 3);
            gramaturaDictonary.Add(100, 4);
            gramaturaDictonary.Add(120, 5);
            gramaturaDictonary.Add(140, 6);
            gramaturaDictonary.Add(150, 7);
            gramaturaDictonary.Add(160, 8);
            gramaturaDictonary.Add(170, 9);
            gramaturaDictonary.Add(180, 10);
            gramaturaDictonary.Add(200, 11);
            gramaturaDictonary.Add(220, 12);
            gramaturaDictonary.Add(250, 13);
            gramaturaDictonary.Add(260, 14);
            gramaturaDictonary.Add(300, 15);
            gramaturaDictonary.Add(320, 16);
            gramaturaDictonary.Add(350, 17);
            gramaturaDictonary.Add(400, 18);
            gramaturaDictonary.Add(450, 19);
            gramaturaDictonary.Add(500, 20);
        }

        public int PobierzIdGramatury(int gramatura)
        {
            return gramaturaDictonary[gramatura];
        }
    }
}
