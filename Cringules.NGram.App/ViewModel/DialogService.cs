using System.Windows;
using Microsoft.Win32;

namespace Cringules.NGram.App.ViewModel;

public class DialogService : IDialogService
{
    public string OpenFilePath { get; set; } = string.Empty;
    public bool ShowOpenFileDialog()
    {
        var dialog = new OpenFileDialog { FileName = OpenFilePath, DefaultExt = ".sp" };

        bool result = dialog.ShowDialog() ?? false;
        if (result)
        {
            OpenFilePath = dialog.FileName;
        }

        return result;
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
