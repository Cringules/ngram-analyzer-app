using Cringules.NGram.App.Model;
using Cringules.NGram.App.Resources;
using OxyPlot;
using OxyPlot.SkiaSharp;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Cringules.NGram.App.Export;

public class SessionReportDocument : IDocument
{
    private readonly WorkSession _workSession;

    public SessionReportDocument(WorkSession workSession)
    {
        _workSession = workSession;
    }

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Content().Column(column =>
            {
                column.Item()
                    .Height(500)
                    .Canvas((canvas, space) =>
                    {
                        using var context = new SkiaRenderContext
                        {
                            RenderTarget = RenderTarget.VectorGraphic, SkCanvas = canvas, UseTextShaping = true
                        };
                        const float dpiScale = 72f / 96;
                        context.DpiScale = dpiScale;

                        IPlotModel model = _workSession.Model;
                        model.Update(true);
                        canvas.Clear(model.Background.ToSKColor());
                        model.Render(context, new OxyRect(0, 0, space.Width / dpiScale, space.Height / dpiScale));
                    });

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text(Strings.Angle);
                        header.Cell().Text(Strings.Distance);
                        header.Cell().Text(Strings.MaxIntensity);
                        header.Cell().Text(Strings.IntergralIntensity);
                    });

                    foreach (PeakData peak in _workSession.Peaks)
                    {
                        table.Cell().Text(string.Format(Strings.AngleDegreesTemplate, peak.Angle));
                        table.Cell().Text(string.Format(Strings.DistanceAngstromTemplate, peak.Distance));
                        table.Cell().Text(string.Format(Strings.IntensityAbsoluteUnitsTemplate, peak.MaxIntensity));
                        table.Cell().Text(string.Format(Strings.IntensityAbsoluteUnitsTemplate,
                            peak.IntegralIntensity));
                    }
                });
            });
        });
    }
}
