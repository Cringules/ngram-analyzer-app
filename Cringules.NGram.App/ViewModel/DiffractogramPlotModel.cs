using System.Collections.Generic;
using System.Linq;
using Cringules.NGram.Api;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a diffractogram plot model.
/// </summary>
public class DiffractogramPlotModel : PlotModel
{
    /// <summary>
    /// The main series of the plot.
    /// </summary>
    private readonly LineSeries _mainSeries;

    /// <summary>
    /// Constructs a diffractogram plot model.
    /// </summary>
    public DiffractogramPlotModel()
    {
        Title = "test";
        Axes.Add(new LinearAxis
        {
            Title = "Angle", Unit = "deg", Position = AxisPosition.Bottom, IsPanEnabled = false, IsZoomEnabled = false
        });
        Axes.Add(new LinearAxis
        {
            Title = "Intensity", Unit = "a.u.", Position = AxisPosition.Left, IsPanEnabled = false,
            IsZoomEnabled = false
        });
        _mainSeries = new LineSeries();
        Series.Add(_mainSeries);
    }

    /// <summary>
    /// Updates the plot data.
    /// </summary>
    /// <param name="points">An enumerable of the data points.</param>
    public void Update(IEnumerable<PlotPoint>? points)
    {
        _mainSeries.ItemsSource = points?.Select(point => new DataPoint(point.Angle, point.Intensity));
        InvalidatePlot(true);
    }
}
