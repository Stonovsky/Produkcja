using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService
{
    public interface IScaleReader
    {
        Task<decimal> GetWeight();
        Task<string> GetWeightInString();
        Task LoadAsync();

    }
}
