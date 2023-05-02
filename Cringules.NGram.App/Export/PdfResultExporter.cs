using Cringules.NGram.App.Model;
using QuestPDF.Fluent;

namespace Cringules.NGram.App.Export;

public class PdfResultExporter : IResultExporter
{
    public string FileExtension => ".pdf";

    public void Export(WorkSession session, string filename)
    {
        var sessionReportDocument = new SessionReportDocument(session);
        sessionReportDocument.GeneratePdf(filename);
    }
}
