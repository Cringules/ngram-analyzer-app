using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.Api;
using Cringules.NGram.App.Model;
using Cringules.NGram.Lib;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a diffractogram plot model.
/// </summary>
public partial class DiffractogramPlotModel : DiffractionDataPlotModel
{
    [ObservableProperty] private List<Point>? _peakBoundaries;

    [ObservableProperty] private List<Point> _selectedBoundary = new(2);

    private List<LineAnnotation> _selectionAnnotations = new(2);

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
