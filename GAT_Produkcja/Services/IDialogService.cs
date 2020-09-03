using System.Windows;

namespace GAT_Produkcja.UI.Services
{
    public interface IDialogService
    {
        void ShowError_BtnOK(string message, string title = "Błąd");
        void ShowInfo_BtnOK(string message, string title = "Informacja");
        bool ShowQuestion_BoolResult(string message, string title = "Uwaga");
        MessageBoxResult ShowQuestion_BtnOK(string message, string title = "Uwaga");
        bool ShowQuestion_CustomButton_BoolResult(string message, string btnOkName, string btnCancelName, string title = "Uwaga");

    }
}