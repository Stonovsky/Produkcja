using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GAT_Produkcja.UI.Utilities.WPFControls.DataGridControl
{
    public class EditCellOnSingleClick : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.LoadingRow += this.OnLoadingRow;
            this.AssociatedObject.UnloadingRow += this.OnUnloading;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.LoadingRow -= this.OnLoadingRow;
            this.AssociatedObject.UnloadingRow -= this.OnUnloading;
        }

        private void OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.GotFocus += this.OnGotFocus;
        }

        private void OnUnloading(object sender, DataGridRowEventArgs e)
        {
            e.Row.GotFocus -= this.OnGotFocus;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.BeginEdit(e);
        }
    }
}
