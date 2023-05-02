using System;
using System.Drawing;
using System.Drawing.Imaging;
using Cringules.NGram.App.Model;
using OxyPlot;
using OxyPlot.Core.Drawing;

namespace Cringules.NGram.App.Export;

public class EmfResultExporter : IResultExporter
{
    public string FileExtension => ".emf";

    public void Export(WorkSession session, string filename)
    {
        const int width = 800;
        const int height = 500;

        IPlotModel model = session.Model;

        using Metafile metafile = CreateMetafile(filename, width, height);
        using Graphics graphics = Graphics.FromImage(metafile);

        if (model.Background.IsVisible())
        {
            using var brush = model.Background.ToBrush();
            graphics.FillRectangle(brush, 0, 0, width, height);
        }

        using var graphicsRenderContext = new GraphicsRenderContext(graphics);
        graphicsRenderContext.RendersToScreen = false;

        model.Update(true);
        model.Render(graphicsRenderContext, new OxyRect(0.0, 0.0, width, height));
    }

    private static Metafile CreateMetafile(string filename, int width, int height)
    {
        using var bitmap = new Bitmap(width, height);
        using Graphics graphics = Graphics.FromImage(bitmap);
        IntPtr hdc = graphics.GetHdc();
        var metafile = new Metafile(filename, hdc);
        graphics.ReleaseHdc();
        return metafile;
    }
}
