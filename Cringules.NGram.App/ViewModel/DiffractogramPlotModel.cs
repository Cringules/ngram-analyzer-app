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
    public LineSeries MainSeries { get; }

    /// <summary>
    /// Constructs a diffractogram plot model.
    /// </summary>
    public DiffractogramPlotModel()
    {
        Title = "Diffractogram data";
        Axes.Add(new LinearAxis
        {
            Title = "Angle", Unit = "deg", Position = AxisPosition.Bottom, IsPanEnabled = false, IsZoomEnabled = false
        });
        Axes.Add(new LinearAxis
        {
            Title = "Intensity", Unit = "a.u.", Position = AxisPosition.Left, IsPanEnabled = false,
            IsZoomEnabled = false
        });
        MainSeries = new LineSeries();
        Series.Add(MainSeries);
    }

    /// <summary>
    /// Updates the plot data.
    /// </summary>
    /// <param name="points">An enumerable of the data points.</param>
    public void Update(IEnumerable<PlotPoint>? points)
    {
        MainSeries.ItemsSource = points?.Select(point => new DataPoint(point.Angle, point.Intensity));
        InvalidatePlot(true);
    }
}
