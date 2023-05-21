namespace Cringules.NGram.App.ViewModel;

public interface IDialogService
{
    public string OpenFilePath { get; set; }
    public bool ShowOpenFileDialog(string? extension);
    
    public string SaveFilePath { get; set; }
    public bool ShowSaveFileDialog(string? extension);
        
    public void ShowErrorMessage(string message);

    public bool AskConfirmation(string message);
}
