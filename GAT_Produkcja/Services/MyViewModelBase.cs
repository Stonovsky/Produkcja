using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Services
{
    [AddINotifyPropertyChangedInterface]

    public abstract class MyViewModelBase : ViewModelBase
    {
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand ZamknijOknoCommand { get; set; }


        public MyViewModelBase()
        {
        }

    }
}
