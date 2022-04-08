using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Service.Manager
{
    public class ManagerService : IHostedService
    {
        private readonly HubManager hubManager;
        private readonly IConfiguration configuration;

        public ManagerService(HubManager hubManager, IConfiguration configuration)
        {
            this.hubManager = hubManager;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await hubManager.DisposeAsync();
        }
    }
}
