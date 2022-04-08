using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public interface IPerifericoController : IPeriferico
    {
        void SetHubController(IHubController hubController);
    }
}
