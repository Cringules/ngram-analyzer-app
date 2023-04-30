using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.View;
using Cringules.NGram.App.ViewModel;
using Cringules.NGram.Lib;
using OxyPlot;

namespace Cringules.NGram.App.Model;

public partial class WorkSession : ObservableObject
{
    [ObservableProperty] private PlotData _data = null!;

    [ObservableProperty] private List<Peak> _peaks = new();

    [ObservableProperty] private double _waveLength;

    [ObservableProperty] private bool _determineNoiseLevel;

    [ObservableProperty] private int _noiseLevel;

    [JsonIgnore] public DiffractogramPlotModel Model { get; } = new();
    [JsonIgnore] public PlotController PlotController { get; } = new();

    public WorkSession(PlotData data)
    {
        Data = data;
        PlotController.BindMouseDown(OxyMouseButton.Left,
            new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) =>
                controller.AddMouseManipulator(view, new PlotSelectionManipulator(view, Model), args)));
    }

    [RelayCommand]
    private void GetWaveLength()
    {
        var viewModel = new WaveLengthViewModel() {SelectedValue = WaveLength};
        var window = new WaveLengthWindow() {DataContext = viewModel};
        if ((window.ShowDialog() ?? false) && viewModel.SelectedValue.HasValue)
        {
            WaveLength = viewModel.SelectedValue.Value;
        }
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

    [RelayCommand]
    private void StartAnalysis()
    {
    }

    partial void OnDataChanged(PlotData? value)
    {
        Model.Update(value?.Points);
        Peaks = PeakFinder.FindPeaks(value);
    }
}
