using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cringules.NGram.App.Resources;
using Cringules.NGram.Lib;
using Cringules.NGram.Lib.Approximation;

namespace Cringules.NGram.App.Model;

public partial class PeakData : ObservableObject
{
    private readonly double _waveLength;
    private static readonly NamedItem<SymmetrizeType> LeftSymmetrize = new("Left", SymmetrizeType.Left);
    private static readonly NamedItem<SymmetrizeType> RightSymmetrize = new("Right", SymmetrizeType.Right);

    private static readonly NamedItem<IApproximator>
        GaussianApproximator = new(Strings.Gaussian, new ApproximationGaussian());

    private static readonly NamedItem<IApproximator>
        LorentzApproximator = new(Strings.Lorentzian, new ApproximationLorentz());

    private static readonly NamedItem<IApproximator>
        VoigtApproximator = new(Strings.Voigt, new ApproximationVoigt());

    public double Angle { get; private set; }
    public double Distance { get; private set; }
    public double MaxIntensity { get; private set; }
    public double IntegralIntensity { get; private set; }
    public double FullWidthHalfMaximum { get; private set; }
    public double IntegralWidth { get; private set; }
    public Point Top { get; private set; }
    public Point LeftBoundary { get; private set; }
    public Point RightBoundary { get; private set; }

    [ObservableProperty] private XrayPeak _xrayPeak;

    [ObservableProperty] private NamedItem<SymmetrizeType> _selectedSymmetrizeType = LeftSymmetrize;

    [ObservableProperty]
    private ObservableCollection<NamedItem<SymmetrizeType>> _symmetrizeTypes = new() {LeftSymmetrize, RightSymmetrize};

    [ObservableProperty] private XrayPeak _symmetrized;

    private ApproximationResult? _approximationResult;
    [ObservableProperty] private XrayPeak? _approximation;

    private readonly XrayPeakAnalyzer _analyzer;

    // TODO Fix this
    [ObservableProperty] private ObservableCollection<NamedItem<IApproximator>> _availableApproximators =
        new() {GaussianApproximator, LorentzApproximator/*, VoigtApproximator*/};

    [ObservableProperty] private NamedItem<IApproximator> _approximator = /*VoigtApproximator*/ LorentzApproximator;

    [ObservableProperty] private bool _automaticApproximation = true;

    [ObservableProperty] private double _height;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _corr;
    [ObservableProperty] private double _lambda;

    [ObservableProperty] private double? _approximatedIntegralIntensity;
    [ObservableProperty] private double? _approximationAccuracy;

    public PeakData(XrayPeak peak, double waveLength)
    {
        _waveLength = waveLength;
        _analyzer = new XrayPeakAnalyzer();

        XrayPeak = peak;
        CalculateAll();
    }

    [RelayCommand]
    public void Approximate()
    {
        _approximationResult = AutomaticApproximation
            ? Approximator.Value.ApproximatePeakAuto(Symmetrized)
            : Approximator.Value.ApproximatePeakManual(Symmetrized, Height, Width, Corr, Lambda);

        Approximation = new XrayPeak(_approximationResult.Value.Points);
        ApproximatedIntegralIntensity = _analyzer.GetIntensityApproximated(Symmetrized, _approximationResult.Value);
        ApproximationAccuracy = _analyzer.GetIntensityDifference(Symmetrized, _approximationResult.Value);
    }

    private void CalculateAll()
    {
        Symmetrized = SelectedSymmetrizeType.Value == SymmetrizeType.Left
            ? XrayPeak.SymmetrizePeakLeft()
            : XrayPeak.SymmetrizePeakRight();
        
        Top = Symmetrized.GetPeakTop();
        LeftBoundary = Symmetrized.Points[0];
        RightBoundary = Symmetrized.Points[^1];
        
        Angle = _analyzer.GetTopAngle(Symmetrized);
        Distance = _analyzer.GetInterplanarDistance(Symmetrized, _waveLength);
        MaxIntensity = _analyzer.GetIntensityMaximum(Symmetrized);
        IntegralIntensity = _analyzer.GetIntensityIntegral(Symmetrized);
        FullWidthHalfMaximum = _analyzer.GetPeakWidth(Symmetrized);
        IntegralWidth = _analyzer.GetIntegralWidth(Symmetrized);

        Approximate();
    }

    partial void OnXrayPeakChanged(XrayPeak value)
    {
        CalculateAll();
    }

    

    partial void OnApproximatorChanged(NamedItem<IApproximator> value)
    {
        CalculateAll();
    }


    partial void OnSelectedSymmetrizeTypeChanged(NamedItem<SymmetrizeType> value)
    {
        CalculateAll();
    }
}
