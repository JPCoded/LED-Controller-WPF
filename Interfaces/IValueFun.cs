using System.Windows.Input;

namespace WPF_LED_Controller
{
    public interface IValueFun
    {
        string OverUnderValidation(string valueToCheck,int max = 255);
        void KeyPreview(object sender, KeyEventArgs e, int max = 255);
        void HexKeyValidation(KeyEventArgs e);
    }
}
