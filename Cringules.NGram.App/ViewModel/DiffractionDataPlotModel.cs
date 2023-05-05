using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.Api;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

[ObservableObject]
public partial class DiffractionDataPlotModel : PlotModel
{
    [ObservableProperty] private IEnumerable<PlotPoint>? _plotPoints;

    /// <summary>
    /// The main series of the plot.
    /// </summary>
    public LineSeries MainSeries { get; } = new();

    protected readonly LinearAxis XAxis = new()
    {
        Title = "Angle", Unit = "deg", Position = AxisPosition.Bottom, IsPanEnabled = false, IsZoomEnabled = false
    };

    protected readonly LinearAxis YAxis = new()
    {
        Title = "Intensity", Unit = "a.u.", Position = AxisPosition.Left, IsPanEnabled = false,
        IsZoomEnabled = false, Minimum = 0
    };

    /// <summary>
    /// Constructs a diffractogram plot model.
    /// </summary>
    public DiffractionDataPlotModel()
    {
        PropertyChanged += UpdatePlot;

        Title = "Diffractogram data";
        Axes.Add(XAxis);
        Axes.Add(YAxis);
        Series.Add(MainSeries);
    }

    partial void OnPlotPointsChanged(IEnumerable<PlotPoint>? value)
    {
        MainSeries.ItemsSource = value?.Select(point => new DataPoint(point.Angle, point.Intensity));
    }

    private void UpdatePlot(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        InvalidatePlot(true);
    }
}
