using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cringules.NGram.App.Model.SessionFile;

namespace Cringules.NGram.App.Model;

public static class SessionFileManager
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = {new JsonStringEnumConverter()}
    };

    public static WorkSession OpenSession(string filename)
    {
        using FileStream stream = File.OpenRead(filename);
        return JsonSerializer.Deserialize<SessionDto>(stream, Options)?.ToWorkSession() ??
               throw new InvalidOperationException();
    }

    public static void SaveSession(WorkSession session, string filename)
    {
        using FileStream stream = File.Create(filename);
        JsonSerializer.Serialize(stream, session.ToDto(), Options);
    }
}
