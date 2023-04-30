using Cringules.NGram.App.Model;
using OxyPlot.SkiaSharp;

namespace Cringules.NGram.App.Export;

public class PdfResultExporter : IResultExporter
{
    public void Export(WorkSession session, string filename)
    {
        PdfExporter.Export(session.Model, filename, 800, 500);
    }
}
