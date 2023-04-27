using System.IO;
using System.Text.Json;

namespace Cringules.NGram.App.Model;

/// <summary>
/// Represents a session saver.
/// </summary>
public static class SessionSaver
{
    public static void SaveSession(WorkSession session, string filename)
    {
        using FileStream stream = File.Create(filename);
        JsonSerializer.Serialize(stream, session);
    }
}
