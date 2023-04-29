using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

public class PlotSelectionManipulator : MouseManipulator
{
    private readonly DiffractogramPlotModel _plotModel;
    private LineAnnotation _currentAnnotation;

    public PlotSelectionManipulator(IPlotView plotView, DiffractogramPlotModel plotModel) : base(plotView)
    {
        _plotModel = plotModel;
    }

    private void CreateLine(OxyMouseEventArgs e)
    {
        
    }

    public override void Completed(OxyMouseEventArgs e)
    {
        base.Completed(e);
    }

    public override void Delta(OxyMouseEventArgs e)
    {
        base.Delta(e);
        e.Handled = true;

        LineSeries series = _plotModel.MainSeries;

        DataPoint point = series.InverseTransform(e.Position);

        _currentAnnotation.X = point.X;
        _plotModel.InvalidatePlot(false);
    }

    public override void Started(OxyMouseEventArgs e)
    {
        base.Started(e);
        
        _currentAnnotation = new LineAnnotation()
        {
            StrokeThickness = 2,
            LineStyle = LineStyle.Solid,
            Color = OxyColors.Red,
            Type = LineAnnotationType.Vertical
        };
        _plotModel.Annotations.Add(_currentAnnotation);
        
        Delta(e);
    }
}
