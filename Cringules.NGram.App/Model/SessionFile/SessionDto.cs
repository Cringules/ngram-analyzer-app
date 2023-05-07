using System.Collections.Generic;

namespace Cringules.NGram.App.Model.SessionFile;

public record SessionDto(List<PointDto> Data, double WaveLength, bool Smoothed, int SmoothingDegree,
    List<PeakDto> Peaks);
