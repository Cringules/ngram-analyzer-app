using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.Lib;
using OxyPlot;
using OxyPlot.Annotations;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a diffractogram plot model.
/// </summary>
public partial class DiffractogramPlotModel : DiffractionDataPlotModel
{
    [ObservableProperty] private List<Point>? _peakBoundaries;

    [ObservableProperty] private List<Point> _selectedBoundary = new(2);

    private readonly List<LineAnnotation> _selectionAnnotations = new(2);

    public LineAnnotation AddSelectedPoint()
    {
        if (_selectionAnnotations.Count == 2)
        {
            Annotations.Remove(_selectionAnnotations[0]);
            Annotations.Remove(_selectionAnnotations[1]);
            _selectionAnnotations.Clear();
            SelectedBoundary.Clear();
        }

        var annotation = new LineAnnotation()
        {
            StrokeThickness = 2,
            LineStyle = LineStyle.Solid,
            Color = OxyColors.Red,
            Type = LineAnnotationType.Vertical
        };
        
        _selectionAnnotations.Add(annotation);
        Annotations.Add(annotation);

        InvalidatePlot(true);

        return annotation;
    }
}
