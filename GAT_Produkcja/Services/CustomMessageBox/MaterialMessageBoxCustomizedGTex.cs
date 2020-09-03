
using BespokeFusion;
using System;
using System.Windows;
using System.Windows.Media;

namespace GAT_Produkcja.UI.Services.CustomMessageBox
{
    public class MaterialMessageBoxCustomizedGTex
    {

        private CustomMaterialMessageBox CreateMessageBox(string wiadomosc,
                                                          string tytul = "Uwaga",
                                                          string btnOk = "Tak",
                                                          string btnCancel = "Nie",
                                                          Visibility visibility = Visibility.Visible)
        {
            CustomMaterialMessageBox msgBox;
            Brush titleBackgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3f51b5"));
            Brush buttonBackgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3f51b5"));

            msgBox = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = wiadomosc },
                TxtTitle = { Text = tytul },
                
                TitleBackgroundPanel = { Background = titleBackgroundColor },
                BorderBrush = Brushes.DarkGray,
                
                BtnOk = { Content = btnOk, Background = buttonBackgroundColor, BorderBrush = Brushes.Gray },
                BtnCancel = { Content = btnCancel, Background = buttonBackgroundColor, BorderBrush = Brushes.Gray, Visibility = visibility },
                
                Background = Brushes.LightGray,
                Foreground = Brushes.White,
            };

            return msgBox;
        }

        public MessageBoxResult ShowQuestion_BtnOkCancel(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = CreateMessageBox(wiadomosc, tytul, btnOk, btnCancel);

            msg.Show();
            return msg.Result;
        }

        public bool ShowQuestionBoolResult(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = CreateMessageBox(wiadomosc, tytul, btnOk, btnCancel);

            msg.Show();
            if (msg.Result == MessageBoxResult.OK)
                return true;
            else
                return false;
        }
        public bool ShowQuestionCustomButtonBoolResult(string wiadomosc, string tytul = "Uwaga", string btnOk = "Tak", string btnCancel = "Nie")
        {
            var msg = CreateMessageBox(wiadomosc, tytul, btnOk, btnCancel);

            msg.Show();
            if (msg.Result == MessageBoxResult.OK)
                return true;
            else
                return false;
        }


        public void ShowInfo_BtnOK(string wiadomosc, string tytul = "Informacja", string btnOk = "OK", string btnCancel = "Nie")
        {
            var msg = CreateMessageBox(wiadomosc, tytul, btnOk, btnCancel, Visibility.Collapsed);
            msg.Show();
        }

        public void ShowError_BtnOK(string wiadomosc, string tytul = "Błąd", string btnOk = "OK", string btnCancel = "Nie")
        {
            var msg = CreateMessageBox(wiadomosc, tytul, btnOk, btnCancel, Visibility.Collapsed);
            msg.Show();
        }
    }
}

