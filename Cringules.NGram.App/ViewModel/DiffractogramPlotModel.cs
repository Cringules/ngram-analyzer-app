using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;
using Cringules.NGram.Lib;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a diffractogram plot model.
/// </summary>
[ObservableObject]
public partial class DiffractogramPlotModel : PlotModel
{
    [ObservableProperty] private IEnumerable<PlotPoint>? _mainPlotPoints;
    [ObservableProperty] private List<Point>? _peakBoundaries;
    [ObservableProperty] private PeakData? _selectedPeak;

    /// <summary>
    /// The main series of the plot.
    /// </summary>
    public LineSeries MainSeries { get; } = new();

    private readonly LineSeries _peakSeries = new();

    private readonly LinearAxis _xAxis = new()
    {
        Title = "Angle", Unit = "deg", Position = AxisPosition.Bottom, IsPanEnabled = false, IsZoomEnabled = false
    };

    private readonly LinearAxis _yAxis = new()
    {
        Title = "Intensity", Unit = "a.u.", Position = AxisPosition.Left, IsPanEnabled = false,
        IsZoomEnabled = false, Minimum = 0
    };

    /// <summary>
    /// Constructs a diffractogram plot model.
    /// </summary>
    public DiffractogramPlotModel()
    {
        PropertyChanged += UpdatePlot;

        Title = "Diffractogram data";
        Axes.Add(_xAxis);
        Axes.Add(_yAxis);
        Series.Add(MainSeries);
    }

    partial void OnMainPlotPointsChanged(IEnumerable<PlotPoint>? value)
    {
        MainSeries.ItemsSource = value?.Select(point => new DataPoint(point.Angle, point.Intensity));
    }

    partial void OnSelectedPeakChanged(PeakData? value)
    {
        Series.Clear();
        Annotations.Clear();

        if (value == null)
        {
            Series.Add(MainSeries);
            return;
        }

        Annotations.Add(new LineAnnotation()
        {
            Type = LineAnnotationType.Vertical,
            X = value.Angle,
            LineStyle = LineStyle.Dash,
            StrokeThickness = 1,
            Color = OxyColors.Red
        });

        var annotation = new PolylineAnnotation();
        annotation.Points.Add(new DataPoint(value.LeftBoundary.X, value.LeftBoundary.Y));
        annotation.Points.Add(new DataPoint(value.RightBoundary.X, value.RightBoundary.Y));
        Annotations.Add(annotation);

        _peakSeries.ItemsSource = value.XrayPeak.points.Select(point => new DataPoint(point.X, point.Y));
        Series.Add(_peakSeries);
    }

    private void UpdatePlot(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        InvalidatePlot(true);
    }
}
