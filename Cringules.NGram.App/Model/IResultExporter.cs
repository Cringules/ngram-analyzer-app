namespace Cringules.NGram.App.Model;

/// <summary>
/// Represents a result exporter.
/// </summary>
public interface IResultExporter
{
    /// <summary>
    /// Exports analysis results to a file.
    /// </summary>
    /// <param name="filename">Filename of the export file.</param>
    public void ExportResult(string filename);
}
