using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.App.Model;
using OxyPlot;
using OxyPlot.Annotations;

namespace Cringules.NGram.App.ViewModel;

public partial class PeakPlotModel : DiffractionDataPlotModel
{
    [ObservableProperty] private PeakData? _selectedPeak;

    public PeakPlotModel() : base("Peak Data")
    {
    }

    partial void OnSelectedPeakChanged(PeakData? value)
    {
        Annotations.Clear();

        if (value == null)
        {
            return;
        }

        Annotations.Add(new LineAnnotation()
        {
            Type = LineAnnotationType.Vertical,
            X = value.Angle,
            LineStyle = LineStyle.Dash,
            StrokeThickness = 1,
            Color = OxyColors.Red
        });

        var annotation = new PolylineAnnotation();
        annotation.Points.Add(new DataPoint(value.LeftBoundary.X, value.LeftBoundary.Y));
        annotation.Points.Add(new DataPoint(value.RightBoundary.X, value.RightBoundary.Y));
        Annotations.Add(annotation);

        PlotPoints = value.XrayPeak.ToPlotPoints();
    }
}
