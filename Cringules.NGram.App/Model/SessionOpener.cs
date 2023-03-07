using System;
using System.IO;
using System.Text.Json;

namespace Cringules.NGram.App.Model;

public static class SessionOpener
{
    public static WorkSession OpenSession(string filename)
    {
        using FileStream stream = File.OpenRead(filename);
        return JsonSerializer.Deserialize<WorkSession>(stream) ?? throw new InvalidOperationException();
    }
}
