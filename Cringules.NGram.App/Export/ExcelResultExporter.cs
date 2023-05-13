using System.IO;
using ClosedXML.Report;
using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.Export;

public class ExcelResultExporter : IResultExporter
{
    public string FileExtension => ".xlsx";
    public void Export(WorkSession session, string filename)
    {
        Stream? stream = typeof(ExcelResultExporter).Assembly.GetManifestResourceStream(
            "Cringules.NGram.App.Resources.report_template.xlsx");
        var template = new XLTemplate(stream);
        
        template.AddVariable(session);
        template.AddVariable(@"DataPoints", session.Data.Points);
        template.AddVariable(@"SmoothedDataPoints", session.SmoothedData?.Points);
        template.Generate();
        template.SaveAs(filename);
    }
}
