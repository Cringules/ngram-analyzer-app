using System.Windows;
using Cringules.NGram.App.Resources;
using Microsoft.Win32;

namespace Cringules.NGram.App.ViewModel;

public class DialogService : IDialogService
{
    public string OpenFilePath { get; set; } = string.Empty;

    public bool ShowOpenFileDialog(string? extension)
    {
        var dialog = new OpenFileDialog {FileName = OpenFilePath};
        if (extension != null)
        {
            dialog.Filter = $@"*{extension}|*{extension}";
        }

        bool result = dialog.ShowDialog() ?? false;
        if (result)
        {
            OpenFilePath = dialog.FileName;
        }

        return result;
    }

    public string SaveFilePath { get; set; } = string.Empty;

    public bool ShowSaveFileDialog(string? extension)
    {
        var dialog = new SaveFileDialog {FileName = SaveFilePath};
        if (extension != null)
        {
            dialog.Filter = $@"*{extension}|*{extension}";
        }

        bool result = dialog.ShowDialog() ?? false;
        if (result)
        {
            SaveFilePath = dialog.FileName;
        }

        return result;
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public bool AskConfirmation(string message)
    {
        MessageBoxResult result = MessageBox.Show(message, "Confirm", MessageBoxButton.OKCancel,
            MessageBoxImage.Asterisk,
            MessageBoxResult.Cancel);

        return result == MessageBoxResult.OK;
    }
}
