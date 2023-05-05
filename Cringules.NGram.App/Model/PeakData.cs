using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.Lib;
using Cringules.NGram.Lib.Approximation;

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

    [ObservableProperty] private ObservableCollection<NamedItem<IApproximator>> _availableApproximators = new()
    {
        new NamedItem<IApproximator>("Gaussian", new ApproximationGaussian()),
        new NamedItem<IApproximator>("Lorentz", new ApproximationLorentz()),
        new NamedItem<IApproximator>("Voigt", new ApproximationVoigt())
    };

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ApproximateCommand))]
    private bool _determineApproximator = true;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ApproximateCommand))]
    private NamedItem<IApproximator>? _approximator;

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

    private bool CanApproximate()
    {
        return DetermineApproximator || Approximator != null;
    }

    [RelayCommand(CanExecute = nameof(CanApproximate))]
    private void Approximate()
    {
        if (DetermineApproximator)
        {
            // TODO: Implement automatic approximator selection
            return;
        }

        if (Approximator == null)
        {
            return;
        }

        if (AutomaticApproximation)
        {
            Approximation = Approximator.Value.ApproximatePeakAuto(XrayPeak);
        }

        Approximation = Approximator.Value.ApproximatePeakManual(XrayPeak, Height, Width, Corr, Lambda);
    }
}
