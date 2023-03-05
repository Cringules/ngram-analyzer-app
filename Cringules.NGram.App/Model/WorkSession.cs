using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.ViewModel;
using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model;

public partial class WorkSession : ObservableObject
{
    [ObservableProperty] private PlotData _data = null!;

    [ObservableProperty] private List<Peak> _peaks = new();

    public DiffractogramPlotModel Model { get; } = new();

    public WorkSession(PlotData data)
    {
        Data = data;
    }

    [RelayCommand]
    private void CleanUpData()
    {
        Data = PlotCleaner.GetCleanedPlot(Data);
    }

    [RelayCommand]
    private void CalculateDataPeaks()
    {
        
    }

    partial void OnDataChanged(PlotData? value)
    {
        Model.Update(value?.Points);
        Peaks = PeakFinder.FindPeaks(value);
    }
}
