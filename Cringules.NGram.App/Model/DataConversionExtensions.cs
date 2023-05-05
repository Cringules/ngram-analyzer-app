using System.Collections.Generic;
using System.Linq;
using Cringules.NGram.Api;
using Cringules.NGram.Lib;
using OxyPlot;

namespace Cringules.NGram.App.Model;

public static class DataConversionExtensions
{
    public static Xray ToXray(this PlotData data)
    {
        return new Xray(data.Points.Select(point => new Point(point.Angle, point.Intensity)));
    }

    public static IEnumerable<DataPoint> ToPlotPoints(this Xray xray)
    {
        return xray.Points.Select(point => new DataPoint(point.X, point.Y));
    }
    
    public static IEnumerable<DataPoint> ToPlotPoints(this XrayPeak peak)
    {
        return peak.Points.Select(point => new DataPoint(point.X, point.Y));
    }
}
