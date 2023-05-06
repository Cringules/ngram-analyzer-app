using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.App.Resources;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

[ObservableObject]
public partial class DiffractionDataPlotModel : PlotModel
{
    [ObservableProperty] private IEnumerable<DataPoint>? _plotPoints;

    /// <summary>
    /// The main series of the plot.
    /// </summary>
    public LineSeries MainSeries { get; } = new();

    private readonly LinearAxis _xAxis = new()
    {
        Title = Strings.Angle, Unit = Strings.Degrees, Position = AxisPosition.Bottom, IsPanEnabled = false,
        IsZoomEnabled = false
    };

    private readonly LinearAxis _yAxis = new()
    {
        Title = Strings.Intensity, Unit = Strings.AbsoluteUnits, Position = AxisPosition.Left, IsPanEnabled = false,
        IsZoomEnabled = false
    };

    /// <summary>
    /// Constructs a diffractogram plot model.
    /// </summary>
    public DiffractionDataPlotModel(string title)
    {
        PropertyChanged += UpdatePlot;

        Title = title;
        Axes.Add(_xAxis);
        Axes.Add(_yAxis);
        Series.Add(MainSeries);
    }

    partial void OnPlotPointsChanged(IEnumerable<DataPoint>? value)
    {
        MainSeries.ItemsSource = value;
    }

    private void UpdatePlot(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        InvalidatePlot(true);
    }
}
