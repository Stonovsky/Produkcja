using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public interface IDetailViewModel
    {
        bool IsChanged { get; }
        bool IsValid { get; }
        void IsChanged_False();
    }
}
