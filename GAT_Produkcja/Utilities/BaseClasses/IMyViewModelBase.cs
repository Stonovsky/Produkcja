using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public interface IMyViewModelBase
    {
        bool IsValid { get; set; }
        bool IsChanged { get; set; }
        Task LoadAsync(int? id);
    }
}
