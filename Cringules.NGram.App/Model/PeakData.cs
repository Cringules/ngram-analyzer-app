using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model;

public class PeakData
{
    public double Angle { get; }
    public double Distance { get; }
    public double MaxIntensity { get; }
    public double IntegralIntensity { get; }
    public Point Top { get; }
    public Point LeftBoundary { get; }
    public Point RightBoundary { get; }
    
    public XrayPeak XrayPeak { get; }

    public PeakData(XrayPeak peak)
    {
        Top = peak.GetPeakTop();
        LeftBoundary = peak.points[0];
        RightBoundary = peak.points[^1];
        
        var analyzer = new XrayPeakAnalyzer(peak);
        Angle = analyzer.GetTopAngle();
        Distance = 0; // TODO Implement
        MaxIntensity = analyzer.GetIntensityMax();
        IntegralIntensity = analyzer.GetIntensityIntegral();

        XrayPeak = peak;
    }
}
