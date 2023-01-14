using System.Collections.Generic;
using Cringules.NGram.Api;

namespace Cringules.NGram.App.Model;

public class PeakFinder
{
    public static List<Peak> FindPeaks(PlotData? data)
    {
        var peaks = new List<Peak>();
        if (data == null)
        {
            return peaks;
        }
        
        for (var i = 1; i < data.Points.Count - 1; i++)
        {
            if (data.Points[i - 1].Intensity < data.Points[i].Intensity &&
                data.Points[i + 1].Intensity < data.Points[i].Intensity)
            {
                peaks.Add(new Peak(data.Points[i].Angle, 0, data.Points[i].Intensity, 0));
            }
        }

        return peaks;
    }
}
