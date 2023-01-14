using System.Collections.Generic;
using Cringules.NGram.Api;

namespace Cringules.NGram.App.Model;

/// <summary>
/// Includes plot cleaning methods.
/// </summary>
public static class PlotCleaner
{
    /// <summary>
    /// Cleans the plot data.
    /// </summary>
    /// <param name="data">Raw plot data.</param>
    /// <returns>Cleaned plot data.</returns>
    public static PlotData GetCleanedPlot(PlotData data)
    {
        if (data.Points.Count < 2)
        {
            return data;
        }

        // TODO: Do actual implementation
        var points = new List<PlotPoint>(data.Points.Count);
        for (var i = 0; i < data.Points.Count; i++)
        {
            PlotPoint point = data.Points[i];
            PlotPoint previous = i == 0 ? point : data.Points[i - 1];
            PlotPoint next = i == data.Points.Count - 1 ? point : data.Points[i + 1];

            points.Add(new PlotPoint(point.Angle,
                0.5 * point.Intensity + 0.25 * (previous.Intensity + next.Intensity)));
        }

        return new PlotData(points);
    }
}
