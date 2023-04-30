using Cringules.NGram.App.Model;

namespace Cringules.NGram.App.Export;

/// <summary>
/// Represents a result exporter.
/// </summary>
public interface IResultExporter
{
    /// <summary>
    /// Exports analysis results to a file.
    /// </summary>
    /// <param name="session">Work session containing results.</param>
    /// <param name="filename">Filename of the export file.</param>
    void Export(WorkSession session, string filename);
}
