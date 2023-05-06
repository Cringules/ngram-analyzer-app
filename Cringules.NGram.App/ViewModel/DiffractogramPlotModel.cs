using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Cringules.NGram.App.Resources;
using Cringules.NGram.Lib;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

/// <summary>
/// Represents a diffractogram plot model.
/// </summary>
public partial class DiffractogramPlotModel : DiffractionDataPlotModel
{
    private LineSeries _smoothedSeries = new();
    [ObservableProperty] private IEnumerable<DataPoint>? _smoothedPoints;

    [ObservableProperty] private List<Point>? _peakBoundaries;

    [ObservableProperty] private bool _canSelect;
    private double? _currentSelection;
    [ObservableProperty] private List<double> _selectedBoundary = new(2);

    private LineAnnotation? _currentAnnotation;
    private readonly List<LineAnnotation> _selectionAnnotations = new(2);

    public DiffractogramPlotModel() : base(Strings.DiffractogramDataHeader)
    {
    }

    private void ClearSelection()
    {
        _currentSelection = null;
        _currentAnnotation = null;
        foreach (LineAnnotation annotation in _selectionAnnotations)
        {
            Annotations.Remove(annotation);
        }

        SelectedBoundary.Clear();
        _selectionAnnotations.Clear();

        InvalidatePlot(true);
    }

    partial void OnCanSelectChanged(bool value)
    {
        ClearSelection();
    }

    public void UpdateSelection(DataPoint point)
    {
        if (!CanSelect)
        {
            return;
        }

        if (_currentAnnotation == null)
        {
            if (_selectionAnnotations.Count == 2)
            {
                ClearSelection();
            }

            _currentAnnotation = new LineAnnotation()
            {
                StrokeThickness = 2,
                LineStyle = LineStyle.Solid,
                Color = OxyColors.Red,
                Type = LineAnnotationType.Vertical
            };

            Annotations.Add(_currentAnnotation);
            _selectionAnnotations.Add(_currentAnnotation);
        }

        _currentAnnotation.X = point.X;
        _currentSelection = point.X;

        InvalidatePlot(true);
    }

    public void FinishSelection()
    {
        if (!CanSelect || _currentAnnotation == null || _currentSelection == null)
        {
            return;
        }

        SelectedBoundary.Add(_currentSelection.Value);
        _currentAnnotation = null;
    }

    partial void OnSmoothedPointsChanged(IEnumerable<DataPoint>? oldValue, IEnumerable<DataPoint>? newValue)
    {
        if (oldValue != null && newValue == null)
        {
            Series.Remove(_smoothedSeries);
        }

        _smoothedSeries.ItemsSource = newValue;

        if (oldValue == null && newValue != null)
        {
            Series.Add(_smoothedSeries);
        }
    }
}
