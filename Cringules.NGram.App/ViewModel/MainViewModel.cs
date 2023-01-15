using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;

namespace Cringules.NGram.App.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private WorkSession? _session;

    private readonly IDialogService _dialogService = new DialogService();
    private readonly IFileDataSource _fileDataSource = new TextFileDataSource();

    [RelayCommand]
    private void OpenPlot()
    {
        if (_dialogService.ShowOpenFileDialog())
        {
            try
            {
                PlotData data = _fileDataSource.GetPlotData(_dialogService.OpenFilePath);
                Session = new WorkSession(data);
            }
            catch (FileFormatException e)
            {
                _dialogService.ShowErrorMessage(e.Message);
            }
        }
    }
}
