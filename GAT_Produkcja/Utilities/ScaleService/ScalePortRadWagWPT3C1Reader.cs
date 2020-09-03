using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService
{
    public class ScalePortRadWagWPT3C1Reader : ScaleReaderBase, IScalePortRadWagWPT3C1Reader
    {
        protected override string ComPortName => "COM4";

        protected override string MessageToScale => "SI\r\n";

        public Task LoadAsync()
        {
            throw new NotImplementedException();
        }

        protected override string MessageParser(string scaleMessage)
        {
            var weight = scaleMessage.Substring(7, 9);
            weight = weight.Replace(" ", "");
            return weight;
        }
    }
}
