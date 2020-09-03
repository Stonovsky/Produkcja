using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public interface IMethodDetailViewModel : IDetailViewModel
    {
        Task LoadAsync(int? id);
        Task SaveAsync(int? id);
        Task DeleteAsync(int id);
    }
}
