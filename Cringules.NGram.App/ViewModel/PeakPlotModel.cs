using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.App.Model;
using Cringules.NGram.App.Resources;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

public partial class PeakPlotModel : DiffractionDataPlotModel
{
    [ObservableProperty] private PeakData? _selectedPeak;

    private readonly LineSeries _approximationSeries = new();

    public PeakPlotModel() : base(Strings.PeakDataHeader)
    {
        Series.Add(_approximationSeries);
    }

    private void UpdateData()
    {
        Annotations.Clear();

        if (SelectedPeak == null)
        {
            return;
        }

        Annotations.Add(new LineAnnotation()
        {
            Type = LineAnnotationType.Vertical,
            X = SelectedPeak.Angle,
            LineStyle = LineStyle.Dash,
            StrokeThickness = 1,
            Color = OxyColors.Red
        });

        var annotation = new PolylineAnnotation();
        annotation.Points.Add(new DataPoint(SelectedPeak.SymmetrizedLeftBoundary.X, SelectedPeak.SymmetrizedLeftBoundary.Y));
        annotation.Points.Add(new DataPoint(SelectedPeak.SymmetrizedRightBoundary.X, SelectedPeak.SymmetrizedRightBoundary.Y));
        Annotations.Add(annotation);

        PlotPoints = SelectedPeak.Symmetrized.ToPlotPoints();
        _approximationSeries.ItemsSource = SelectedPeak.Approximation?.ToPlotPoints();
    }


    partial void OnSelectedPeakChanged(PeakData? oldValue, PeakData? newValue)
    {
        if (oldValue != null)
        {
            oldValue.PropertyChanged -= SelectedPeakOnPropertyChanged;
        }

        if (newValue != null)
        {
            newValue.PropertyChanged += SelectedPeakOnPropertyChanged;
        }
        
        UpdateData();
    }

    private void SelectedPeakOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateData();
    }
}
