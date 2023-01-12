namespace Cringules.NGram.App.ViewModel;

public interface IDialogService
{
    public string OpenFilePath { get; set; }
    public bool ShowOpenFileDialog();
    public void ShowErrorMessage(string message);
}
