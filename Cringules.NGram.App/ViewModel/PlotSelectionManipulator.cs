using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cringules.NGram.App.ViewModel;

public class PlotSelectionManipulator : MouseManipulator
{
    private readonly DiffractogramPlotModel _plotModel;

    public PlotSelectionManipulator(IPlotView plotView, DiffractogramPlotModel plotModel) : base(plotView)
    {
        _plotModel = plotModel;
    }

    public override void Completed(OxyMouseEventArgs e)
    {
        base.Completed(e);
        e.Handled = true;
        
        _plotModel.FinishSelection();
    }

    public override void Delta(OxyMouseEventArgs e)
    {
        base.Delta(e);
        e.Handled = true;

        DataPoint point = Axis.InverseTransform(e.Position, XAxis, YAxis);
        _plotModel.UpdateSelection(point);
    }

    public override void Started(OxyMouseEventArgs e)
    {
        base.Started(e);
        
        Delta(e);
    }
}
