using System.Collections.Generic;
using Cringules.NGram.Api;

namespace Cringules.NGram.App.Model;

public class SessionOpener
{
    public static WorkSession OpenSession(string filename)
    {
        // TODO: Deserialization or smth
        return new WorkSession(new PlotData(new List<PlotPoint>()));
    }
}
