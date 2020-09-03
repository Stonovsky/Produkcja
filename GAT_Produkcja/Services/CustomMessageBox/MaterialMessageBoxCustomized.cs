
using BespokeFusion;
using System;
using System.Windows;
using System.Windows.Media;

namespace GAT_Produkcja.UI.Services.CustomMessageBox
{
    public class MaterialMessageBoxCustomized
    {

        public MessageBoxResult ShowQuestion_BtnOkCancel(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                BtnOk = { Content = btnOk, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                BtnCancel = { Content = btnCancel, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                TitleBackgroundPanel = { Background = Brushes.OrangeRed },
                BorderBrush = Brushes.OrangeRed
            };
            msg.Show();
            return msg.Result;
        }

        public bool ShowQuestionBoolResult(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                BtnOk = { Content = btnOk, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                BtnCancel = { Content = btnCancel, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                TitleBackgroundPanel = { Background = Brushes.OrangeRed },
                BorderBrush = Brushes.OrangeRed
            };
            msg.Show();
            if (msg.Result == MessageBoxResult.OK)
                return true;
            else
                return false;
        }
        public bool ShowQuestionCustomButtonBoolResult(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                BtnOk = { Content = btnOk, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                BtnCancel = { Content = btnCancel, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                TitleBackgroundPanel = { Background = Brushes.OrangeRed },
                BorderBrush = Brushes.OrangeRed
            };
            msg.Show();
            if (msg.Result == MessageBoxResult.OK)
                return true;
            else
                return false;
        }


        public void ShowInfo_BtnOK(string wiadomosc, string tytul = "Informacja", string btnOk = "OK")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                BtnOk = { Content = btnOk, Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                BtnCancel = { Visibility = System.Windows.Visibility.Collapsed },
                TitleBackgroundPanel = { Background = Brushes.OrangeRed },
                BorderBrush = Brushes.OrangeRed
            };
            msg.Show();
        }

        public void ShowError_BtnOK(string wiadomosc, string tytul = "Błąd")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                BtnOk = { Content = "OK", Background = Brushes.Gray, BorderBrush = Brushes.Gray },
                BtnCancel = { Visibility = System.Windows.Visibility.Collapsed },
                TitleBackgroundPanel = { Background = Brushes.Red },
                BorderBrush = Brushes.Red
            };
            msg.Show();
        }
    }
}
