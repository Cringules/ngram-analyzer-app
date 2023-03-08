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

    public string SaveFilePath { get; set; } = string.Empty;

    public bool ShowSaveFileDialog()
    {
        var dialog = new SaveFileDialog { FileName = SaveFilePath, DefaultExt = ".ngram" };

        bool result = dialog.ShowDialog() ?? false;
        if (result)
        {
            SaveFilePath = dialog.FileName;
        }

        return result;
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
