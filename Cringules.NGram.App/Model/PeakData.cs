using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.App.Resources;
using Cringules.NGram.Lib;
using Cringules.NGram.Lib.Approximation;

namespace Cringules.NGram.App.Model;

public partial class PeakData : ObservableObject
{
    private static readonly NamedItem<IApproximator>
        GaussianApproximator = new(Strings.Gaussian, new ApproximationGaussian());

    private static readonly NamedItem<IApproximator>
        LorentzApproximator = new(Strings.Lorentzian, new ApproximationLorentz());

    private static readonly NamedItem<IApproximator>
        VoigtApproximator = new(Strings.Voigt, new ApproximationVoigt());

    public double Angle { get; }
    public double Distance { get; }
    public double MaxIntensity { get; }
    public double IntegralIntensity { get; }
    public Point Top { get; }
    public Point LeftBoundary { get; }
    public Point RightBoundary { get; }

    public XrayPeak XrayPeak { get; }
    public XrayPeak? Approximation { get; private set; }

    [ObservableProperty] private ObservableCollection<NamedItem<IApproximator>> _availableApproximators =
        new() {GaussianApproximator, LorentzApproximator, VoigtApproximator};

    [ObservableProperty] private NamedItem<IApproximator> _approximator = VoigtApproximator;

    [ObservableProperty] private bool _automaticApproximation = true;

    [ObservableProperty] private double _height;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _corr;
    [ObservableProperty] private double _lambda;

    public PeakData(XrayPeak peak)
    {
        Top = peak.GetPeakTop();
        LeftBoundary = peak.Points[0];
        RightBoundary = peak.Points[^1];

        var analyzer = new XrayPeakAnalyzer(peak);
        Angle = analyzer.GetTopAngle();
        Distance = 0; // TODO Implement
        MaxIntensity = analyzer.GetIntensityMax();
        IntegralIntensity = analyzer.GetIntensityIntegral();

        XrayPeak = peak;
    }

    private void Approximate()
    {
        if (AutomaticApproximation)
        {
            Approximation = new XrayPeak(Approximator.Value.ApproximatePeakAuto(XrayPeak).Points);
        }

        Approximation =
            new XrayPeak(Approximator.Value.ApproximatePeakManual(XrayPeak, Height, Width, Corr, Lambda).Points);
    }
}
