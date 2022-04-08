using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public abstract class HubBase<TController> : Microsoft.AspNetCore.SignalR.Hub
        where TController : IHubController
    {
        private readonly HubManager manager;
        private readonly Lazy<TController> controller;

        public HubBase(HubManager manager)
        {
            this.manager = manager;
            controller = new Lazy<TController>(() => this.manager.GetController<TController>(), true);
        }

        protected TController Controller { get { return controller.Value; } }
    }
}
