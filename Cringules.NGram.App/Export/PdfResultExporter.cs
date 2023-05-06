using Cringules.NGram.App.Model;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Cringules.NGram.App.Export;

public class PdfResultExporter : IResultExporter
{
    static PdfResultExporter()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public string FileExtension => ".pdf";

    public void Export(WorkSession session, string filename)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var sessionReportDocument = new SessionReportDocument(session);
        sessionReportDocument.GeneratePdf(filename);
    }
}
