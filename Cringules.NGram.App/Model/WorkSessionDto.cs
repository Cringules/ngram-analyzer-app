using System.Collections.Generic;
using Cringules.NGram.Api;
using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model;

public class WorkSessionDto
{
    public PlotData Data { get; init; }
    public List<Peak> Peaks { get; init; }
    public double WaveLength { get; init; }
}
