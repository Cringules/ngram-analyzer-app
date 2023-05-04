using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Lib;

namespace Cringules.NGram.App.Model;

public partial class PeakData : ObservableObject
{
    public double Angle { get; }
    public double Distance { get; }
    public double MaxIntensity { get; }
    public double IntegralIntensity { get; }
    public Point Top { get; }
    public Point LeftBoundary { get; }
    public Point RightBoundary { get; }

    public XrayPeak XrayPeak { get; }
    public XrayPeak? Approximation { get; private set; }

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(Approximate))]
    private bool _determineApproximator = true;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(Approximate))]
    private IAutoApproximator? _autoApproximator;

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

    private bool CanApproximate()
    {
        return DetermineApproximator || AutoApproximator != null;
    }

    [RelayCommand(CanExecute = nameof(CanApproximate))]
    private void Approximate()
    {
        if (DetermineApproximator)
        {
            // TODO: Implement automatic approximation
            return;
        }

        if (AutoApproximator == null)
        {
            return;
        }

        Approximation = AutoApproximator.ApproximatePeakAuto(XrayPeak);
    }
}
