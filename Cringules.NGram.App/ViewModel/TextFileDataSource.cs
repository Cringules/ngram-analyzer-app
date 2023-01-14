using System;
using System.Collections.Generic;
using System.IO;
using Cringules.NGram.Api;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a text file data source of diffractogram plot data.
/// </summary>
public class TextFileDataSource : IFileDataSource
{
    /// <summary>
    /// Gets diffractogram plot data from text file
    /// </summary>
    /// <param name="filename">Filename of the source file.</param>
    /// <returns>Diffractogram plot data obtained from the source file.</returns>
    public PlotData GetPlotData(string filename)
    {
        var points = new List<PlotPoint>();

        using (var reader = new StreamReader(filename))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                points.Add(ParsePoint(line));
            }
        }

        return new PlotData(points);
    }

    /// <summary>
    /// Parses a data point from a line.
    /// </summary>
    /// <param name="line">The line to parse from.</param>
    /// <returns>Parsed data point.</returns>
    /// <exception cref="FileFormatException">The line has invalid format.</exception>
    private static PlotPoint ParsePoint(string line)
    {
        string[] lines = line.Split((string[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length != 2 || !double.TryParse(lines[0], out double angle) ||
            !double.TryParse(lines[1], out double intensity))
        {
            throw new FileFormatException();
        }

        return new PlotPoint(angle, intensity);
    }
}
