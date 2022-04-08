using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public interface IPeriferico: IAsyncDisposable
    {
        string Modelo { get; }
    }
}
