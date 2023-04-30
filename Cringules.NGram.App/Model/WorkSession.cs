using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Api;
using Cringules.NGram.App.ViewModel;
using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model;

public partial class WorkSession : ObservableObject
{
    [ObservableProperty] private PlotData _data = null!;

    [ObservableProperty] private double _waveLength;

    [ObservableProperty] private bool _determineNoiseLevel;

    [ObservableProperty] private int _noiseLevel;

    [ObservableProperty] private List<Point> _peakBoundaries = new();

    [ObservableProperty] private List<XrayPeak> _xrayPeaks = new();

    [ObservableProperty] private List<Peak> _peaks = new();

    [JsonIgnore] public DiffractogramPlotModel Model { get; } = new();

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
    private void StartAnalysis()
    {
        var xray = new Xray(Data.Points.Select(point => new Point(point.Angle, point.Intensity)));
        PeakBoundaries = xray.GetPeakBoundaries();

        XrayPeaks.Clear();
        for (var i = 0; i < PeakBoundaries.Count - 1; i++)
        {
            XrayPeaks.Add(xray.GetPeak(PeakBoundaries[i].X, PeakBoundaries[i + 1].X));
        }

        Peaks = XrayPeaks.Select(peak =>
        {
            var analyzer = new XrayPeakAnalyzer(peak);
            return new Peak(analyzer.GetTopAngle(), 0, analyzer.GetIntensityMax(), analyzer.GetIntensityIntegral());
        }).ToList();
    }

    partial void OnDataChanged(PlotData? value)
    {
        Model.Update(value?.Points);
        // Peaks = PeakFinder.FindPeaks(value);
    }
}
