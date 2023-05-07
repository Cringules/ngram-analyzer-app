using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model.SessionFile;

public static class SessionFileConversionExtensions
{
    public static SessionDto ToDto(this WorkSession session)
    {
        return new SessionDto(session.Data.Points.ToDto(),
            session.WaveLength, session.SmoothingEnabled, session.SmoothingDegree,
            session.Peaks.Select(data => data.ToDto()).ToList());
    }

    public static WorkSession ToWorkSession(this SessionDto dto)
    {
        var xray = new Xray(dto.Data.ToPoints());

        var session = new WorkSession(xray)
        {
            WaveLength = dto.WaveLength,
            SmoothingEnabled = dto.Smoothed,
            SmoothingDegree = dto.SmoothingDegree
        };
        
        if (dto.Smoothed)
        {
            session.SmoothData();
            xray = session.SmoothedData!;
        }

        session.Peaks = new ObservableCollection<PeakData>(dto.Peaks.Select(peakDto =>
            peakDto.ToPeakData(xray, dto.WaveLength)));

        return session;
    }

    public static List<PointDto> ToDto(this IEnumerable<Point> points)
    {
        return points.Select(point => new PointDto(point.X, point.Y)).ToList();
    }
    
    public static List<Point> ToPoints(this IEnumerable<PointDto> dto)
    {
        return dto.Select(pointDto => new Point(pointDto.X, pointDto.Y)).ToList();
    }

    public static PeakDto ToDto(this PeakData peak)
    {
        return new PeakDto(peak.LeftBoundary.X, peak.RightBoundary.X, ApproximatorTypeDto.Voigt);
    }
    
    public static PeakData ToPeakData(this PeakDto dto, Xray xray, double lambda)
    {
        XrayPeak peak = xray.GetPeak(dto.LeftBoundary, dto.RightBoundary);
        var peakData = new PeakData(peak, lambda);
        peakData.Approximate();
        return peakData;
    }
}
