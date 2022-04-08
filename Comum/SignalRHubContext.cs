using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public class SignalRHubContext
    {
        public SignalRHubContext(string connectionId)
        {
            this.ConnectionId = connectionId;
        }

        public string ConnectionId { get; }
    }

    public class SignalRHubContext<TContextValue> : SignalRHubContext
    {
        public SignalRHubContext(string clientConnectionId, TContextValue value) : base(clientConnectionId)
        {
            this.Value = value;
        }

        public TContextValue Value { get; }
    }
}
