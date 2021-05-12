using System.Windows.Controls;
using System.Windows.Input;

namespace Clicker.Controls
{
    public class NumericInput : TextBox
    {
        public NumericInput()
        {
            PreviewTextInput += PreviewTextInputEventHandler;
            PreviewKeyDown += PreviewKeyDownEventHandler;
        }

        private void PreviewTextInputEventHandler(object sender, TextCompositionEventArgs e) =>
            e.Handled = !int.TryParse(e.Text, out _);
        private void PreviewKeyDownEventHandler(object sender, KeyEventArgs e) => e.Handled = e.Key == Key.Space;
    }
}