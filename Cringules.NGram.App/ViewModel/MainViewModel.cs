using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private PlotData? _data;

    public DiffractogramPlotModel Model { get; }

    private readonly IDialogService _dialogService = new DialogService();
    private readonly IFileDataSource _fileDataSource = new TextFileDataSource();

    public MainViewModel()
    {
        Model = new DiffractogramPlotModel();
    }

    [RelayCommand]
    private void OpenPlot()
    {
        if (_dialogService.ShowOpenFileDialog())
        {
            try
            {
                Data = _fileDataSource.GetPlotData(_dialogService.OpenFilePath);
            }
            catch (FileFormatException e)
            {
                _dialogService.ShowErrorMessage(e.Message);
            }
        }
    }

    [RelayCommand]
    private void UpdatePoints()
    {
        if (Data != null)
        {
            Data = PlotCleaner.GetCleanedPlot(Data);
        }
    }

    partial void OnDataChanged(PlotData? value)
    {
        Model.Update(value?.Points);
    }
}
