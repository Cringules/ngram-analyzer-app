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
    public double FullWidthHalfMaximum { get; }
    public double IntegralWidth { get; }
    public Point Top { get; }
    public Point LeftBoundary { get; }
    public Point RightBoundary { get; }

    public XrayPeak XrayPeak { get; }
    private ApproximationResult? _approximationResult;
    public XrayPeak? Approximation { get; private set; }

    private readonly XrayPeakAnalyzer _analyzer;

    [ObservableProperty] private ObservableCollection<NamedItem<IApproximator>> _availableApproximators =
        new() {GaussianApproximator, LorentzApproximator, VoigtApproximator};

    [ObservableProperty] private NamedItem<IApproximator> _approximator = VoigtApproximator;

    [ObservableProperty] private bool _automaticApproximation = true;

    [ObservableProperty] private double _height;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _corr;
    [ObservableProperty] private double _lambda;

    [ObservableProperty] private double? _approximatedIntegralIntensity;
    [ObservableProperty] private double? _approximationAccuracy;

    public PeakData(XrayPeak peak, double lambda)
    {
        Top = peak.GetPeakTop();
        LeftBoundary = peak.Points[0];
        RightBoundary = peak.Points[^1];

        _analyzer = new XrayPeakAnalyzer(peak);
        Angle = _analyzer.GetTopAngle();
        Distance = _analyzer.GetInterplaneDistance(lambda);
        MaxIntensity = _analyzer.GetIntensityMaximum();
        IntegralIntensity = _analyzer.GetIntensityIntegral();
        FullWidthHalfMaximum = _analyzer.GetPeakWidth();
        IntegralWidth = _analyzer.GetIntegralWidth();

        XrayPeak = peak;
    }

    public void Approximate()
    {
        _approximationResult = AutomaticApproximation
            ? Approximator.Value.ApproximatePeakAuto(XrayPeak)
            : Approximator.Value.ApproximatePeakManual(XrayPeak, Height, Width, Corr, Lambda);

        Approximation = new XrayPeak(_approximationResult.Value.Points);
        ApproximatedIntegralIntensity = _analyzer.GetIntensityApproximated(_approximationResult.Value);
        ApproximationAccuracy = _analyzer.GetIntensityDifference();
    }
}
