using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public abstract class PerifericoControllerBase<THubController> : IPerifericoController where THubController : IHubController
    {
        public abstract string Modelo { get; }

        protected virtual THubController Controller { get; private set; }

        public virtual ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public virtual void SetHubController(IHubController hubController)
        {
            Controller = (THubController)hubController;
        }
    }
}
