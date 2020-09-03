using BespokeFusion;
using GAT_Produkcja.UI.Services.CustomMessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAT_Produkcja.UI.Services
{
    public class DialogService : IDialogService
    {
        private MaterialMessageBoxCustomizedGTex msgBox;

        public DialogService()
        {
            msgBox = new MaterialMessageBoxCustomizedGTex();
        }

        public void ShowError_BtnOK(string message, string title = "Błąd")
        {
            msgBox.ShowError_BtnOK(message, title);
        }

        public void ShowInfo_BtnOK(string message, string title = "Informacja")
        {
            msgBox.ShowInfo_BtnOK(message, title);
        }

        public MessageBoxResult ShowQuestion_BtnOK(string message, string title = "Uwaga")
        {
            return msgBox.ShowQuestion_BtnOkCancel(message, title);
        }

        public bool ShowQuestion_BoolResult(string message, string title = "Uwaga")
        {
            return msgBox.ShowQuestionBoolResult(message, title);
        }

        public bool ShowQuestion_CustomButton_BoolResult(string message, string btnOkName, string btnCancelName, string title = "Uwaga")
        {
            return msgBox.ShowQuestionCustomButtonBoolResult(message, title, btnOkName, btnCancelName);
        }
    }
}
