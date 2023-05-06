using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    [ObservableProperty] private Xray _data;

    [ObservableProperty] private double _waveLength;

    [ObservableProperty] private bool _determineSmoothingDegree;
    [ObservableProperty] private int _minSmoothingDegree = 3;
    [ObservableProperty] private int _maxSmoothingDegree;

    [ObservableProperty] private int _smoothingDegree = 3;

    [ObservableProperty] private List<Point> _peakBoundaries = new();

    [ObservableProperty] private ObservableCollection<PeakData> _peaks = new();
    [ObservableProperty] private PeakData? _selectedPeak;

    [ObservableProperty] private bool _peakShown;

    [JsonIgnore] public DiffractogramPlotModel Model { get; } = new();
    [JsonIgnore] public PlotController PlotController { get; } = new();

    [JsonIgnore] public PeakPlotModel PeakModel { get; } = new();

    public WorkSession(PlotData data)
    {
        Data = data.ToXray();
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
    }

    [RelayCommand]
    private void StartAnalysis()
    {
        Xray smoothed = Data.SmoothXray(SmoothingDegree);
        Model.SmoothedPoints = smoothed.ToPlotPoints();

        PeakBoundaries = smoothed.GetPeakBoundaries();
        Model.PeakBoundaries = PeakBoundaries;

        var xrayPeaks = new List<XrayPeak>();
        for (var i = 0; i < PeakBoundaries.Count - 1; i++)
        {
            xrayPeaks.Add(smoothed.GetPeak(PeakBoundaries[i].X, PeakBoundaries[i + 1].X));
        }

        Peaks = new ObservableCollection<PeakData>(xrayPeaks.Select(peak => new PeakData(peak)));
    }

    [RelayCommand]
    private void SelectPeak()
    {
        Model.CanSelect = true;
    }

    [RelayCommand]
    private void CancelSelection()
    {
        Model.CanSelect = false;
    }

    [RelayCommand]
    private void AddPeak()
    {
        List<double> points = Model.SelectedBoundary.ToList();
        points.Sort();
        XrayPeak peak = Data.GetPeak(points[0], points[1]);
        Peaks.Add(new PeakData(peak));

        Model.CanSelect = false;
    }

    partial void OnDataChanged(Xray value)
    {
        Model.PlotPoints = value.ToPlotPoints();
        MaxSmoothingDegree = value.Points.Count;
    }

    partial void OnSelectedPeakChanged(PeakData? value)
    {
        if (value == null)
        {
            PeakShown = false;
        }

        PeakModel.SelectedPeak = value;
    }
}
