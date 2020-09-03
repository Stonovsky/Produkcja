using System.Windows;

namespace GAT_Produkcja.UI.ViewModel.Login
{
    /// <summary>
    /// Logika interakcji dla klasy LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            tbxKodKreskowy.Focus();
        }
    }
}
