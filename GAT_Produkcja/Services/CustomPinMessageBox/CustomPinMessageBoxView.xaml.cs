using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GAT_Produkcja.Services.CustomPinMessageBox
{
    /// <summary>
    /// Logika interakcji dla klasy CustomPinMessageBoxView.xaml
    /// </summary>
    public partial class CustomPinMessageBoxView : Window
    {
        public CustomPinMessageBoxView()
        {
            InitializeComponent();
        }

        private void StackPanel_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //}
            TextBox s = e.Source as TextBox;
            if (s != null)
            {
                s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                //IInputElement focusedControl = FocusManager.GetFocusedElement(this);
                //TextBox nextPin = focusedControl as TextBox;
                //if (nextPin != null)
                //{
                //    nextPin.Text = null;
                //}
            }

            e.Handled = true;
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox s = e.Source as TextBox;
            if (!string.IsNullOrEmpty(s.Text))
            {
                var keyString = e.Key.ToString();

                if (keyString.StartsWith("D") && keyString.Length == 2)
                {
                    var newNumber = keyString[1];
                    s.Text = string.Empty;
                    s.Text = newNumber.ToString();
                }
            }
        }
    }
}
